using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class HiddenVaultCardController : CardController
    {
        public HiddenVaultCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            //This card and any card it is next to are indestructible.
            base.AddThisCardControllerToList(CardControllerListType.MakesIndestructible);
        }

        public override bool AskIfCardIsIndestructible(Card card)
        {
            //This card and any card it is next to are indestructible.
            return card == base.Card || base.IsThisCardNextToCard(card);
        }

        public override IEnumerator Play()
        {
         
            //Display message.
            IEnumerator coroutine4 = base.GameController.SendMessageAction("Lootcase steals a case card from the villain deck to put into his hidden vault.", Priority.Low, GetCardSource(), null, showCardSource: true);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine4);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine4);
            }

            //When this card comes into play, reveal cards from the top of the Villain deck until a Case card is revealed.  Put the Case into play.  Shuffle the other revealed cards into the Villain deck.
            List<Card> storedResults = new List<Card>();
            IEnumerator coroutine = base.RevealCards_MoveMatching_ReturnNonMatchingCards(base.TurnTakerController, base.TurnTaker.Deck, false, true, false, new LinqCardCriteria((Card c) => c.DoKeywordsContain("case"), "case"), 1, storedPlayResults: storedResults);

            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }

            Card CardInVault = storedResults.FirstOrDefault();


            if (CardInVault != null && CardInVault.IsInPlayAndHasGameText)
            {
                //move this card next to it
                IEnumerator coroutine2 = GameController.MoveCard(base.TurnTakerController, base.Card, CardInVault.NextToLocation,
                playCardIfMovingToPlayArea: false,
                cardSource: GetCardSource());
                if (this.UseUnityCoroutines)
                {
                    yield return this.GameController.StartCoroutine(coroutine2);
                }
                else
                {
                    this.GameController.ExhaustCoroutine(coroutine2);
                }
                yield break;
            }
            else
            {
            }


        }

    }
}
