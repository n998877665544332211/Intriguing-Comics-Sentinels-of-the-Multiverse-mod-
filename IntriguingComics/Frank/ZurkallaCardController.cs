using System.Collections;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class ZurkallaCardController : CardController
    {
        public ZurkallaCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowNonVillainTargetWithHighestHP(2);
            base.SpecialStringMaker.ShowDamageDealt(new LinqCardCriteria((Card c) => c == base.CharacterCard), null, null, showTotalAmountOfDamageDealt: false, null, null, thisRound: false, thisTurn: true, base.Card);
        }

        public override void AddTriggers()
        {
            AddDealDamageAtEndOfTurnTrigger(base.TurnTaker, base.Card, (Card c) => !IsVillainTarget(c), TargetType.HighestHP, base.H, DamageType.Melee, isIrreducible: false, optional: false, 2);
            AddTrigger((DealDamageAction dd) => dd.DidDealDamage && dd.DamageSource.Card != null && dd.DamageSource.IsSameCard(base.CharacterCard) && dd.Target.IsHeroCharacterCard && !HasDamageOccurredThisTurn(null, (Card c) => c == base.CharacterCard, dd, (Card c) => c.IsHeroCharacterCard), (DealDamageAction dd) => FollowUpDamageResponse(dd), TriggerType.DealDamage, TriggerTiming.After);
        }

        private IEnumerator FollowUpDamageResponse(DealDamageAction dd)
        {
            IEnumerator coroutine = DealDamage(base.Card, dd.Target, base.Game.H - 1, DamageType.Melee);
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
