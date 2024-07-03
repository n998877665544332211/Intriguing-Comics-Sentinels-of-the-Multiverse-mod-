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
    public class ExposeTest : BaseTest
    {
        protected TurnTakerController clara { get { return FindEnvironment(); } }

        [Test()]

        public void ExposeDestroysVillainTargetLowestHPAtEndOfTurn()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card battalion = PlayCard("BladeBattalion");

            Card expose = PlayCard("Expose");

            StartGame();

            DecisionSelectCard = battalion;

            GoToStartOfTurn(clara);

            AssertNotInPlay(battalion);
        }

        [Test()]
        public void ExposeDoesNotDestroyVillainTargetWithOver10HPAtEndOfTurn()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            StartGame();

            DestroyNonCharacterVillainCards();

            Card conqueror = PlayCard("TcfConqueror");

            Card expose = PlayCard("Expose");

            DecisionSelectCard = conqueror;

            GoToStartOfTurn(clara);

            AssertIsInPlay(conqueror);
        }

        [Test()]
        public void ExposeDestroysVillainOngoingAtStartOfTurn()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card backlash = PlayCard("BacklashField");

            Card expose = PlayCard("Expose");

            StartGame();

            DecisionSelectCard = backlash;

            GoToEndOfTurn(clara);

            AssertNotInPlay(backlash);
        }
    }
}