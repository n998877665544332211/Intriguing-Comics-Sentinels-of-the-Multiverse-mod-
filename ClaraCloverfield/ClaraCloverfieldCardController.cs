using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class ClaraCloverfieldCardController : CardController
    {
        public ClaraCloverfieldCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.MakesIndestructible);
        }


        //This card counts as a hero card.
        //Taken care of in DeckList

        //If this card has 1 or more HP, it is indestructible.
        public override bool AskIfCardIsIndestructible(Card card)
        {
            if (base.Card.HitPoints.Value >= 1)
            {
            return card == base.Card;
            }
            return false;
     	}

        public override void AddTriggers()
        {

            //If this card is destroyed, the heroes lose.

            AddTrigger((MoveCardAction moveCard) => moveCard.CardToMove == base.Card && !moveCard.Destination.IsInPlay && moveCard.CardSource.Card.Identifier != "TheScionReserve", (MoveCardAction m) => HeroesLoseResponse(m), TriggerType.GameOver, TriggerTiming.Before);
            AddBeforeDestroyAction(HeroesLoseResponse);
        }



        //If this card is destroyed, the heroes lose.
        private IEnumerator HeroesLoseResponse(GameAction action)
        {

            if (!HasBeenSetToTrueThisTurn("GameOverThisTurn", (CardPropertiesJournalEntry cp) => cp.Card.Identifier == base.Card.Identifier))
            {
                SetCardPropertyToTrueIfRealAction("GameOverThisTurn");
                IEnumerator coroutine = base.GameController.GameOver(EndingResult.EnvironmentDefeat, "Clara Cloverfield has filed her last story.", showEndingTextAsMessage: true, null, null, GetCardSource());
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