using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class TheRegionalManagerCardController : CardController
    {
        public TheRegionalManagerCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowNumberOfCardsAtLocation(base.TurnTaker.Deck, new LinqCardCriteria((Card c) => IsOrder(c), "order"));
            base.SpecialStringMaker.ShowNumberOfCardsAtLocation(base.TurnTaker.Trash, new LinqCardCriteria((Card c) => c.Identifier == "TheRestOfTheGang", "the rest of the gang"));
        }

        public override void AddTriggers()
        {
            AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, (PhaseChangeAction p) => RevealCards_MoveMatching_ReturnNonMatchingCards(base.TurnTakerController, base.TurnTaker.Deck, playMatchingCards: false, putMatchingCardsIntoPlay: true, moveMatchingCardsToHand: false, new LinqCardCriteria((Card c) => IsOrder(c), "order"), 1, null, shuffleSourceAfterwards: true, showMessage: true), TriggerType.PhaseChange);
            AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, (PhaseChangeAction p) => PlayCardFromLocation(base.TurnTaker.Trash, "TheRestOfTheGang"), TriggerType.PlayCard);
        }

        private bool IsOrder(Card c)
        {
            return c.DoKeywordsContain("order");
        }
    }
}
