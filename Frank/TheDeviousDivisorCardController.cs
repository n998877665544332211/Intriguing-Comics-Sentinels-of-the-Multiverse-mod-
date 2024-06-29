using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class TheDeviousDivisorCardController : CardController
    {
        public TheDeviousDivisorCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger((DealDamageAction dealDamage) => IsVillainTarget(dealDamage.Target) && dealDamage.IsIrreducible, base.ImmuneToDamageResponse, TriggerType.MakeDamageUnincreasable, TriggerTiming.Before);
            AddTrigger((UsePowerAction p) => true, DealDamageResponse, TriggerType.DealDamage, TriggerTiming.After);
        }

        private IEnumerator DealDamageResponse(UsePowerAction usePower)
        {
            CardController cc = ((usePower.Power.CopiedFromCardController != null) ? usePower.Power.CopiedFromCardController : ((usePower.Power.CardSource.CardController == null || usePower.Power.CardSource.CardController == usePower.Power.CardController) ? usePower.Power.CardController : usePower.Power.CardSource.CardController));
            if (cc.GetPowerNumerals(usePower.Power, usePower.Power.Index).Count() == 0)
            {
                IEnumerator showMessage = base.GameController.SendMessageAction("The power used does not contain numerals, so the Devious Divisor does not deal damage.", Priority.Medium, GetCardSource(), null, showCardSource: true);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(showMessage);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(showMessage);
                }
                yield break;
            }
            int damageToDeal = cc.GetPowerNumerals(usePower.Power, usePower.Power.Index).Sum();
            Card card = null;
            Power power = usePower.Power;
            TurnTakerController ttc = power.TurnTakerController;
            if (ttc != null && ttc.HasMultipleCharacterCards && power.CardController.Card.IsHeroCharacterCard && ttc.TurnTaker == power.CardController.Card.Owner)
            {
                card = power.CardController.Card;
            }
            if (card == null)
            {
                if (ttc == null)
                {
                    if (power.CardController.Card.IsCharacter)
                    {
                        card = power.CardController.Card;
                    }
                }
                else if (usePower.HeroCharacterCardUsingPower != null)
                {
                    card = usePower.HeroCharacterCardUsingPower.Card;
                }
                else
                {
                    List<Card> storedResults = new List<Card>();
                    IEnumerator coroutine = FindCharacterCardToTakeDamage(ttc.TurnTaker, storedResults, base.CharacterCard, damageToDeal, DamageType.Energy, isIrreducible: true);
                    if (base.UseUnityCoroutines)
                    {
                        yield return base.GameController.StartCoroutine(coroutine);
                    }
                    else
                    {
                        base.GameController.ExhaustCoroutine(coroutine);
                    }
                    card = storedResults.FirstOrDefault();
                }
            }
            if (card != null)
            {
                IEnumerator coroutine2 = DealDamage(base.Card, card, damageToDeal, DamageType.Energy, isIrreducible: true);
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
    }
}
