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
    public class ThemedWeaponTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void IncreasesFrankDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Megalopolis");

            Card weapon = PlayCard("ThemedWeapon");

            QuickHPStorage(legacy);

            DealDamage(frank, legacy, 2, DamageType.Fire);

            QuickHPCheck(-3);
        }

        [Test()]
        public void IncreasesRestOfGangDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Megalopolis");

            Card weapon = PlayCard("ThemedWeapon");

            Card gang = PlayCard("TheRestOfTheGang");

            QuickHPStorage(legacy);

            DealDamage(gang, legacy, 2, DamageType.Fire);

            QuickHPCheck(-3);
        }

        [Test()]
        public void DoesNotIncreaseOtherVillainDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Megalopolis");

            AssertHitPoints(frank.CharacterCard, 13);

            Card weapon = PlayCard("ThemedWeapon");

            QuickHPStorage(legacy);

            DealDamage(weapon, legacy, 2, DamageType.Fire);

            QuickHPCheck(-2);
        }

        [Test()]
        public void ChangesDamageType()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Megalopolis");

            AssertHitPoints(frank.CharacterCard, 13);

            StartGame();

            Card rhyme = PlayCard("RhymeScheme");

            Card weapon = PlayCard("ThemedWeapon");

            Card flesh = PlayCard("FleshOfTheSunGod");

            QuickHPStorage(ra);

            DealDamage(rhyme, legacy, 2, DamageType.Fire);

            DealDamage(frank, ra, 2, DamageType.Melee);

            QuickHPCheck(0);
        }

        [Test()]
        public void ChangesDamageTypeTwice()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Megalopolis");

            AssertHitPoints(frank.CharacterCard, 13);

            StartGame();

            Card rhyme = PlayCard("RhymeScheme");

            Card weapon = PlayCard("ThemedWeapon");

            Card flesh = PlayCard("FleshOfTheSunGod");

            QuickHPStorage(ra);

            DealDamage(rhyme, legacy, 2, DamageType.Fire);

            DealDamage(rhyme, legacy, 2, DamageType.Energy);

            DealDamage(frank, ra, 2, DamageType.Melee);

            QuickHPCheck(-3);
        }

        [Test()]
        public void DontCheckDamageTypeBeforeEnteredPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Megalopolis");

            Card thraiw = PutOnDeck("Thraiw");

            DiscardTopCards(frank.TurnTaker.Deck, 1);

            AssertHitPoints(frank.CharacterCard, 13);

            StartGame();

            Card rhyme = PlayCard("RhymeScheme");

            Card flesh = PlayCard("FleshOfTheSunGod");

            QuickHPStorage(ra);

            DealDamage(rhyme, legacy, 2, DamageType.Fire);

            Card weapon = PlayCard("ThemedWeapon");

            DealDamage(frank, ra, 2, DamageType.Melee);

            QuickHPCheck(-3);
        }
    }
}