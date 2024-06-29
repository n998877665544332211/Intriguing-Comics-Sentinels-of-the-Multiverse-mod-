using System.Collections;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class ThraiwCardController : CardController
    {
        public ThraiwCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowHeroTargetWithHighestHP(1, base.Game.H - 2);
            base.SpecialStringMaker.ShowNumberOfCardsAtLocation(base.TurnTaker.Deck, new LinqCardCriteria((Card c) => c.IsDevice, "device"));
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, (PhaseChangeAction p) => RevealCards_MoveMatching_ReturnNonMatchingCards(base.TurnTakerController, base.TurnTaker.Deck, playMatchingCards: false, putMatchingCardsIntoPlay: true, moveMatchingCardsToHand: false, new LinqCardCriteria((Card c) => c.IsDevice, "device"), 1, null, shuffleSourceAfterwards: true, showMessage: true), TriggerType.PhaseChange);
            AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, DealDamageResponse, TriggerType.DealDamage);
        }

        private IEnumerator DealDamageResponse(PhaseChangeAction a)
        {
            IEnumerator coroutine = DealDamageToHighestHP(base.Card, 1, (Card c) => IsHeroTarget(c), (Card c) => 3, DamageType.Projectile, isIrreducible: false, optional: false, null, () => base.Game.H - 2, (DealDamageAction dd) => ReduceDamageDealtByThatTargetUntilTheStartOfYourNextTurnResponse(dd, 1));
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
