using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class FeatureStoryCardController : ScoopCardController
    {
        public FeatureStoryCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            //All damage to villain targets is irreducible.
            AddMakeDamageIrreducibleTrigger((DealDamageAction dd) => IsVillainTarget(dd.Target));
        }
    }
}
