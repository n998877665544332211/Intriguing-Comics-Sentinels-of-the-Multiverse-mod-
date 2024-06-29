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
    public class TheDeviousDivisorTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void DeviousDivisorImmuneToIrreducibleTest()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            Card gang = PlayCard("TheDeviousDivisor");

            QuickHPStorage(frank);

            DealDamage(legacy, frank, 3, DamageType.Fire, true);

            QuickHPCheck(0);
        }

        [Test()]
        public void DeviousDivisorImmuneToMadeIrreducibleTest()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "MrFixer", "Megalopolis");

            Card gang = PlayCard("TheDeviousDivisor");

            Card claw = PlayCard("AlternatingTigerClaw");

            QuickHPStorage(frank);

            DealDamage(fixer, frank, 3, DamageType.Fire, true);

            QuickHPCheck(0);
        }

        [Test()]
        public void DeviousDivisorPowerDamageResponseTest()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "MrFixer", "Megalopolis");

            Card gang = PlayCard("TheDeviousDivisor");

            QuickHPStorage(ra);

            UsePower(ra);

            QuickHPCheck(-3);
        }

        [Test()]
        public void DeviousDivisorPowerDamageNoPowerNumeralsResponseTest()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tachyon", "Megalopolis");

            Card gang = PlayCard("TheDeviousDivisor");

            QuickHPStorage(tachyon);

            UsePower(tachyon);

            QuickHPCheck(0);
        }

        [Test()]
        public void DeviousDivisorPowerGuiseICanDoThatTooResponseTest()
        {
            SetupGameController("IntriguingComics.Frank", "Ra", "TheWraith", "Guise", "Megalopolis");

            Card gang = PlayCard("TheDeviousDivisor");

            QuickHPStorage(guise, ra);

            DecisionSelectPower = ra.CharacterCard;
            Card guy = PlayCard("ICanDoThatToo");

            QuickHPCheck(-3, 0);
        }

        [Test()]
        public void DeviousDivisorPowerGuiseLemmeSeeThatResponseTest()
        {
            SetupGameController("IntriguingComics.Frank", "Ra", "TheWraith", "Guise", "Megalopolis");

            Card gang = PlayCard("TheDeviousDivisor");

            QuickHPStorage(guise, wraith);

            Card stun = PlayCard("StunBolt");

            Card lemme = PlayCard("LemmeSeeThat");


            UsePower(stun);

            QuickHPCheck(-3, 0);
        }

        [Test()]
        public void DeviousDivisorPowerTombOfAnubisResponseTest()
        {
            SetupGameController("IntriguingComics.Frank", "Ra", "TheWraith", "Guise", "TombOfAnubis");

            Card gang = PlayCard("TheDeviousDivisor");

            QuickHPStorage(ra);

            Card rod = PlayCard("RodOfAnubis");
            Card spike = PlayCard("SpikeTrap");

            DecisionSelectTurnTaker = ra.TurnTaker;
            DestroyCard(spike);

            UsePower(rod);

            QuickHPCheck(-3);
        }

        [Test()]
        public void DeviousDivisorPowerResponseMultiplePowersSameCardWithNumeralTest()
        {
            SetupGameController("IntriguingComics.Frank", "Ra", "TheWraith", "Tempest", "TombOfAnubis");

            Card gang = PlayCard("TheDeviousDivisor");

            Card hurricane = PlayCard("LocalizedHurricane");

            QuickHPStorage(tempest);

            UsePower(hurricane, 0);

            QuickHPCheck(-8);
        }

        [Test()]
        public void DeviousDivisorPowerResponseMultiplePowersSameCardWithoutNumeralTest()
        {
            SetupGameController("IntriguingComics.Frank", "Ra", "TheWraith", "Tempest", "TombOfAnubis");

            Card gang = PlayCard("TheDeviousDivisor");

            Card hurricane = PlayCard("LocalizedHurricane");

            QuickHPStorage(tempest);

            UsePower(hurricane, 1);

            QuickHPCheck(0);
        }

    }
}
