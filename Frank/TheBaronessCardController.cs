using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class TheBaronessCardController : CardController
    {
        public TheBaronessCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowHeroTargetWithHighestHP();
        }

        public override void AddTriggers()
        {
            AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, DealDamageResponse, TriggerType.DealDamage);
            AddTrigger((DealDamageAction dd) => CheckDamageCriteria(dd), (DealDamageAction dd) => base.GameController.SelectAndDestroyCard(DecisionMaker, new LinqCardCriteria((Card card) => card.IsHero && (IsOngoing(card) || IsEquipment(card)), "hero ongoing or equipment"), optional: false, null, null, GetCardSource()), TriggerType.DestroyCard, TriggerTiming.After);
            AddTrigger((CancelAction c) => c.ActionToCancel is DealDamageAction && c.IsPreventEffect && CheckDamageCriteria(c.ActionToCancel as DealDamageAction), (CancelAction c) => base.GameController.SelectAndDestroyCard(DecisionMaker, new LinqCardCriteria((Card card) => card.IsHero && (IsOngoing(card) || IsEquipment(card)), "hero ongoing or equipment"), optional: false, null, null, GetCardSource()), TriggerType.DestroyCard, TriggerTiming.After);
            AddIncreaseDamageTrigger((DealDamageAction dda) => dda.Target == base.Card && dda.DamageType == DamageType.Radiant, 1);
        }

        private bool CheckDamageCriteria(DealDamageAction dd)
        {
            if (!dd.IsPretend && dd.DamageSource.Card != null && IsVillainTarget(dd.DamageSource.Card) && !dd.DidDealDamage)
            {
                return !dd.DamageModifiers.Any((ModifyDealDamageAction ga) => ga is ImmuneToDamageAction immuneToDamageAction && (dd.Target == null || (immuneToDamageAction.DealDamageAction != null && immuneToDamageAction.DealDamageAction.Target == dd.Target))) && (dd.OriginalAmount > 0 || dd.DamageModifiers.Where((ModifyDealDamageAction ga) => ga is IncreaseDamageAction).Any());
            }
            return false;
        }

        private IEnumerator DealDamageResponse(PhaseChangeAction phaseChange)
        {
            List<Card> storedHero = new List<Card>();
            IEnumerator coroutine2 = base.GameController.FindTargetsWithHighestHitPoints(1, 1, (Card card) => IsHeroTarget(card), storedHero, null, null, evenIfCannotDealDamage: false, optional: false, null, ignoreBattleZone: false, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine2);
            }
            if (storedHero.FirstOrDefault() != null)
            {
                Card hero = storedHero.FirstOrDefault();
                IEnumerator coroutine = DealMultipleInstancesOfDamage(new List<DealDamageAction>
            {
                new DealDamageAction(GetCardSource(), new DamageSource(base.GameController, base.Card), null, 2, DamageType.Melee),
                new DealDamageAction(GetCardSource(), new DamageSource(base.GameController, base.Card), null, 2, DamageType.Infernal)
            }, (Card c) => c == hero);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }
        }
    }
}
