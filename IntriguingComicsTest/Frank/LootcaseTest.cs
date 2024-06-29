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
    public class LootcaseTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void LootcasePutsGetThemIntoPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card lootcase = PlayCard("Lootcase");

            AssertNumberOfCardsAtLocation(frank.TurnTaker.PlayArea, 1, (Card c) => c.Identifier == "GetThem");
        }



        [Test()]
        public void LootcaseStealsEquipment()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card lootcase = PlayCard("Lootcase");

            Card equipment = GetTopMatchingCardOfLocation(wraith.TurnTaker.Deck, (Card c) => IsEquipment(c));

            DealDamage(frank, wraith, 2, DamageType.Fire);

            AssertUnderCard(lootcase, equipment);
        }

        [Test()]
        public void LootcaseReturnsOtherCardsToDeck()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card lootcase = PlayCard("Lootcase");

            DealDamage(frank, wraith, 2, DamageType.Fire);

            AssertNumberOfCardsInDeck(wraith, 35);
        }

        [Test()]
        public void LootcaseDestroyedOtherCardsReturnedToBottomOfDeck()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card lootcase = PlayCard("Lootcase");

            Card equipment = GetTopMatchingCardOfLocation(wraith.TurnTaker.Deck, (Card c) => IsEquipment(c));

            DealDamage(frank, wraith, 2, DamageType.Fire);

            DealDamage(legacy, lootcase, 100, DamageType.Fire);

            AssertOnBottomOfLocation(equipment, wraith.TurnTaker.Deck);
        }

        [Test()]
        public void LootcaseMovedOtherCardsReturnedToBottomOfDeck()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card lootcase = PlayCard("Lootcase");

            Card equipment = GetTopMatchingCardOfLocation(wraith.TurnTaker.Deck, (Card c) => IsEquipment(c));

            DealDamage(frank, wraith, 2, DamageType.Fire);

            Card rhyme = PlayCard("RhymeScheme");

            AssertOnBottomOfLocation(equipment, wraith.TurnTaker.Deck);
        }


        [Test()]
        public void LootcaseDoesNotStealIfNoDamageDealt()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card lootcase = PlayCard("Lootcase");

            UsePower(wraith);

            DealDamage(frank, wraith, 2, DamageType.Fire);

            AssertNumberOfCardsInDeck(wraith, 36);
        }
    }
}
