using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public abstract class PerilCardController : CardController
    {
        public PerilCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }
		public override IEnumerator Play()
		{
			//When this card enters play, destroy all Scoop cards in play.
			IEnumerator coroutine = base.GameController.DestroyCards(DecisionMaker, new LinqCardCriteria((Card c) => c.DoKeywordsContain("scoop")), autoDecide: false, null, null, null, SelectionType.DestroyCard, GetCardSource());
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
