using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class CollectorsItemCardController : CardController
    {
        public CollectorsItemCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowNumberOfCardsAtLocation(base.TurnTaker.Trash, new LinqCardCriteria((Card c) => c.Identifier == YoinkIdentifier, "Yoink!"));
        }

        public readonly string YoinkIdentifier = "Yoink";

        public override IEnumerator Play()
        {
            IEnumerator coroutine4 = base.GameController.SendMessageAction("Collector's Item searches the villain trash for a copy of Yoink! to put into play.", Priority.Low, GetCardSource(), null, showCardSource: true);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine4);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine4);
            }


            IEnumerator coroutine;
            //When this card enters play, search the Villain trash for a card called "Yoink!" and put it into play.
            IEnumerable<Card> YoinkTrash = base.GameController.FindCardsWhere(new LinqCardCriteria((Card c) => c.Identifier == YoinkIdentifier && c.Location.IsTrash && c.Location.IsVillain));
            if (YoinkTrash.Any())
            {
                coroutine = base.GameController.PlayCard(TurnTakerController, YoinkTrash.FirstOrDefault(), isPutIntoPlay: true, cardSource: GetCardSource());
                if (UseUnityCoroutines)
                {
                    yield return GameController.StartCoroutine(coroutine);
                }
                else
                {
                    GameController.ExhaustCoroutine(coroutine);
                }
            }
            else
            {
                IEnumerator coroutine2 = base.GameController.SendMessageAction("There are no copies of Yoink! in the villain trash.", Priority.Low, GetCardSource(), null, showCardSource: true);
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