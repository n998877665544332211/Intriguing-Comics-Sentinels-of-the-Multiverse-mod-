using System.Collections;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class TheBigJobCardController : CardController
    {
        public TheBigJobCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            IEnumerator coroutine = RevealCards_MoveMatching_ReturnNonMatchingCards(base.TurnTakerController, base.TurnTaker.Deck, playMatchingCards: false, putMatchingCardsIntoPlay: true, moveMatchingCardsToHand: false, new LinqCardCriteria((Card c) => IsOrder(c) || Ishench(c) || c.IsDevice, "order, hench, or device"), null, base.H, shuffleSourceAfterwards: true, showMessage: false, RevealedCardDisplay.ShowRevealedCards, shuffleReturnedCards: true);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            IEnumerator coroutine2 = PlayTheTopCardOfTheVillainDeckResponse(null);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine2);
            }
        }

        private bool IsOrder(Card c)
        {
            return c.DoKeywordsContain("order");
        }

        private bool Ishench(Card c)
        {
            return c.DoKeywordsContain("hench");
        }
    }
}