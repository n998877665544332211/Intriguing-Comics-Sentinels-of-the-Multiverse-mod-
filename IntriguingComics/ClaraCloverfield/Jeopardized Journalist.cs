using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class JeopardizedJournalistCardController : PerilCardController
    {
        public JeopardizedJournalistCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            //At the start of the environment turn, destroy Clara Cloverfield, even if another card says she's indestructible.
            base.AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, HeroesLoseResponse, TriggerType.GameOver);

            //A player may skip their turn to destroy this card.
            AddStartOfTurnTrigger((TurnTaker tt) => tt.IsHero, base.SkipTheirTurnToDestroyThisCardResponse, new TriggerType[2]
            {
            TriggerType.SkipTurn,
            TriggerType.DestroySelf
            });
        }
    

        //  Instead of destroying Clara, this skips to the consequence: the heroes lose.
        private IEnumerator HeroesLoseResponse(GameAction action)
        {
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
