using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class PricelessAntiquesCardController : CardController
    {
        public PricelessAntiquesCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowHeroTargetWithHighestHP(numberOfTargets: 1);
        }

        public override void AddTriggers()
        {
            //If this card would be dealt 3 or more damage, redirect that damage to the Hero target with the highest HP.
            base.AddTrigger<DealDamageAction>((DealDamageAction action) => action.Target == base.Card && action.Amount >= 3, this.RedirectDamageResponse, TriggerType.RedirectDamage, TriggerTiming.Before);
        }
        private IEnumerator RedirectDamageResponse(DealDamageAction action)
        {
            IEnumerator coroutine = base.RedirectDamage(action, TargetType.HighestHP, (Card c) => IsHeroTarget(c));
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
    }
}
