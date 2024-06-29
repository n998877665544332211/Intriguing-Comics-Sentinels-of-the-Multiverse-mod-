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
    public class TheRegionalManagerTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void TheRegionalManagerPutsOrderIntoPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card manager = PlayCard("TheRegionalManager");

            Card order = GetTopMatchingCardOfLocation(frank.TurnTaker.Deck, (Card c) => c.DoKeywordsContain("order"));

            GoToNextTurn();

            AssertIsInPlay(order);
        }

        [Test()]
        public void TheRegionalManagerBringsBackTheRestOfTheGang()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card manager = PlayCard("TheRegionalManager");

            Card gang = PlayCard("TheRestOfTheGang");

            DestroyCard(gang);

            GoToNextTurn();

            GoToStartOfTurn(frank);

            AssertIsInPlay(gang);
        }
    }
}
