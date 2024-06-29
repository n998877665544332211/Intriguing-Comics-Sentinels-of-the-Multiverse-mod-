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
    public class TheBaronessTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void BaronessEndOfTurnDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            DestroyNonCharacterVillainCards();

            Card cover = PlayCard("CoverMyBack");

            Card baroness = PlayCard("TheBaroness");

            QuickHPStorage(legacy, ra, tempest);

            GoToEndOfTurn(frank);

            QuickHPCheck(-4, 0, 0);
        }

        [Test()]
        public void ReduceDamageToHeroTo0DestroyOngoing()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            Card fortitude = PlayCard("Fortitude");

            Card baroness = PlayCard("TheBaroness");

            QuickHPStorage(legacy);

            DealDamage(frank, legacy, 1, DamageType.Melee);

            QuickHPCheck(0);

            AssertNotInPlay(fortitude);
        }

        [Test()]
        public void ReduceDamageByHeroTo0NoDestroyOngoing()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            Card fortitude = PlayCard("Fortitude");

            Card baroness = PlayCard("TheBaroness");

            Card outfit = PlayCard("AssignedOutfit");

            QuickHPStorage(frank);

            DealDamage(legacy, frank, 1, DamageType.Melee);

            QuickHPCheck(0);

            AssertIsInPlay(fortitude);
        }

        [Test()]
        public void ReduceDamageToHeroAbove0NoDestroyOngoing()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            Card fortitude = PlayCard("Fortitude");

            Card baroness = PlayCard("TheBaroness");

            QuickHPStorage(legacy);

            DealDamage(frank, legacy, 2, DamageType.Melee);

            QuickHPCheck(-1);

            AssertIsInPlay(fortitude);
        }

        [Test()]
        public void PreventDamageToHeroDestroyOngoing()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Fanatic", "Megalopolis");

            Card undaunted = PlayCard("Undaunted");

            Card baroness = PlayCard("TheBaroness");

            QuickHPStorage(fanatic);

            DealDamage(frank, fanatic, 1, DamageType.Melee);

            QuickHPCheck(0);

            AssertNotInPlay(undaunted);
        }

        [Test()]
        public void HeroImmuneNoDestroyOngoing()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Fanatic", "Megalopolis");

            Card flesh = PlayCard("FleshOfTheSunGod");

            Card baroness = PlayCard("TheBaroness");

            QuickHPStorage(ra);

            DealDamage(frank, ra, 1, DamageType.Fire);

            QuickHPCheck(0);

            AssertIsInPlay(flesh);
        }

        [Test()]
        public void NotReduced0DamageNoDestroyOngoing()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "SkyScraper", "Megalopolis");

            Card danger = PlayCard("DangerSense");

            Card baroness = PlayCard("TheBaroness");

            QuickHPStorage(legacy);

            DealDamage(frank, legacy, 0, DamageType.Melee);

            QuickHPCheck(0);

            AssertIsInPlay(danger);
        }

        [Test()]
        public void IncreasedThenReduceBackTo0DamageDestroyOngoing()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "SkyScraper", "Megalopolis");

            Card weapon = PlayCard("ThemedWeapon");

            Card fortitude = PlayCard("Fortitude");

            Card baroness = PlayCard("TheBaroness");

            QuickHPStorage(legacy);

            DealDamage(frank, legacy, 0, DamageType.Melee); 

            QuickHPCheck(0);

            AssertNotInPlay(fortitude);
        }


        [Test()]
        public void RedirectNoDestroyOngoing()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "SkyScraper", "TheEnclaveOfTheEndlings");

            Card immutus = PlayCard("Immutus");

            Card fortitude = PlayCard("Fortitude");

            Card baroness = PlayCard("TheBaroness");

            QuickHPStorage(immutus);

            DealDamage(frank, legacy, 5, DamageType.Melee);

            QuickHPCheck(-5);

            AssertIsInPlay(fortitude);
        }

        [Test()]
        public void BaronessRadiantDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "SkyScraper", "Megalopolis");

            Card baroness = PlayCard("TheBaroness");

            QuickHPStorage(baroness);

            DealDamage(legacy, baroness, 1, DamageType.Radiant);

            QuickHPCheck(-2);
        }
    }
}
