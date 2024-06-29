using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.GoldDragon
{
    public class TheEverPopularGoldDragonDragonCharacterCardController : HeroCharacterCardController
    {
        public TheEverPopularGoldDragonDragonCharacterCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.ActivatesEffects);
        }

        public override bool? AskIfActivatesEffect(TurnTakerController turnTakerController, string effectKey)
        {
            bool? result = null;
            if (turnTakerController == TurnTakerController && effectKey == "{dragon}")
            {
                result = true;
            }
            return result;
        }

        public override IEnumerator UsePower(int index = 0)
		{
			int powerNumeral = GetPowerNumeral(0, 0);
			IEnumerator coroutine = DealDamage(base.Card, (Card c) => c.IsTarget && c.IsInPlayAndHasGameText, powerNumeral, DamageType.Energy);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
		}

		public override void AddTriggers()
		{
			AddTrigger((DealDamageAction dd) => dd.Target == base.Card && dd.Amount >= 2 && !base.Card.IsFlipped, ChangeToHumanResponse, new TriggerType[1] { TriggerType.DealDamage }, TriggerTiming.After);

		}


		private IEnumerator ChangeToHumanResponse(GameAction ga)
		{
				IEnumerator sendMessage2 = base.GameController.SendMessageAction("Gold Dragon's injury causes him to revert to human form.", Priority.Medium, GetCardSource(), null, showCardSource: true);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(sendMessage2);
				}
				else
				{
					base.GameController.ExhaustCoroutine(sendMessage2);
				}
			Card otherCard = base.TurnTaker.FindCard("GoldDragonHumanCharacter");
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
			}


		}



		public override IEnumerator UseIncapacitatedAbility(int index)
		{
			switch (index)
			{
				case 0:
					{
						IEnumerator coroutine3 = base.GameController.SelectHeroToUsePower(base.HeroTurnTakerController, optionalSelectHero: false, optionalUsePower: true, allowAutoDecide: false, null, null, null, omitHeroesWithNoUsablePowers: true, canBeCancelled: true, GetCardSource());
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
				case 1:
					{
						IEnumerator coroutine = base.GameController.SelectAndDestroyCard(base.HeroTurnTakerController, new LinqCardCriteria((Card c) => IsOngoing(c), "ongoing"), optional: false, null, null, GetCardSource());
						if (base.UseUnityCoroutines)
						{
							yield return base.GameController.StartCoroutine(coroutine);
						}
						else
						{
							base.GameController.ExhaustCoroutine(coroutine);
						}
						break;
					}
				case 2:
					{
							ChangeDamageTypeStatusEffect changeDamageEffect = new ChangeDamageTypeStatusEffect(DamageType.Lightning);
							changeDamageEffect.SourceCriteria.IsHero = true;
							changeDamageEffect.SourceCriteria.IsTarget = true;
                            changeDamageEffect.UntilStartOfNextTurn(base.TurnTaker);
							IEnumerator changeType = AddStatusEffect(changeDamageEffect);
							if (base.UseUnityCoroutines)
							{
								yield return base.GameController.StartCoroutine(changeType);
							}
							else
							{
								base.GameController.ExhaustCoroutine(changeType);
							}
						}
						break;
					}
			}
		}

	}