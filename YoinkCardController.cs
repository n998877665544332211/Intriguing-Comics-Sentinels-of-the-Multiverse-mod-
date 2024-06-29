using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class YoinkCardController : CardController
    {
        public YoinkCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
			base.SpecialStringMaker.ShowSpecialString(() => $"The current loot value of this card is ${YoinkValue.ToString("#,#", CultureInfo.InvariantCulture)}.");
			Card.UnderLocation.OverrideIsInPlay = false;
            SpecialStringMaker.ShowNumberOfCardsUnderCard(Card);
        }
        public override void AddTriggers()
        {
			//At the end of the Villain turn, each player moves a card from their hand, chosen randomly, under this card.  Cards under this card have no text.
			AddEndOfTurnTrigger((TurnTaker tt) => tt == TurnTaker, MoveCardsResponse, TriggerType.MoveCard);
        }

		//  Calculates loot value for the string, but the real calculation happens on LootcaseCharacterCardController.
		private int YoinkValue
		{
			get
			{
				return ((base.FindCardsWhere((Card c) => c.IsUnderCard).Count())*50000);
			}
		}

		private IEnumerator MoveCardsResponse (PhaseChangeAction action)
        {
			IEnumerator coroutine4 = base.GameController.SendMessageAction("Yoink!  Lootcase steals a random card from each player's hand.", Priority.Low, GetCardSource(), null, showCardSource: true);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine4);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine4);
			}




			//At the end of the Villain turn, each player moves a card from their hand, chosen randomly, under this card.  Cards under this card have no text.
			IEnumerator coroutine = DoActionToEachTurnTakerInTurnOrder((TurnTakerController ttc) => ttc.IsHero && !ttc.IsIncapacitatedOrOutOfGame, (TurnTakerController ttc) => MoveCardsUnderResponse(ttc));
            if (this.UseUnityCoroutines)
            {
                yield return this.GameController.StartCoroutine(coroutine);
            }
            else
            {
                this.GameController.ExhaustCoroutine(coroutine);
            }
            yield break;
        }




		private IEnumerator MoveCardsUnderResponse(TurnTakerController ttc)
		{
			List<Card> storedResults = new List<Card>();
			TurnTaker tt = ttc.TurnTaker;
			if (!tt.IsHero)
			{
				yield break;
			}
			HeroTurnTakerController heroTurnTakerController = base.GameController.FindHeroTurnTakerController(tt.ToHero());
			if (heroTurnTakerController.HeroTurnTaker.Hand.Cards.Count() > 0)
			{
				int amount = heroTurnTakerController.HeroTurnTaker.Hand.Cards.Count();
				IEnumerable<Card> rndCard = heroTurnTakerController.HeroTurnTaker.Hand.Cards.TakeRandom(amount, base.GameController.Game.RNG);
				if (rndCard.Count() > 0)
				{
					IEnumerator coroutine = base.GameController.MoveCard(DecisionMaker, rndCard.First(), Card.UnderLocation, showMessage: false, cardSource: base.GetCardSource());
					if (base.UseUnityCoroutines)
					{
						yield return base.GameController.StartCoroutine(coroutine);
					}
					else
					{
						base.GameController.ExhaustCoroutine(coroutine);
					}
					storedResults?.Add(rndCard.First());
				}
				if (storedResults.Count() > 0)
				{
						IEnumerator coroutine2 = base.GameController.SendMessageAction("Lootcase stole " + storedResults.First().Title + " from " + tt.Name + "'s hand and moved it under Yoink!", Priority.Medium, GetCardSource(), null, showCardSource: true);
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
			else
			{
				IEnumerator coroutine3 = base.GameController.SendMessageAction(tt.Name + " does not have any cards for Lootcase to steal.", Priority.Low, GetCardSource(), null, showCardSource: true);
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
	}