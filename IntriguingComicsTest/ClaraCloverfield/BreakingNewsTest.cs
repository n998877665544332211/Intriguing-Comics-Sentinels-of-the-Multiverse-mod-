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
    public class BreakingNewsTest : BaseTest
    {
        protected TurnTakerController clara { get { return FindEnvironment(); } }

        [Test()]

        public void BreakingNewsLetsHeroWithLowestHPPutOngoingOrEquipmentIntoPlay()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card downpour = FindCard((Card c) => c.Identifier == "CleansingDownpour");

            DecisionSelectCard = downpour;

            Card breaking = PlayCard("BreakingNews");

            AssertIsInPlay(downpour);
        }

        [Test()]
        public void BreakingNewsSelfDestructs()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            StartGame();

            Card breaking = PlayCard("BreakingNews");

            GoToEndOfTurn(clara);

            AssertNotInPlay(breaking);

        }
    }
}