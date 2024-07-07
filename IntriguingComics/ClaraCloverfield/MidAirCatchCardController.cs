using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class MidAirCatchCardController : RescueCardController
    {
        public MidAirCatchCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }


        public override void AddTriggers()
        {
			//When Clara Cloverfield would be dealt damage, a player may destroy one of their ongoing cards to prevent that damage.
			AddTrigger((DealDamageAction dd) => dd.Target == Clara && dd.Amount > 0,
                DestroyAndPreventResponse,
                new TriggerType[]
                    {
                        TriggerType.DestroyCard,
                        TriggerType.WouldBeDealtDamage,
                        TriggerType.CancelAction
                    },
                isActionOptional: true,
                timing: TriggerTiming.Before);
        }

		private IEnumerator DestroyAndPreventResponse(DealDamageAction action)
		{

			if (!IsPropertyTrue("HasDestroyedOngoing"))
			{
				List<DestroyCardAction> storedResults = new List<DestroyCardAction>();
				IEnumerator coroutine = base.GameController.SelectHeroToDestroyTheirCard(DecisionMaker, new LinqCardCriteria((Card c) => IsOngoing(c), "ongoing"), optionalSelectHero: true, optionalDestroyCard: true, storedResults, null, null, GetCardSource());
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine);
				}

				if (DidDestroyCard(storedResults))
				{
					SetCardProperty("HasDestroyedOngoing", true);
				}
			}


			if (IsPropertyTrue("HasDestroyedOngoing"))
			{
				IEnumerator coroutine2 = base.CancelAction(action, isPreventEffect: true);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine2);
				}
				else
				{
				base.GameController.ExhaustCoroutine(coroutine2);
				}
				if (!action.IsPretend)
				{
					SetCardProperty("HasDestroyedOngoing", false);
				}

			}
		}

		private Card Clara => base.GameController.FindCardsWhere((Card c) => c.Identifier == "ClaraCloverfield").FirstOrDefault();
	}
}
