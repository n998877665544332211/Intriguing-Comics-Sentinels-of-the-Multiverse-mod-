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
    public class TheHeelTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void TheHeelPutsOrderIntoPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card heel = PlayCard("TheHeel");

            Card cover = PutOnDeck("CoverMyBack");

            GoToNextTurn();

            AssertIsInPlay(cover);
        }

        [Test()]
        public void TheHeelPutsDeviceIntoPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card heel = PlayCard("TheHeel");

            Card outfit = PutOnDeck("AssignedOutfit");

            GoToNextTurn();

            AssertIsInPlay(outfit);
        }

        [Test()]
        public void TheHeelPutsTheRestOfTheGangIntoPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card heel = PlayCard("TheHeel");

            Card gang = PutOnDeck("TheRestOfTheGang");

            GoToNextTurn();

            AssertIsInPlay(gang);
        }

        [Test()]
        public void TheHeelDoesNotPutOtherCardsIntoPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card heel = PlayCard("TheHeel");

            Card rhyme = PutOnDeck("RhymeScheme");

            GoToNextTurn();

            AssertNotInPlay(rhyme);
        }

        [Test()]
        public void TheHeelDoesDamageIfDamageWasDealt()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card cover = PutOnDeck("CoverMyBack");
            Card heel = PlayCard("TheHeel");

            GoToNextTurn();

            DealDamage(ra, frank, 2, DamageType.Fire);

            QuickHPStorage(tempest);

            GoToStartOfTurn(frank);

            QuickHPCheck(-3);
        }

        [Test()]
        public void TheHeelDoesNotDamageIfNoDamageWasDealt()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card heel = PlayCard("TheHeel");

            GoToNextTurn();

            QuickHPStorage(tempest);

            GoToStartOfTurn(frank);

            QuickHPCheck(0);
        }

        [Test()]
        public void TheHeelDoesNoDamageIfDamageWasDealtBeforeSheWasPlayed()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            DealDamage(ra, frank, 2, DamageType.Fire);

            Card heel = PlayCard("TheHeel");
            
            QuickHPStorage(tempest);

            GoToStartOfTurn(frank);

            QuickHPCheck(0);
        }
    }
}
