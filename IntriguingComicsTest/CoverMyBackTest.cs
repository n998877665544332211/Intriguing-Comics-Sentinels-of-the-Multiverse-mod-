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
    public class CoverMyBackTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void FrankDealsDamageWhenBossDamaged()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Megalopolis");

            Card breakaway = PlayCard("Breakaway");

            Card cover = PlayCard("CoverMyBack");

            QuickHPStorage(legacy);

            DealDamage(legacy, breakaway, 2, DamageType.Fire);

            QuickHPCheck(-1);
        }

        [Test()]
        public void FrankDoesNotDealDamageWhenNonBossDamaged()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Megalopolis");

            Card breakaway = PlayCard("Breakaway");

            Card cover = PlayCard("CoverMyBack");

            QuickHPStorage(legacy);

            DealDamage(legacy, frank, 2, DamageType.Fire);

            QuickHPCheck(0);
        }

    }
}
