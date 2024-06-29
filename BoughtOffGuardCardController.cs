using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class BoughtOffGuardCardController : CardController
    {
        public BoughtOffGuardCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            //At the end of the Villain turn, play the top X cards of the Villain deck, where X equals the number of Environment cards in play.
            AddEndOfTurnTrigger(tt => tt == TurnTaker, PlayCardsBasedOnEnvironment, TriggerType.PlayCard);
        }

        private IEnumerator PlayCardsBasedOnEnvironment(GameAction ga)
        {
            Func<int> numEnvironment = () => FindCardsWhere(new LinqCardCriteria((Card c) => c.IsEnvironment && c.IsInPlay)).Count();
            IEnumerator coroutine;
            for (int i = 1; i <= numEnvironment(); i++)
            {
                string ordinal = "a";
                switch (i)
                {
                    case 2:
                        {
                            ordinal = "a second";
                            break;
                        }
                    case 3:
                        {
                            ordinal = "a third";
                            break;
                        }
                    case 4:
                        {
                            ordinal = "a fourth";
                            break;
                        }
                    case 5:
                        {
                            ordinal = "a fifth";
                            break;
                        }
                    case 6:
                        {
                            ordinal = "a sixth";
                            break;
                        }
                    case 7:
                        {
                            ordinal = "a seventh";
                            break;
                        }
                    case 8:
                        {
                            ordinal = "an eighth";
                            break;
                        }
                    case 9:
                        {
                            ordinal = "a ninth";
                            break;
                        }
                    case 10:
                        {
                            ordinal = "a tenth";
                            break;
                        }
                    case 11:
                        {
                            ordinal = "an eleventh";
                            break;
                        }
                    case 12:
                        {
                            ordinal = "a twelfth";
                            break;
                        }
                    case 13:
                        {
                            ordinal = "a thirteenth";
                            break;
                        }
                    case 14:
                        {
                            ordinal = "a fourteenth";
                            break;
                        }
                    case 15:
                        {
                            ordinal = "a fifteenth";
                            break;
                        }
                        //not possible to get to 16
                }
                coroutine = GameController.SendMessageAction($"Bought-Off Guard plays {ordinal} card...", Priority.Medium, GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
                coroutine = PlayTheTopCardOfTheVillainDeckResponse(ga);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }

            }
            yield break;
        }
    }
}