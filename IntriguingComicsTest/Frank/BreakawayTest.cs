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
    public class BreakawayTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void BreakawayPutsGetThemIntoPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card breakaway = PlayCard("Breakaway");

            AssertNumberOfCardsAtLocation(frank.TurnTaker.PlayArea, 1, (Card c) => c.Identifier == "GetThem");
        }

        [Test()]
        public void BreakawayStartHP()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Megalopolis");


            Card breakaway = PlayCard("Breakaway");

            AssertHitPoints(breakaway, 40);
        }

        [Test()]
        public void BreakawayGainsHP()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            Card breakaway = PlayCard("Breakaway");

            QuickHPStorage(breakaway);

            DealDamage(frank, legacy, 2, DamageType.Fire);

            QuickHPCheck(+3);
        }

        [Test()]
        public void BreakawaySpecialDefeat()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            Card breakaway = PlayCard("Breakaway");

            GainHP(breakaway, 20);

            GoToNextTurn();

            GoToStartOfTurn(frank);

            AssertGameOver(EndingResult.AlternateDefeat);
        }

    }
}
