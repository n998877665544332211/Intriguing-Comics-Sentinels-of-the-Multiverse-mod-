using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Globalization;
using System.Collections;
using System.Linq;

namespace IntriguingComics.Lootcase
{
    public class LootcaseTurnTakerController:TurnTakerController
    {

        
        public LootcaseTurnTakerController(TurnTaker turnTaker, GameController gameController)
            : base(turnTaker, gameController)
        {
        }

          
        public override IEnumerator StartGame()
        {

            // Advanced - At the start of the game, one "Yoink!" is put into play.  The villain deck is shuffled.
            if (base.GameController.Game.IsAdvanced)
            {

                // one "Yoink!" is put into play.
                Card card = this.TurnTaker.GetCardByIdentifier("Yoink");
                var playCard = this.GameController.PlayCard(this, card, true, cardSource: new CardSource(this.CharacterCardController));

                //  The villain deck is shuffled.
                var shuffle = this.GameController.ShuffleLocation(this.TurnTaker.Deck, cardSource: new CardSource(this.CharacterCardController));
                if (UseUnityCoroutines)
                {
                    yield return this.GameController.StartCoroutine(playCard);
                    yield return this.GameController.StartCoroutine(shuffle);
                }
                else
                {
                    this.GameController.ExhaustCoroutine(playCard);
                    this.GameController.ExhaustCoroutine(shuffle);
                }
            }
        }
    }
}