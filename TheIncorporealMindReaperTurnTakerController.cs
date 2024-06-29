using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using Handelabra;
using System;
using System.Globalization;
using System.Collections;
using System.Linq;

namespace IntriguingComics.MindReaper
{
    public class TheIncorporealMindReaperTurnTakerController:TurnTakerController
    {

        
        public TheIncorporealMindReaperTurnTakerController(TurnTaker turnTaker, GameController gameController)
            : base(turnTaker, gameController)
        {
        }

          
        public override IEnumerator StartGame()
        {
            
            Card cardByIdentifier = base.TurnTaker.GetCardByIdentifier("Possession");
            IEnumerator coroutine2 = base.GameController.PlayCard(this, cardByIdentifier, isPutIntoPlay: true, null, optional: false, null, null, evenIfAlreadyInPlay: false, null, null, null, associateCardSource: false, fromBottom: false, canBeCancelled: true, new CardSource(base.CharacterCardController));
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine2);
            }


            if (base.GameController.Game.IsAdvanced)
            {
                IEnumerator coroutine = PutCardsIntoPlay(new LinqCardCriteria((Card c) => c.IsProjection, "projection"), base.H - 2);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
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
        }

    }
}