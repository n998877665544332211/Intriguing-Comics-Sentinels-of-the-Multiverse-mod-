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
    public class GetThemTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void GetThemDealsDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            DestroyNonCharacterVillainCards();

            Card get = PlayCard("GetThem");

            QuickHPStorage(legacy);

            GoToEndOfTurn(frank);

            QuickHPCheck(-1);
        }
    }
}