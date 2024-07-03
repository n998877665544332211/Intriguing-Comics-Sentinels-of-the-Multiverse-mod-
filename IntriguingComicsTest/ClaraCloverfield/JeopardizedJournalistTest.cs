using NUnit.Framework;
using System;
using IntriguingComics.ClaraCloverfield;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Linq;
using System.Collections;
using Handelabra.Sentinels.UnitTest;
using System.Collections.Generic;
using Handelabra.Sentinels.Engine.Controller.Legacy;

namespace IntriguingComicsTest
{
    [TestFixture()]
    public class JeopardizedJournalistTest : BaseTest
    {
        protected TurnTakerController clara { get { return FindEnvironment(); } }

        [Test()]
        public void JeopardizedJournalistGameOver()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card journalist = PlayCard("JeopardizedJournalist");

            StartGame();

            DecisionYesNo = false;

            GoToStartOfTurn(clara);

            AssertGameOver(EndingResult.EnvironmentDefeat);
        }

        [Test()]
        public void JeopardizedJournalistSkipTurnToDestroy()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card journalist = PlayCard("JeopardizedJournalist");

            StartGame();

            DecisionYesNo = true;

            GoToStartOfTurn(legacy);

            AssertNotInPlay(journalist);

            EnterNextTurnPhase();

            AssertCurrentTurnPhase(ra, Phase.Start);
        }
    }
}