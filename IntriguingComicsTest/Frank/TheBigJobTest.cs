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
    public class TheBigJobTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void TheBigJobPutsOrderGangAndDeviceIntoPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "Tempest", "Megalopolis");

            Card outfit = PutOnDeck("AssignedOutfit");
            Card follow = PutOnDeck("FollowMyLead");
            Card gang = PutOnDeck("TheRestOfTheGang");

            Card job = PlayCard("TheBigJob");

            AssertIsInPlay(outfit);
            AssertIsInPlay(follow);
            AssertIsInPlay(gang);
        }
    }
}
