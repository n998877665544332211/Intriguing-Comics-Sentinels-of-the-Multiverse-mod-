using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class BrainwashedBylinerCardController : PerilCardController
    {
        public BrainwashedBylinerCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            //At the end of the environment turn, Clara CLoverfield does each hero target other than herself 1 projectile damage.
            AddDealDamageAtEndOfTurnTrigger(TurnTaker, Clara, (Card c) => IsHeroTarget(c) && c.Identifier != "ClaraCloverfield", TargetType.All, 1, DamageType.Projectile);

            //At the end of the environment turn, Clara CLoverfield does each hero target other than herself 2 projectile damage.
            AddDealDamageAtStartOfTurnTrigger(TurnTaker, Clara, (Card c) => IsHeroTarget(c) && c.Identifier != "ClaraCloverfield", TargetType.All, 2, DamageType.Projectile);
        }
        private Card Clara => base.GameController.FindCardsWhere((Card c) => c.Identifier == "ClaraCloverfield").FirstOrDefault();

    }
}
