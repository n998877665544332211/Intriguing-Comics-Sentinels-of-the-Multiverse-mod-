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
    public class MindReaperTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void MindReaperEndOfTurnDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Haka", "TheWraith", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card reaper = PlayCard("MindReaper");

            QuickHPStorage(frank, tempest, wraith, haka);

            GoToNextTurn();

            QuickHPCheck(-1, -1, -1, -5);
        }

        [Test()]
        public void MindReaperTargetsNotDealtDamageDoNotDealDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Haka", "TheWraith", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card reaper = PlayCard("MindReaper");

            Card resilience = PlayCard("OtherworldlyResilience");

            QuickHPStorage(tempest, haka);

            GoToNextTurn();

            QuickHPCheck(0, -4);
        }


        [Test()]
        public void MindReaperDestroyedTargetsDoNotDealDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Haka", "TheWraith", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card reaper = PlayCard("MindReaper");

            Card monorail = PlayCard("PlummetingMonorail");
            SetHitPoints(monorail, 1);

            QuickHPStorage(haka);

            GoToNextTurn();

            QuickHPCheck(-5);
        }

        [Test()]
        public void MindReaperNoSelfDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Haka", "TheWraith", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card reaper = PlayCard("MindReaper");

            QuickHPStorage(reaper);

            GoToNextTurn();

            QuickHPCheck(0);
        }

        [Test()]
        public void MindReaperNoDamageToDevices()
        {
            SetupGameController("IntriguingComics.Frank", "Haka", "TheWraith", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();
            DestroyNonCharacterVillainCards();

            Card reaper = PlayCard("MindReaper");

            Card outfit = PlayCard("AssignedOutfit");

            QuickHPStorage(outfit);

            GoToNextTurn();

            QuickHPCheck(0);
        }
    }
}