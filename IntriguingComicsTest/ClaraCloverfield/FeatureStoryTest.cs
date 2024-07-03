using NUnit.Framework;
using System;
using IntriguingComics.ClaraCloverfield;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Linq;
using System.Collections;
using Handelabra.Sentinels.UnitTest;
using System.Collections.Generic;
using Handelabra.Sentinels.Engine.Controller.Legacy;

namespace IntriguingComicsTest
{
    [TestFixture()]
    public class FeatureStoryTest : BaseTest
    {
        protected TurnTakerController clara { get { return FindEnvironment(); } }

        [Test()]

        public void FeatureStoryMakesDamageToVillainTargetsIrreducible()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card feature = PlayCard("FeatureStory");

            Card guard = PlayCard("GeneBoundGuard");

            QuickHPStorage(voss);

            DealDamage(legacy, voss, 1, DamageType.Melee);

            QuickHPCheck(-1);
        }

        [Test()]
        public void FeatureStoryMakesDamageToVillainTargetsFromNonHeroSourceIrreducible()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card feature = PlayCard("FeatureStory");

            Card guard = PlayCard("GeneBoundGuard");

            QuickHPStorage(voss);

            DealDamage(guard, voss, 1, DamageType.Melee);

            QuickHPCheck(-1);
        }
    }
}