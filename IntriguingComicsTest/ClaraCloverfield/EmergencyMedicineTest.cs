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
    public class EmergencyMedicineTest : BaseTest
    {
        protected TurnTakerController clara { get { return FindEnvironment(); } }

        [Test()]

        public void EmergencyMedicineClaraIsHealedIfYes()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card medicine = PlayCard("EmergencyMedicine");

            DealDamage(voss, legacy, 3, DamageType.Melee);

            DealDamage(voss, clarac, 5, DamageType.Fire);

            QuickHPStorage(clarac);

            DecisionYesNo = true;

            GainHP(legacy, 2);

            QuickHPCheck(2);
        }

        [Test()]
        public void EmergencyMedicineHeroNotHealedIfYes()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card medicine = PlayCard("EmergencyMedicine");

            DealDamage(voss, legacy, 3, DamageType.Melee);

            DealDamage(voss, clarac, 5, DamageType.Fire);

            QuickHPStorage(legacy);

            DecisionYesNo = true;

            GainHP(legacy, 2);

            QuickHPCheck(0);
        }

        [Test()]
        public void EmergencyMedicineClaraNotHealedIfNo()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card medicine = PlayCard("EmergencyMedicine");

            DealDamage(voss, legacy, 3, DamageType.Melee);

            DealDamage(voss, clarac, 5, DamageType.Fire);

            QuickHPStorage(clarac);

            DecisionYesNo = false;

            GainHP(legacy, 2);

            QuickHPCheck(0);
        }

        [Test()]
        public void EmergencyMedicineHeroIsHealedIfNo()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card medicine = PlayCard("EmergencyMedicine");

            DealDamage(voss, legacy, 3, DamageType.Melee);

            DealDamage(voss, clarac, 5, DamageType.Fire);

            QuickHPStorage(legacy);

            DecisionYesNo = false;

            GainHP(legacy, 2);

            QuickHPCheck(2);
        }

        [Test()]
        public void EmergencyMedicineClaraIsHealedEvenIfHeroWasAtMaxHP()
        {
            SetupGameController("GrandWarlordVoss", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card medicine = PlayCard("EmergencyMedicine");

            DealDamage(voss, clarac, 5, DamageType.Fire);

            QuickHPStorage(clarac);

            DecisionYesNo = true;

            GainHP(legacy, 2);

            QuickHPCheck(2);
        }
    }
}