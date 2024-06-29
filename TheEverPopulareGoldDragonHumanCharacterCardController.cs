using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.GoldDragon
{
    public class TheEverPopularGoldDragonHumanCharacterCardController : HeroCharacterCardController
    {
        public TheEverPopularGoldDragonHumanCharacterCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
        }


		public virtual IEnumerator Switch(DealDamageAction dd, HeroTurnTaker hero, StatusEffect effect, int[] powerNumerals = null)
		{
			Card otherCard = base.TurnTaker.FindCard("GoldDragonDragonCharacter");
			if (otherCard != null)
			{
				IEnumerator switchToDragon = base.GameController.SwitchCards(base.Card, otherCard, playCardIfMovingToPlayArea: false, ignoreFlipped: false, ignoreHitPoints: false, GetCardSource());
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(switchToDragon);
				}
				else
				{
					base.GameController.ExhaustCoroutine(switchToDragon);
				}

				if (otherCard.IsFlipped)
				{
					IEnumerator stopbeingtarget = base.GameController.RemoveTargets((Card c) => c == otherCard, ignoreBattleZone: true, GetCardSource());
					if (base.UseUnityCoroutines)
					{
						yield return base.GameController.StartCoroutine(stopbeingtarget);
					}
					else
					{
						base.GameController.ExhaustCoroutine(stopbeingtarget);
					}
				}
			}
		}





		public override IEnumerator UsePower(int index = 0)
        {
			if (base.TurnTakerController.CharacterCard.SharedIdentifier != "TheEverPopularGoldDragonCharacter")
			{
				IEnumerator showMessage = base.GameController.SendMessageAction("\"You can use the energy breath,\" Gold Dragon warns, \"but I don't think the other part will work.\"", Priority.Critical, GetCardSource(), null, showCardSource: true);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(showMessage);
				}
				else
				{
					base.GameController.ExhaustCoroutine(showMessage);
				}

				{
					int powerNumeral2 = GetPowerNumeral(1, 1);
					int powerNumeral3 = GetPowerNumeral(2, 1);
					GameController gameController = base.GameController;
					HeroTurnTakerController decisionMaker = DecisionMaker;
					DamageSource source = new DamageSource(base.GameController, base.Card);
					int? numberOfTargets = powerNumeral2;
					int? requiredTargets = powerNumeral2;
					CardSource cardSource = GetCardSource();
					IEnumerator coroutine = gameController.SelectTargetsAndDealDamage(decisionMaker, source, powerNumeral3, DamageType.Energy, numberOfTargets, optional: false, requiredTargets, isIrreducible: false, allowAutoDecide: false, autoDecide: false, null, null, null, null, null, selectTargetsEvenIfCannotDealDamage: false, null, null, ignoreBattleZone: false, null, cardSource);
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



		
			else
			{


				int powerNumeral = GetPowerNumeral(0, 2);
				OnDealDamageStatusEffect damageStatusEffect = new OnDealDamageStatusEffect(base.CardWithoutReplacements, "Switch", "The next time Gold Dragon is dealt either energy, cold, fire, or lightning damage, switch his character card.", new TriggerType[1] { TriggerType.DealDamage }, null, base.Card);
				damageStatusEffect.TargetCriteria.IsSpecificCard = base.CharacterCard;
				damageStatusEffect.BeforeOrAfter = BeforeOrAfter.After;
				damageStatusEffect.NumberOfUses = 1;
				damageStatusEffect.DamageTypeCriteria.AddType(DamageType.Energy);
				damageStatusEffect.DamageTypeCriteria.AddType(DamageType.Cold);
				damageStatusEffect.DamageTypeCriteria.AddType(DamageType.Fire);
				damageStatusEffect.DamageTypeCriteria.AddType(DamageType.Lightning);
				damageStatusEffect.DamageAmountCriteria.GreaterThan = powerNumeral-1;
				IEnumerator addDamageStatusEffect = AddStatusEffect(damageStatusEffect);
				if (base.UseUnityCoroutines)
				{
				yield return base.GameController.StartCoroutine(addDamageStatusEffect);
				}
				else
				{
				base.GameController.ExhaustCoroutine(addDamageStatusEffect);
				}



			{
				int powerNumeral2 = GetPowerNumeral(1, 1);
				int powerNumeral3 = GetPowerNumeral(2, 1);
				GameController gameController = base.GameController;
				HeroTurnTakerController decisionMaker = DecisionMaker;
				DamageSource source = new DamageSource(base.GameController, base.Card);
				int? numberOfTargets = powerNumeral2;
				int? requiredTargets = powerNumeral2;
				CardSource cardSource = GetCardSource();
				IEnumerator coroutine = gameController.SelectTargetsAndDealDamage(decisionMaker, source, powerNumeral3, DamageType.Energy, numberOfTargets, optional: false, requiredTargets, isIrreducible: false, allowAutoDecide: false, autoDecide: false, null, null, null, null, null, selectTargetsEvenIfCannotDealDamage: false, null, null, ignoreBattleZone: false, null, cardSource);
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


		public override IEnumerator UseIncapacitatedAbility(int index)
		{
			switch (index)
			{
				case 0:
					{
						IEnumerator drawCard = base.GameController.SelectHeroToDrawCard(base.HeroTurnTakerController, optionalSelectHero: false, optionalDrawCard: true, allowAutoDecideHero: false, null, null, null, GetCardSource());
						if (base.UseUnityCoroutines)
						{
							yield return base.GameController.StartCoroutine(drawCard);
						}
						else
						{
							base.GameController.ExhaustCoroutine(drawCard);
						}
						break;
					}
				case 1:
					{
						IEnumerator coroutine3 = base.GameController.SelectHeroAndReduceNextDamageTaken(base.HeroTurnTakerController, 2, 1, null, GetCardSource());
						if (base.UseUnityCoroutines)
						{
							yield return base.GameController.StartCoroutine(coroutine3);
						}
						else
						{
							base.GameController.ExhaustCoroutine(coroutine3);
						}
						break;
					}
				case 2:
					{
						List<SelectTurnTakerDecision> storedResults = new List<SelectTurnTakerDecision>();
						GameController gameController = base.GameController;
						HeroTurnTakerController heroTurnTakerController = base.HeroTurnTakerController;
						CardSource cardSource = GetCardSource();
						IEnumerator coroutine = gameController.SelectHeroTurnTaker(heroTurnTakerController, SelectionType.DealDamageAfterUsePower, optional: false, allowAutoDecide: false, storedResults, null, null, allowIncapacitatedHeroes: false, null, null, canBeCancelled: true, null, cardSource);
						if (base.UseUnityCoroutines)
						{
							yield return base.GameController.StartCoroutine(coroutine);
						}
						else
						{
							base.GameController.ExhaustCoroutine(coroutine);
						}
						if (!storedResults.Any((SelectTurnTakerDecision d) => d.Completed && d.SelectedTurnTaker != null && d.SelectedTurnTaker.IsHero))
						{
							break;
						}
						TurnTaker selectedTurnTaker = storedResults.FirstOrDefault().SelectedTurnTaker;
						if (selectedTurnTaker.IsHero)
						{
							HeroTurnTaker heroTurnTaker = selectedTurnTaker.ToHero();
							Card damageSource = ((!heroTurnTaker.HasMultipleCharacterCards) ? heroTurnTaker.CharacterCard : null);
							DealDamageAfterUsePowerStatusEffect dealDamageAfterUsePowerStatusEffect = new DealDamageAfterUsePowerStatusEffect(heroTurnTaker, damageSource, null, 1, DamageType.Energy, 1, isIrreducible: false);
							dealDamageAfterUsePowerStatusEffect.TurnTakerCriteria.IsSpecificTurnTaker = heroTurnTaker;
							if (!heroTurnTaker.HasMultipleCharacterCards)
							{
								dealDamageAfterUsePowerStatusEffect.CardDestroyedExpiryCriteria.Card = heroTurnTaker.CharacterCard;
							}
							dealDamageAfterUsePowerStatusEffect.NumberOfUses = 1;
							coroutine = AddStatusEffect(dealDamageAfterUsePowerStatusEffect);
							if (base.UseUnityCoroutines)
							{
								yield return base.GameController.StartCoroutine(coroutine);
							}
							else
							{
								base.GameController.ExhaustCoroutine(coroutine);
							}
						}
						break;

					}
			}
		}

	}
}