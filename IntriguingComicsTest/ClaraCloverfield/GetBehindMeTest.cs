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
    public class GetBehindMeTest : BaseTest
    {
        protected TurnTakerController clara { get { return FindEnvironment(); } }

        [Test()]

        public void GetBehindMeRedirectsDamageToHeroWithLowestHP()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card behind = PlayCard("GetBehindMe");

            DecisionYesNo = true;

            QuickHPStorage(tempest);

            DealDamage(baron, clarac, 4, DamageType.Fire);

            QuickHPCheck(-4);

            AssertHitPoints(clarac, 15);
        }

        [Test()]

        public void GetBehindMeNoRedirectChosen()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card behind = PlayCard("GetBehindMe");

            DecisionYesNo = false;

            QuickHPStorage(clarac);

            DealDamage(baron, clarac, 4, DamageType.Fire);

            QuickHPCheck(-4);

            AssertHitPoints(tempest, 26);
        }
    }
}