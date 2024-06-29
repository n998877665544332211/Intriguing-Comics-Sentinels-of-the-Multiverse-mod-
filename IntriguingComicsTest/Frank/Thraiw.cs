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
    public class ThraiwTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void ThraiwEndOfTurnDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Tachyon", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            DestroyNonCharacterVillainCards();

            Card thraiw = PlayCard("Thraiw");

            QuickHPStorage(legacy, ra, tempest, tachyon);

            GoToEndOfTurn(frank);

            QuickHPCheck(-3, -3, 0, 0);
        }

        [Test()]
        public void ThraiwDamageReduction()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Tachyon", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            DestroyNonCharacterVillainCards();

            Card thraiw = PlayCard("Thraiw");

            GoToEndOfTurn(frank);

            QuickHPStorage(thraiw);

            DealDamage(legacy, thraiw, 2, DamageType.Fire);

            QuickHPCheck(-1);
        }

        [Test()]
        public void ThraiwPutDeviceInPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Tachyon", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            DestroyNonCharacterVillainCards();

            Card thraiw = PlayCard("Thraiw");

            GoToNextTurn();

            Card device = GetTopMatchingCardOfLocation(frank.TurnTaker.Deck, (Card c) => c.DoKeywordsContain("device"));

            GoToStartOfTurn(frank);

            AssertIsInPlay(device);
        }


    }
}
