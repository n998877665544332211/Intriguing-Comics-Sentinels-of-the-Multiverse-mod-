using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using Handelabra;
using System;
using System.Globalization;
using System.Collections;
using System.Linq;

namespace IntriguingComics.GrandWarlordVoss
{
    public class InvaderVossTurnTakerController:TurnTakerController
    {

        
        public InvaderVossTurnTakerController(TurnTaker turnTaker, GameController gameController)
            : base(turnTaker, gameController)
        {
        }

          
        public override IEnumerator StartGame()
        {
            IEnumerator coroutine = PutInTrash((Card c) => c.IsMinion);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }


            Card cardByIdentifier = base.TurnTaker.GetCardByIdentifier("TcfStalwart");
            IEnumerator coroutine2 = base.GameController.PlayCard(this, cardByIdentifier, isPutIntoPlay: true, null, optional: false, null, null, evenIfAlreadyInPlay: false, null, null, null, associateCardSource: false, fromBottom: false, canBeCancelled: true, new CardSource(base.CharacterCardController));
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine2);
            }

            IEnumerator coroutine3 = PutToSide((Card c) => c.Identifier == "ForcedDeployment");
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine3);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine3);
            }



            IEnumerator coroutine4 = base.GameController.ShuffleLocation(base.TurnTaker.Deck, null, new CardSource(base.CharacterCardController));
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine4);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine4);
            }

            IEnumerator coroutine5 = MoveToBottomOfDeck((Card c) => c.Identifier == "ForcedDeployment");
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine5);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine5);
            }
        }

        protected IEnumerator PutIntoPlay(string identifier)
        {
            Card cardByIdentifier = base.TurnTaker.GetCardByIdentifier(identifier);
            IEnumerator coroutine = base.GameController.PlayCard(this, cardByIdentifier, isPutIntoPlay: true, null, optional: false, null, null, evenIfAlreadyInPlay: false, null, null, null, associateCardSource: false, fromBottom: false, canBeCancelled: true, new CardSource(base.CharacterCardController));
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
        }

        protected IEnumerator PutInTrash(Func<Card, bool> cardCriteria)
        {
            IEnumerator coroutine = base.GameController.BulkMoveCards(this, base.TurnTaker.GetAllCards().Where(cardCriteria), base.TurnTaker.Trash, toBottom: false, performBeforeDestroyActions: true, null, isDiscard: false, new CardSource(base.CharacterCardController));
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
        }
        protected IEnumerator PutToSide(Func<Card, bool> cardCriteria)
        {
            IEnumerator coroutine = base.GameController.BulkMoveCards(this, base.TurnTaker.GetAllCards().Where(cardCriteria), base.TurnTaker.OffToTheSide, toBottom: false, performBeforeDestroyActions: true, null, isDiscard: false, new CardSource(base.CharacterCardController));
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
        }

        protected IEnumerator MoveToBottomOfDeck(Func<Card, bool> cardCriteria)
        {
            IEnumerator coroutine = base.GameController.BulkMoveCards(this, base.TurnTaker.GetAllCards().Where(cardCriteria), base.TurnTaker.Deck, toBottom: true, performBeforeDestroyActions: true, null, isDiscard: false, new CardSource(base.CharacterCardController));
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
