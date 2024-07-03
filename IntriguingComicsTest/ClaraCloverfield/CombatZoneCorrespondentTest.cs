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
    public class CombatZoneCorrespondentTest : BaseTest
    {
        protected TurnTakerController clara { get { return FindEnvironment(); } }

        [Test()]
        public void CombatZoneCorrespondentHeroTargetDeals4HPDamage1DamageToClara()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card correspondent = PlayCard("CombatZoneCorrespondent");

            QuickHPStorage(clarac);

            DealDamage(legacy, baron, 4, DamageType.Fire);

            QuickHPCheck(-1);
        }

        [Test()]
        public void CombatZoneCorrespondentVillainTargetDeals4HPDamage1DamageToClara()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card correspondent = PlayCard("CombatZoneCorrespondent");

            QuickHPStorage(clarac);

            DealDamage(baron, legacy, 4, DamageType.Fire);

            QuickHPCheck(-1);
        }

        [Test()]
        public void CombatZoneCorrespondentNoDamageToClaraIfLessThan4Damage()
        {
            SetupGameController("BaronBlade", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield");

            Card clarac = PlayCard("ClaraCloverfield");

            Card correspondent = PlayCard("CombatZoneCorrespondent");

            QuickHPStorage(clarac);

            DealDamage(baron, ra, 3, DamageType.Fire);

            QuickHPCheck(0);
        }

        [Test()]
        public void CombatZoneCorrespondentNoDamageToClaraIf4DamageDealtToIndestructibleTarget()
        {
            SetupGameController(new string[] { "TheMatriarch", "Legacy", "Ra", "Tempest", "IntriguingComics.ClaraCloverfield" }, false, null, null, null, true, null, null, null);

            Card clarac = PlayCard("ClaraCloverfield");

            Card fields = PlayCard("CarrionFields");

            Card correspondent = PlayCard("CombatZoneCorrespondent");

            QuickHPStorage(clarac);

            DealDamage(legacy, fields, 4, DamageType.Fire);

            QuickHPCheck(0);
        }
    }
}