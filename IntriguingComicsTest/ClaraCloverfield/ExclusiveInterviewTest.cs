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
    public class ExclusiveInterviewTest : BaseTest
    {
        protected TurnTakerController clara { get { return FindEnvironment(); } }

        [Test()]
        public void ExclusiveInterviewDiscard()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card interview = PlayCard("ExclusiveInterview");

            StartGame();

            Card topcard = GetTopCardOfDeck(baron);

            DecisionYesNo = true;

            GoToEndOfTurn(clara);

            AssertInTrash(topcard);
        }

        [Test()]
        public void ExclusiveInterviewNoDiscard()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card interview = PlayCard("ExclusiveInterview");

            StartGame();

            Card topcard = GetTopCardOfDeck(baron);

            DecisionYesNo = false;

            GoToEndOfTurn(clara);

            AssertOnTopOfDeck(topcard);
        }
    }
}