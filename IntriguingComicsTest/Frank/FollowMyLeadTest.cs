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
    public class FollowMyLeadTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void FrankDealsDamageWhenBossDealsDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Megalopolis");

            Card breakaway = PlayCard("Breakaway");

            Card follow = PlayCard("FollowMyLead");

            QuickHPStorage(legacy);

            DealDamage(breakaway, legacy, 2, DamageType.Fire);

            QuickHPCheck(-3);
        }
    }
}
