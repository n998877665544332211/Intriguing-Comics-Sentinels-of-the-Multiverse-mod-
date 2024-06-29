using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class ShinyJewelsCardController : CardController
    {
        public ShinyJewelsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            //At the end of the Villain turn, this card deals each Hero target 2 radiant damage.
            AddDealDamageAtEndOfTurnTrigger(TurnTaker, this.Card, (Card c) => IsHeroTarget(c), TargetType.All, 2, DamageType.Radiant);
        }
    }
}
