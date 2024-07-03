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
    public class BrainwashedBylinerTest : BaseTest
    {
        protected TurnTakerController clara { get { return FindEnvironment(); } }

        [Test()]
        public void BrainwashedBylinerClaraDeals2DamageStartOfTurn()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card byliner = PlayCard("BrainwashedByliner");

            StartGame();

            QuickHPStorage(legacy);

            GoToStartOfTurn(clara);

            QuickHPCheck(-2);
        }

        [Test()]
        public void BrainwashedByLinerClaraDeals1DamageEndOfTurn()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card byliner = PlayCard("BrainwashedByliner");

            StartGame();

            GoToPlayCardPhase(clara);

            QuickHPStorage(legacy);

            GoToEndOfTurn(clara);

            QuickHPCheck(-1);
        }

        [Test()]
        public void BrainwashedBylinerClaraDoesNotSelfDamage()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card byliner = PlayCard("BrainwashedByliner");

            Card clarac = FindCard((Card c) => c.Identifier == "ClaraCloverfield");

            StartGame();

            QuickHPStorage(clarac);

            GoToEndOfTurn(clara);

            QuickHPCheck(0);
        }
    }
}