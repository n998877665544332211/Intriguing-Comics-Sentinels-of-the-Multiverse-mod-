using NUnit.Framework;
using System;
using IntriguingComics.ClaraCloverfield;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Linq;
using System.Collections;
using Handelabra.Sentinels.UnitTest;
using System.Collections.Generic;

namespace IntriguingComicsTest
{
    [TestFixture()]
    public class ClaraCloverfieldCardTest : BaseTest
    {

        protected TurnTakerController clara { get { return FindEnvironment(); } }


        [Test()]

        public void ClaraPutIntoPlayAtStartOfGame()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = FindCard((Card c) => c.Identifier == "ClaraCloverfield");
            StartGame();

            AssertIsInPlay(clarac);
        }

        [Test()]
        public void ClaraIsIndestructibleWithMoreThan0HP()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = FindCard((Card c) => c.Identifier == "ClaraCloverfield");
            StartGame();

            DestroyCard(clarac);

            AssertIsInPlay(clarac);
        }

        [Test()]
        public void ClaraDestroyedGameOver()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = FindCard((Card c) => c.Identifier == "ClaraCloverfield");

            StartGame();

            AssertHitPoints(clarac, 15);

            DealDamage(legacy, clarac, 15, DamageType.Melee);

            AssertGameOver(EndingResult.EnvironmentDefeat);
        }

        [Test()]
        public void ClaraRemovedFromPlayGameOver()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "ChronoRanger", "IntriguingComics.ClaraCloverfield");

            Card clarac = FindCard((Card c) => c.Identifier == "ClaraCloverfield");

            StartGame();

            DecisionSelectCard = clarac;

            Card no = PlayCard("NoExecutions");

            AssertHitPoints(clarac, 15);

            DealDamage(legacy, clarac, 15, DamageType.Melee);

            AssertGameOver(EndingResult.EnvironmentDefeat);
        }
    }
}