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
    public class ZurkallaTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void ZurkallaEndOfTurnDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            DestroyNonCharacterVillainCards();

            Card zurkalla = PlayCard("Zurkalla");

            QuickHPStorage(ra);
            
            GoToEndOfTurn(frank);

            QuickHPCheck(-3);
        }

        [Test()]
        public void ZurkallaEndOfTurnDamageTargetsEnvironment()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            StartGame();

            DestroyNonCharacterVillainCards();

            Card monorail = PlayCard("PlummetingMonorail");

            Card zurkalla = PlayCard("Zurkalla");

            SetHitPoints(ra, 6);
            SetHitPoints(tempest, 5);

            QuickHPStorage(monorail);

            GoToEndOfTurn(frank);

            QuickHPCheck(-3);
        }

        [Test()]
        public void ZurkallaFollowUpDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            Card zurkalla = PlayCard("Zurkalla");

            QuickHPStorage(legacy);

            DealDamage(frank, legacy, 1, DamageType.Fire);

            QuickHPCheck(-3);
        }

        [Test()]
        public void ZurkallaNoFollowUpDamageToNonHeroTarget()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            RemoveVillainTriggers();

            Card monorail = PlayCard("PlummetingMonorail");

            Card zurkalla = PlayCard("Zurkalla");

            QuickHPStorage(monorail);

            DealDamage(frank, monorail, 1, DamageType.Fire);

            QuickHPCheck(-1);
        }
    }
}