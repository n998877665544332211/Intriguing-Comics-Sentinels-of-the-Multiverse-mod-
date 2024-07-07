using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class HostageHoldingHenchmanCardController : PerilCardController
    {
        public HostageHoldingHenchmanCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {

            //Environment cards cannot be played.
            CannotPlayCards(null, (Card c) => c.IsEnvironment);

            //At the end of the environment turn, this card deals Clara Cloverfield 3 Melee damage.
            AddDealDamageAtEndOfTurnTrigger(TurnTaker, this.Card, (Card c) => c.Identifier == "ClaraCloverfield", TargetType.HighestHP, 3, DamageType.Melee);
        }
    }
}
