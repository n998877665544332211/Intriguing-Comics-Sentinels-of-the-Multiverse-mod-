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
    public class HostageHoldingHenchmanTest : BaseTest
    {
        protected TurnTakerController clara { get { return FindEnvironment(); } }

        [Test()]

        public void HostageHoldingHenchman3DamageToClaraClara()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card henchman = PlayCard("HostageHoldingHenchman");

            QuickHPStorage(clarac);

            GoToEndOfTurn(clara);

            QuickHPCheck(-3);
        }

        [Test()]
        public void HostageHoldingHenchmanPreventsEnvironmentCardPlay()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card henchman = PlayCard("HostageHoldingHenchman");

            Card breaking = PlayCard("BreakingNews");

            AssertNotInPlay(breaking);
        }
    }
}