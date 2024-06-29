using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class CombatZoneCorrespondentCardController : PerilCardController
    {
        public CombatZoneCorrespondentCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            //  Whenever a hero target or villain target deals 4 or more damage to a target other than Clara Cloverfield, they also deal Clara Cloverfield 1 HP of the same type of damage.
            AddTrigger((DealDamageAction dd) => dd.DamageSource.Card != null && (IsHeroTarget(dd.DamageSource.Card) || IsVillainTarget(dd.DamageSource.Card)) && dd.Amount >= 4 && dd.DidDealDamage && !base.GameController.IsCardIndestructible(dd.Target), DealClaraDamageResponse, TriggerType.DealDamage, TriggerTiming.After);
        }
        private Card Clara => base.GameController.FindCardsWhere((Card c) => c.Identifier == "ClaraCloverfield").FirstOrDefault();


        //... they also deal Clara Cloverfield 1 HP of the same type of damage.

        private IEnumerator DealClaraDamageResponse(DealDamageAction damage)
		{
			IEnumerator coroutine = DealDamage(damage.DamageSource.Card, Clara, 1, damage.DamageType, isIrreducible: false);
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
