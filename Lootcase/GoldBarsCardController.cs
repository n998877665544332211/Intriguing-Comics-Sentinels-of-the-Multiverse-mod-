using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class GoldBarsCardController : CardController
    {
        public GoldBarsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowHeroTargetWithHighestHP(numberOfTargets: 2);
        }

        public override void AddTriggers()
        {
            //At the end of the villain turn, Lootcase deals the 2 hero targets with the highest HP {H-1} melee damage each.
            base.AddDealDamageAtEndOfTurnTrigger(base.TurnTaker, base.CharacterCard, (Card c) => IsHeroTarget(c), TargetType.HighestHP, base.Game.H - 1, DamageType.Melee, numberOfTargets: 2);
        }
    }
}
