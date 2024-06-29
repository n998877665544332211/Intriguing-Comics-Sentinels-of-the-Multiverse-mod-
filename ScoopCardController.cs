using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public abstract class ScoopCardController : CardController
    {
        public ScoopCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }
		public override IEnumerator Play()
		{
			// When this card enters play, destroy all Rescue cards in play.
			IEnumerator coroutine = base.GameController.DestroyCards(DecisionMaker, new LinqCardCriteria((Card c) => c.DoKeywordsContain("rescue")), autoDecide: false, null, null, null, SelectionType.DestroyCard, GetCardSource());
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