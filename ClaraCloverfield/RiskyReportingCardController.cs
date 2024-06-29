using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class RiskyReportingCardController : PerilCardController
    {

		public override bool AllowFastCoroutinesDuringPretend
		{
			get
			{
				if (IsHighestHitPointsUnique((Card card) => IsVillainTarget(card)))
				{
					return IsHighestHitPointsUnique((Card card) => IsHeroTarget(card));
				}
				return false;
			}
		}

		public RiskyReportingCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
			base.SpecialStringMaker.ShowVillainTargetWithHighestHP(numberOfTargets: 1);
			base.SpecialStringMaker.ShowHeroTargetWithHighestHP(numberOfTargets: 1);
		}

		public override void AddTriggers()
        {
            //  Whenever the villain target with the highest hp would deal damage to the hero target with the highest hp, redirect that damage to Clara Cloverfield.
            List<TriggerType> list = new List<TriggerType>();
            list.Add(TriggerType.RedirectDamage);
            Func<DealDamageAction, bool> criteria = (DealDamageAction dealDamage) => (dealDamage.DamageSource.Card != null && IsVillainTarget(dealDamage.DamageSource.Card) && CanCardBeConsideredHighestHitPoints(dealDamage.DamageSource.Card, (Card c) => IsVillainTarget(c) && base.GameController.IsCardVisibleToCardSource(c, GetCardSource())) && base.GameController.IsCardVisibleToCardSource(dealDamage.DamageSource.Card, GetCardSource())) & (IsHeroTarget(dealDamage.Target) && base.GameController.IsCardVisibleToCardSource(dealDamage.Target, GetCardSource())) & CanCardBeConsideredHighestHitPoints(dealDamage.Target, (Card c) => IsHeroTarget(c) && base.GameController.IsCardVisibleToCardSource(c, GetCardSource()));
            AddTrigger(criteria, RedirectToClara, list, TriggerTiming.Before);
        }

        private Card Clara => base.GameController.FindCardsWhere((Card c) => c.Identifier == "ClaraCloverfield").FirstOrDefault();

		private IEnumerator RedirectToClara(DealDamageAction dealDamage)
		{
					
			List<bool> shouldRedirect = new List<bool>();
			IEnumerator coroutine = DetermineIfGivenCardIsTargetWithLowestOrHighestHitPoints(dealDamage.DamageSource.Card, highest: true, (Card card) => IsVillainTarget(card) && base.GameController.IsCardVisibleToCardSource(card, GetCardSource()), dealDamage, shouldRedirect);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
			if (!shouldRedirect.First())
			{
				yield break;
			}
			List<bool> shouldRedirect2 = new List<bool>();
			IEnumerator coroutine2 = DetermineIfGivenCardIsTargetWithLowestOrHighestHitPoints(dealDamage.Target, highest: true, (Card card) => IsHeroTarget(card) && base.GameController.IsCardVisibleToCardSource(card, GetCardSource()), dealDamage, shouldRedirect2);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine2);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine2);
			}
			if (!shouldRedirect2.First())
			{
				yield break;
			}

				IEnumerator coroutine3 = base.GameController.RedirectDamage(dealDamage, Clara, isOptional: false, GetCardSource());
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine3);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine3);
				}
			
		}
    }
}
