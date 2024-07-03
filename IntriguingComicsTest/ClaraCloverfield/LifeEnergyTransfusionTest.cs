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
    public class LifeEnergyTransfusionTest : BaseTest
    {
        protected TurnTakerController clara { get { return FindEnvironment(); } }

        [Test()]

        public void LifeEnergyTransfusionClaraRegainsHP()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            DealDamage(voss, clarac, 10, DamageType.Melee);

            QuickHPStorage(clarac);

            DecisionYesNo = true;

            Card transfusion = PlayCard("LifeEnergyTransfusion");

            QuickHPCheck(3);

            AssertHitPoints(legacy, 31);
            AssertHitPoints(ra, 29);
            AssertHitPoints(tempest, 25);
        }

        [Test()]
        public void LifeEnergyTransfusionClaraDoesNotRegainHPWhenNoDamageDealt()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card fortitude = PlayCard("Fortitude");

            DealDamage(voss, clarac, 10, DamageType.Melee);

            QuickHPStorage(clarac);

            DecisionYesNo = true;

            Card transfusion = PlayCard("LifeEnergyTransfusion");

            QuickHPCheck(2);

            AssertHitPoints(legacy, 32);
            AssertHitPoints(ra, 29);
            AssertHitPoints(tempest, 25);
        }

        [Test()]
        public void LifeEnergyTransfusionClaraRegainsExtraIfExtraDamageDealt()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card strength = PlayCard("SurgeOfStrength");

            DealDamage(voss, clarac, 10, DamageType.Melee);

            QuickHPStorage(clarac);

            DecisionYesNo = true;

            Card transfusion = PlayCard("LifeEnergyTransfusion");

            QuickHPCheck(4);

            AssertHitPoints(legacy, 30);
            AssertHitPoints(ra, 29);
            AssertHitPoints(tempest, 25);
        }

        [Test()]
        public void LifeEnergyTransfusionClaraRegainsEvenIfRedirected()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "MrFixer", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card mantis = PlayCard("DrivingMantis");

            DealDamage(voss, clarac, 10, DamageType.Melee);

            QuickHPStorage(clarac);

            DecisionYesNo = true;

            DecisionRedirectTarget = voss.CharacterCard;

            Card transfusion = PlayCard("LifeEnergyTransfusion");

            QuickHPCheck(3);

            AssertHitPoints(fixer, 28);
            AssertHitPoints(voss, 89);
            AssertHitPoints(tempest, 25);
            AssertHitPoints(legacy, 31);
        }

    }
}