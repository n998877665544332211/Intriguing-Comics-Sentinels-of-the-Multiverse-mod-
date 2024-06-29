using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class GetBehindMeCardController : RescueCardController
    {
        public GetBehindMeCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
			base.SpecialStringMaker.ShowHeroCharacterCardWithLowestHP(numberOfTargets: 1);

		}


		public override void AddTriggers()
        {
			//Whenever Clara Cloverfield would be dealt damage, it may be redirected to the Hero with the lowest HP.
			AddTrigger((DealDamageAction dd) => dd.Target == Clara, MaybeRedirectResponse, TriggerType.RedirectDamage, TriggerTiming.Before);
		}

		private IEnumerator MaybeRedirectResponse(DealDamageAction dd)
		{
			List<Card> storedResultsGetBehindMe = new List<Card>();
			IEnumerator coroutine = base.GameController.FindTargetWithLowestHitPoints(1, (Card c) => c.IsHeroCharacterCard, storedResultsGetBehindMe, null, dd.ToEnumerable().ToList(), evenIfCannotDealDamage: false, optional: false, null, ignoreBattleZone: false, GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
			if (storedResultsGetBehindMe.Count() > 0)
			{
				Card newTarget = storedResultsGetBehindMe.First();
				coroutine = base.GameController.RedirectDamage(dd, newTarget, isOptional: true, GetCardSource());
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
		private Card Clara => base.GameController.FindCardsWhere((Card c) => c.Identifier == "ClaraCloverfield").FirstOrDefault();
	}
}
