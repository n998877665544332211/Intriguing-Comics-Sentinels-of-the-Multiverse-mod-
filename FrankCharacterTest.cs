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
    public class FrankCharacterTest : BaseTest
    {
        protected TurnTakerController frank { get { return FindVillain("Frank"); } }

        [Test()]
        public void FrankPutsBossIntoPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            StartGame();

            Card rhyme = PlayCard("RhymeScheme");

            DealDamage(legacy, rhyme, 100, DamageType.Melee);

            Card boss = GetTopMatchingCardOfLocation(frank.TurnTaker.Deck, (Card c) => c.DoKeywordsContain("boss"));

            GoToStartOfTurn(frank);

            AssertIsInPlay(boss);
        }

        [Test()]
        public void FrankDestroyedNoBossInPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card rhyme = PlayCard("RhymeScheme");

            StartGame();

            DealDamage(legacy, rhyme, 100, DamageType.Melee);

            DealDamage(legacy, frank, 15, DamageType.Melee);

            AssertGameOver(EndingResult.VillainDestroyedVictory);
        }

        [Test()]
        public void FrankFlipsInsteadOfDestroyedIfBossInPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card rhyme = PlayCard("RhymeScheme");

            StartGame();

            DealDamage(legacy, frank, 15, DamageType.Melee);

            AssertNotGameOver();
            AssertFlipped(frank);
        }

        [Test()]
        public void FrankFlipsMovesOrdersToDeck()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card rhyme = PlayCard("RhymeScheme");
            Card cover = PlayCard("CoverMyBack");

            StartGame();

            Card lootcase = PlayCard("Lootcase");

            DealDamage(legacy, frank, 15, DamageType.Melee);

            AssertInDeck(cover);
        }

        [Test()]
        public void FrankFlipsBack()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card rhyme = PlayCard("RhymeScheme");

            StartGame();

            DealDamage(legacy, frank, 15, DamageType.Melee);

            GoToStartOfTurn(frank);

            AssertNotFlipped(frank);
            AssertHitPoints(frank, 13);
        }

        [Test()]
        public void FrankFlipsBackPlaysOrder()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card rhyme = PlayCard("RhymeScheme");

            StartGame();

            DealDamage(legacy, frank, 15, DamageType.Melee);

            Card order = GetTopMatchingCardOfLocation(frank.TurnTaker.Deck, (Card c) => c.DoKeywordsContain("order"));

            GoToStartOfTurn(frank);

            AssertIsInPlay(order);
        }

        [Test()]
        public void BossDestroyedAlternateWin()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card rhyme = PlayCard("RhymeScheme");

            StartGame();

            DealDamage(legacy, frank, 15, DamageType.Melee);

            DealDamage(legacy, rhyme, 50, DamageType.Fire);

            AssertGameOver(EndingResult.AlternateVictory);
        }

        [Test()]
        public void BossMovedAlternateWin()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card rhyme = PlayCard("RhymeScheme");

            StartGame();

            DealDamage(legacy, frank, 15, DamageType.Melee);

            DealDamage(legacy, rhyme, 35, DamageType.Melee);

            MoveCard(frank, rhyme, frank.TurnTaker.Deck);

            AssertGameOver(EndingResult.AlternateVictory);
        }

        [Test()]
        public void BossFlipAlternateWin()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card rhyme = PlayCard("RhymeScheme");

            StartGame();

            DealDamage(legacy, frank, 15, DamageType.Melee);

            DealDamage(legacy, rhyme, 35, DamageType.Melee);

            FlipCard(rhyme);

            AssertGameOver(EndingResult.AlternateVictory);
        }

        [Test()]
        public void BossesWithMoreThan10HPAreIndestructible()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card lootcase = PlayCard("Lootcase");

            DestroyCard(lootcase);

            AssertIsInPlay(lootcase);
        }

        [Test()]
        public void BossesWithLessThan10HPAreNotIndestructible()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card lootcase = PlayCard("Lootcase");

            DealDamage(legacy, lootcase, 35, DamageType.Fire);

            DestroyCard(lootcase);

            AssertNotInPlay(lootcase);
        }

        [Test()]
        public void FrankCannotDealDamageWhenFlipped()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card lootcase = PlayCard("Lootcase");

            DealDamage(legacy, frank, 15, DamageType.Fire);

            QuickHPStorage(legacy);

            DealDamage(frank, legacy, 10, DamageType.Melee);

            QuickHPCheck(0);
        }

        [Test()]
        public void VillainCardsCannotBePlayedWhenFlipped()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card lootcase = PlayCard("Lootcase");

            DealDamage(legacy, frank, 15, DamageType.Fire);

            Card outfit = PlayCard("AssignedOutfit");

            AssertNotInPlay(outfit);
        }

        [Test()]
        public void BossEntersPlay()
        {
            SetupGameController("IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis");

            Card lootcase = PlayCard("Lootcase");

            Card rhyme = PlayCard("RhymeScheme");

            AssertInTrash(lootcase);
        }


        [Test()]
        public void FrankAdvancedDamageIrreducible()
        {
            SetupGameController(new string[] { "IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis" }, true);

            Card fortitude = PlayCard("Fortitude");

            QuickHPStorage(legacy);

            DealDamage(frank, legacy, 2, DamageType.Melee);

            QuickHPCheck(-2);
        }

        [Test()]
        public void AdvancedSecondBossEntersPlay()
        {
            SetupGameController(new string[] { "IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis" }, false, null, null, null, true);

            Card lootcase = PlayCard("Lootcase");

            Card rhyme = PlayCard("RhymeScheme");

            AssertIsInPlay(lootcase);
        }

        public void AdvancedThirdBossEntersPlay()
        {
            SetupGameController(new string[] { "IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis" }, false, null, null, null, true);

            Card lootcase = PlayCard("Lootcase");

            Card rhyme = PlayCard("RhymeScheme");

            DecisionSelectCard = rhyme;

            Card breakaway = PlayCard("Breakaway");

            AssertInTrash(rhyme);
            AssertIsInPlay(lootcase);
        }

        [Test()]
        public void AdvancedFlipBossDamage()
        {
            SetupGameController(new string[] { "IntriguingComics.Frank", "Legacy", "Ra", "TheWraith", "Megalopolis" }, true);

            Card rhyme = PlayCard("RhymeScheme");

            StartGame();

            DealDamage(legacy, frank, 15, DamageType.Melee);

            QuickHPStorage(legacy);

            DealDamage(legacy, rhyme, 10, DamageType.Fire);

            QuickHPCheck(-2);
        }
    }
}