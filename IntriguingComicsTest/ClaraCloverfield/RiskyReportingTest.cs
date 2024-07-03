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
    public class RiskyReportingTest : BaseTest
    {
        protected TurnTakerController clara { get { return FindEnvironment(); } }

        [Test()]
        public void RiskyReportingRedirectToClaraHighestHPVillainHighestHPHero()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card risky = PlayCard("RiskyReporting");

            QuickHPStorage(clarac);

            DealDamage(baron, legacy, 3, DamageType.Fire);

            QuickHPCheck(-3);
        }

        [Test()]
        public void RiskyReportingNoRedirectNotHighestHPHero()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card risky = PlayCard("RiskyReporting");

            QuickHPStorage(clarac);

            DealDamage(baron, ra, 3, DamageType.Fire);

            QuickHPCheck(0);
        }

        [Test()]
        public void RiskyReportingNoRedirectNotHighestHPVillain()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card risky = PlayCard("RiskyReporting");

            Card battalion = PlayCard("BladeBattalion");

            QuickHPStorage(clarac);

            DealDamage(battalion, legacy, 3, DamageType.Fire);

            QuickHPCheck(0);
        }
    }
}