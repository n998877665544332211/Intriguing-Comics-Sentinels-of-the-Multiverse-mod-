using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class ExclusiveInterviewCardController : ScoopCardController
    {
        public ExclusiveInterviewCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            //At the end of the environment turn, reveal the top card of the villain deck.  Replace or discard it.
            base.AddEndOfTurnTrigger((TurnTaker turnTaker) => turnTaker == base.TurnTaker, ReplaceOrDiscardVillainResponse, TriggerType.PhaseChange);
        }



        private IEnumerator ReplaceOrDiscardVillainResponse (PhaseChangeAction action)
        {

			List<SelectLocationDecision> storedResultsDeck = new List<SelectLocationDecision>();
            IEnumerator coroutine6 = base.GameController.SelectADeck(null, SelectionType.RevealTopCardOfDeck, (Location d) => d.IsVillain, storedResultsDeck, optional: false, null, GetCardSource());
						

            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine6);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine6);

            }

            
				List<Card> cards = new List<Card>();
				IEnumerator coroutine = base.GameController.RevealCards(base.TurnTakerController, GetSelectedLocation(storedResultsDeck), 1, cards, fromBottom: false, RevealedCardDisplay.ShowRevealedCards, null, GetCardSource());
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine);
				}
				Card revealedCard = GetRevealedCard(cards);
				if (revealedCard != null)
				{
					IEnumerator coroutine2 = base.GameController.MoveCard(base.TurnTakerController, revealedCard, GetSelectedLocation(storedResultsDeck), toBottom: false, isPutIntoPlay: false, playCardIfMovingToPlayArea: true, null, showMessage: false, null, null, null, evenIfIndestructible: false, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, GetCardSource());
					if (base.UseUnityCoroutines)
					{
						yield return base.GameController.StartCoroutine(coroutine2);
					}
					else
					{
						base.GameController.ExhaustCoroutine(coroutine2);
					}
				}
				IEnumerable<Card> associatedCards = null;
				if (revealedCard != null)
				{
					associatedCards = new Card[1] { revealedCard };
				}

				


			List<bool> storedResults = new List<bool>();
			Func<HeroTurnTakerController, bool> heroCriteria = (HeroTurnTakerController hero) => !hero.IsIncapacitatedOrOutOfGame;
			IEnumerator coroutine7 = MakeUnanimousDecision(heroCriteria, SelectionType.Custom, 1, upTo: false, storedResults,associatedCards);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine7);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine7);
			}
			if (storedResults.Count() > 0 && storedResults.First())
			{



				
					IEnumerator coroutine4 = base.GameController.DiscardTopCard(GetSelectedLocation(storedResultsDeck), null, null, null, GetCardSource());
					if (base.UseUnityCoroutines)
					{
						yield return base.GameController.StartCoroutine(coroutine4);
					}
					else
					{
						base.GameController.ExhaustCoroutine(coroutine4);
					}
			}
				List<Location> list = new List<Location>();
				list.Add(base.TurnTaker.Revealed);
				IEnumerator coroutine5 = CleanupCardsAtLocations(list, GetSelectedLocation(storedResultsDeck), toBottom: false, addInhibitorException: true, shuffleAfterwards: false, sendMessage: false, isDiscard: false, isReturnedToOriginalLocation: true, cards);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine5);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine5);
				}





     		
        }

		public override CustomDecisionText GetCustomDecisionText(IDecision decision)
		{
			if (decision is YesNoDecision)
			{
				return new CustomDecisionText("Discard the top card of the villain deck?", "The players are choosing whether to discard the top card of the villain deck.", "Vote to discard the top card of the villain deck", "Discard the top card of the villain deck");
			}
			return null;
		}

	}
}