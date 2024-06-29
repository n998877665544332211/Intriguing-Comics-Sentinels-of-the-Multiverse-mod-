using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class BreakingNewsCardController : CardController
    {
        public BreakingNewsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowHeroCharacterCardWithLowestHP(numberOfTargets: 1);
        }

		//When this card enters play, the hero with the lowest HP may search their deck for an ongoing or equipment card and put it into play.
		public override IEnumerator Play()
		{

            IEnumerator coroutine = base.GameController.DestroyCards(DecisionMaker, new LinqCardCriteria((Card c) => c.DoKeywordsContain("rescue")), autoDecide: false, null, null, null, SelectionType.DestroyCard, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }


            List<Card> storedHero = new List<Card>();
            IEnumerator coroutine2 = base.GameController.FindTargetsWithLowestHitPoints(1, 1, (Card card) => card.IsHeroCharacterCard && card.IsTarget, storedHero, cardSource: base.GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine2);
            }
            else
            {
                GameController.ExhaustCoroutine(coroutine2);
            }
            if (storedHero.FirstOrDefault() != null)
            {
            }


            TurnTakerController ttc = FindTurnTakerController(storedHero.FirstOrDefault().Owner);

            TurnTaker tt = ttc.TurnTaker;
            if (!tt.IsHero)
            {
                yield break;
            }
            HeroTurnTakerController heroTurnTakerController = base.GameController.FindHeroTurnTakerController(tt.ToHero());


            IEnumerator coroutine3 = SearchForCards(heroTurnTakerController, searchDeck: true, searchTrash: false, 1, 1, new LinqCardCriteria((Card c) => IsOngoing(c) || IsEquipment(c), "ongoing or equipment"), putIntoPlay: true, putInHand: false, putOnDeck: false, optional: false, null, autoDecideCard: false);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine3);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine3);
            }
        }
       
        
        //At the end of the environment turn, destroy this card.
        public override void AddTriggers()
		{
			AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, base.DestroyThisCardResponse, TriggerType.DestroySelf);
		}
	}
}
