using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class LifeEnergyTransfusionCardController : CardController
    {
        public LifeEnergyTransfusionCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

		private Card Clara => base.GameController.FindCardsWhere((Card c) => c.Identifier == "ClaraCloverfield").FirstOrDefault();
		public override IEnumerator Play()
        {
			//        When this card enters play, destroy all Peril cards in play.
			IEnumerator coroutine = base.GameController.DestroyCards(DecisionMaker, new LinqCardCriteria((Card c) => c.DoKeywordsContain("peril")), autoDecide: false, null, null, null, SelectionType.DestroyCard, GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}


			//Then, each hero may do themselves 1 Energy damage.  Then, Clara regains X HP, where X equals the total amount of damage dealt this way.
			List<DealDamageAction> storedResults = new List<DealDamageAction>();
		IEnumerator e2 = base.GameController.DealDamageToSelf(DecisionMaker, (Card c) => c.IsHeroCharacterCard, 1, DamageType.Energy, isIrreducible: false, storedResults, isOptional: false, null, requiredDecisions: 0, GetCardSource());
		if (base.UseUnityCoroutines)
		{
			yield return base.GameController.StartCoroutine(e2);
		}
		else
		{
			base.GameController.ExhaustCoroutine(e2);
		}
			
			
			int amount = (from dd in storedResults
						  where dd.DidDealDamage
						  select dd.Amount).Sum();


			if (amount > 0)
			{
				IEnumerator coroutine2 = base.GameController.GainHP(Clara, amount, null, null, GetCardSource());
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine2);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine2);
				}
			}



		}


        //At the end of the environment turn, destroy this card.
        public override void AddTriggers()
        {
            AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, base.DestroyThisCardResponse, TriggerType.DestroySelf);
        }
    }
}