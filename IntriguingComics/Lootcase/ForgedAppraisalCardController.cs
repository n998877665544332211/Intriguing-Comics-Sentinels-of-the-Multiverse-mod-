using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class ForgedAppraisalCardController : CardController
    {
        public ForgedAppraisalCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            //Multiply the value of all Case cards in play by 2.
            //See LootcaseCharacterCardController
        }
    }
}
