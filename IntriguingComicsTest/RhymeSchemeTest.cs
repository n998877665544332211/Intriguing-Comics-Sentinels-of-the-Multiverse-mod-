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
    public class RhymeSchemeTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void RhymeSchemeEndOfTurnDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            DestroyNonCharacterVillainCards();

            Card cover = PlayCard("CoverMyBack");

            Card rhyme = PlayCard("RhymeScheme");

            QuickHPStorage(legacy, ra, tempest);

            GoToEndOfTurn(frank);

            QuickHPCheck(-2, -2, 0);

        }


        [Test()]
        public void RhymeSchemeMakesPairedVillainCardsIndestructible()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            DestroyNonCharacterVillainCards();

            Card gang = PlayCard("TheRestOfTheGang");
            Card gang2 = PlayCard("TheRestOfTheGang");

            Card rhyme = PlayCard("RhymeScheme");

            DealDamage(legacy, gang, 10, DamageType.Melee);

            AssertIsInPlay(gang);
        }

        [Test()]
        public void RhymeSchemeMakesNonPairedVillainCardsNotIndestructible()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            DestroyNonCharacterVillainCards();

            Card gang = PlayCard("TheRestOfTheGang");

            Card rhyme = PlayCard("RhymeScheme");

            DealDamage(legacy, gang, 10, DamageType.Melee);

            AssertNotInPlay(gang);
        }

        [Test()]
        public void RhymeSchemeMakesPairedNonVillainCardsNotIndestructible()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            DestroyNonCharacterVillainCards();

            Card monorail = PlayCard("PlummetingMonorail");

            Card moonorail2 = PlayCard("PlummetingMonorail");

            DealDamage(legacy, monorail, 10, DamageType.Melee);

            AssertNotInPlay(monorail);
        }

        [Test()]
        public void CardsDestroyedAfterRhymeSchemeDestroyed()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            DestroyNonCharacterVillainCards();

            Card gang = PlayCard("TheRestOfTheGang");
            Card gang2 = PlayCard("TheRestOfTheGang");

            Card rhyme = PlayCard("RhymeScheme");

            DealDamage(legacy, gang, 10, DamageType.Melee);

            DealDamage(legacy, rhyme, 50, DamageType.Melee);

            AssertNotInPlay(gang);
        }

    }
}
