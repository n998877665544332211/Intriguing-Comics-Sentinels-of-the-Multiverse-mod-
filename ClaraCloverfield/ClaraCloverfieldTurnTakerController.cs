using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Linq;

namespace IntriguingComics.ClaraCloverfield
{
    public class ClaraCloverfieldTurnTakerController:TurnTakerController
    {
     
        public ClaraCloverfieldTurnTakerController(TurnTaker turnTaker, GameController gameController)
            : base(turnTaker, gameController)
        {
        }


		//"At the start of the game, put this card into play.  Shuffle the environment deck."
		public override IEnumerator StartGame()
        {
			Card Clara = base.TurnTaker.GetCardByIdentifier("ClaraCloverfield");

			CardController claraCardController = base.GameController.FindCardController(Clara);
		
				IEnumerator coroutine = base.GameController.PlayCard(this, Clara, isPutIntoPlay: true, null, optional: false, null, null, evenIfAlreadyInPlay: false, null, null, null, associateCardSource: false, fromBottom: false, canBeCancelled: true, cardSource: claraCardController.GetCardSource());
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
