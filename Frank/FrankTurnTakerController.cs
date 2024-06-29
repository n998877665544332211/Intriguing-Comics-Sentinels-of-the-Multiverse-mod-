using System.Collections;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class FrankTurnTakerController : TurnTakerController
    {
        public FrankTurnTakerController(TurnTaker turnTaker, GameController gameController)
            : base(turnTaker, gameController)
        {
        }

        public override IEnumerator StartGame()
        {
            IEnumerator coroutine = PutCardsIntoPlay(new LinqCardCriteria((Card c) => IsOrder(c), "order"), 1);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
        }

        private bool IsOrder(Card c)
        {
            return c.DoKeywordsContain("order");
        }
    }
}
