using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class FamilyHeirloomsCardController : CardController
    {
        public FamilyHeirloomsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            //At the end of the environment turn, destroy H-1 equipment cards.
            base.AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, DestroyEquipmentResponse, TriggerType.DestroyCard);

        }
        private IEnumerator DestroyEquipmentResponse(PhaseChangeAction action)
        {
            //...destroy H - 1 equipment cards.
            IEnumerator coroutine = GameController.SelectAndDestroyCards(null, new LinqCardCriteria((Card c) => c.IsHero && (IsEquipment(c)), "equipment"), Game.H - 2, cardSource: GetCardSource());
            if (UseUnityCoroutines)
            {
                yield return GameController.StartCoroutine(coroutine);
            }
            else
            {
                GameController.ExhaustCoroutine(coroutine);
            }
        }
        
    }

}