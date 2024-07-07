using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class BelovedChildhoodMementoCardController : CardController
    {
        public BelovedChildhoodMementoCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowHeroCharacterCardWithLowestHP(numberOfTargets: 1);
        }

        public override void AddTriggers()
        {
            //At the end of the Villain turn, the Hero with the lowest HP deals themself H+1 psychic damage.
            base.AddEndOfTurnTrigger((TurnTaker turnTaker) => turnTaker == base.TurnTaker, SelfDamageResponse, TriggerType.DealDamage);
        }


        private IEnumerator SelfDamageResponse(PhaseChangeAction action)
        {

        	List<Card> storedHero = new List<Card>();
			IEnumerator coroutine = base.GameController.FindTargetsWithLowestHitPoints(1, 1, (Card card) => card.IsHeroCharacterCard && card.IsTarget, storedHero, cardSource: base.GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
            {
                GameController.ExhaustCoroutine(coroutine);
            }
            if (storedHero.FirstOrDefault() != null)
            {
                var hero = storedHero.FirstOrDefault();
                coroutine = DealDamage(hero, hero, base.Game.H, DamageType.Psychic, cardSource: GetCardSource());
                if (UseUnityCoroutines)
                {
                    yield return GameController.StartCoroutine(coroutine);
                }
                else
                {
                    GameController.ExhaustCoroutine(coroutine);
                }
            }
            yield break;
        }

    }
}
