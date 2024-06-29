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
    public class TheRestOfTheGangTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void TheRestOfTheGangDealsDamageToOtherHeroTargets()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            Card gang = PlayCard("TheRestOfTheGang");

            QuickHPStorage(ra);

            DealDamage(frank, legacy, 1, DamageType.Fire);

            QuickHPCheck(-1);
        }

        [Test()]
        public void TheRestOfTheGangDoesNotDamageNonHeroTargets()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            Card gang = PlayCard("TheRestOfTheGang");

            Card monorail = PlayCard("PlummetingMonorail");

            QuickHPStorage(monorail);

            DealDamage(frank, legacy, 1, DamageType.Fire);

            QuickHPCheck(0);
        }

        [Test()]
        public void TheRestOfTheGangDoesNotDealDamageIfFrankDoesNot()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            Card gang = PlayCard("TheRestOfTheGang");
            Card fortitude = PlayCard("Fortitude");

            QuickHPStorage(ra);

            DealDamage(frank, legacy, 1, DamageType.Fire);

            QuickHPCheck(0);
        }

        [Test()]
        public void TheRestOfTheGangDoesNotDamageIfFrankDamagesNonHero()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            Card gang = PlayCard("TheRestOfTheGang");

            Card monorail = PlayCard("PlummetingMonorail");

            QuickHPStorage(ra);

            DealDamage(frank, monorail, 1, DamageType.Fire);

            QuickHPCheck(0);
        }

    }
}
