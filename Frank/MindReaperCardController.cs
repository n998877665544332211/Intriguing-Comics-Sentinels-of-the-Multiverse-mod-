using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class MindReaperCardController : CardController
    {
        public MindReaperCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, DealDamageResponse, TriggerType.DealDamage);
        }

        private IEnumerator DealDamageResponse(PhaseChangeAction phaseChange)
        {
            List<DealDamageAction> storedResults = new List<DealDamageAction>();
            IEnumerator coroutine = DealDamage(base.Card, (Card c) => c != base.Card && !c.IsDevice, 1, DamageType.Psychic, isIrreducible: false, optional: false, storedResults);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            IEnumerable<Card> damagedTargets = (from d in storedResults
                                                where d.DidDealDamage
                                                select d.Target).Distinct();
            if (storedResults != null)
            {
                IEnumerator coroutine2 = MultipleDamageSourcesDealDamage(new LinqCardCriteria((Card c) => damagedTargets.Contains(c) && c.IsInPlayAndHasGameText && c.IsTarget), TargetType.HighestHP, 1, new LinqCardCriteria((Card c) => IsHeroTarget(c)), 1, DamageType.Melee);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine2);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine2);
                }
            }
        }
    }
}
