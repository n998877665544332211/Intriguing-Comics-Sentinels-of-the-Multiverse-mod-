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
    public class AsignedOutfitTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void ReducesFrankDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Megalopolis");


            Card outfit = PlayCard("AssignedOutfit");

            QuickHPStorage(frank.CharacterCard);

            DealDamage(legacy, frank, 2, DamageType.Fire);

            QuickHPCheck(-1);
        }

        [Test()]
        public void ReducesRestOfGangDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Megalopolis");

            Card outfit = PlayCard("AssignedOutfit");

            Card gang = PlayCard("TheRestOfTheGang");

            QuickHPStorage(gang);

            DealDamage(legacy, gang, 2, DamageType.Fire);

            QuickHPCheck(-1);
        }

        [Test()]
        public void DoesNotReduceDeviceDamage()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Megalopolis");

            Card outfit = PlayCard("AssignedOutfit");

            QuickHPStorage(outfit);

            DealDamage(legacy, outfit, 2, DamageType.Fire);

            QuickHPCheck(-2);
        }
    }
}