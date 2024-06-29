using NUnit.Framework;
using System;
using IntriguingComics.Frank;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Linq;
using System.Collections;
using Handelabra.Sentinels.UnitTest;
using System.Collections.Generic;

namespace IntriguingComicsTest
{
    [TestFixture()]
    public class LuckyPunchTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void LuckyPunchDealsDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            QuickHPStorage(wraith);

            Card breakaway = PlayCard("LuckyPunch");

            QuickHPCheck(-4);
        }
    }
}
