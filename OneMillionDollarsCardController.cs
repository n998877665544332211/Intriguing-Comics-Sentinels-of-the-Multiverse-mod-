using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class OneMillionDollarsCardController : CardController
    {
        public OneMillionDollarsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            //At the end of the Villain turn, all Case cards regain H-1 HP.
            base.AddEndOfTurnTrigger((TurnTaker turnTaker) => turnTaker == base.TurnTaker, GainHPResponse, TriggerType.GainHP);
        }
        private IEnumerator GainHPResponse(PhaseChangeAction action)
        {
            IEnumerator coroutine = base.GameController.GainHP(this.DecisionMaker, (Card c) => IsCase(c), H - 1, cardSource: base.GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            yield break;
        }
        private bool IsCase(Card c)
        {
            return c.DoKeywordsContain("case");
        }
    }
}