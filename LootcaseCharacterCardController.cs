using System;
using System.Collections;
using System.Linq;
using System.Globalization;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class LootcaseCharacterCardController : VillainCharacterCardController
    {
        public LootcaseCharacterCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowSpecialString(() => $"The total value of {CharacterCard.Title}'s stolen loot is ${LootValue.ToString("#,#0", CultureInfo.InvariantCulture)}.", () => true);
            base.SpecialStringMaker.ShowIfElseSpecialString(() => base.Game.Journal.DealDamageEntriesThisTurn().Where((DealDamageJournalEntry entry) => entry.TargetCard == base.CharacterCard).Any(), () => "Lootcase has been dealt damage this turn.", () => "Lootcase has not been dealt damage this turn.").Condition = () => !Card.IsFlipped;
        }

        //Calculation of loot value for the string.

        private int NumberOf100000DollarCasesInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => AskIfCardContainsKeyword(c, "case") && AskIfCardContainsKeyword(c, "$100,000") && c.IsInPlayAndHasGameText).Count();
            }
        }
        private int NumberOf200000DollarCasesInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => AskIfCardContainsKeyword(c, "case") && AskIfCardContainsKeyword(c, "$200,000") && c.IsInPlayAndHasGameText).Count();
            }
        }
        private int NumberOf300000DollarCasesInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => AskIfCardContainsKeyword(c, "case") && AskIfCardContainsKeyword(c, "$300,000") && c.IsInPlayAndHasGameText).Count();
            }
        }
        private int NumberOf400000DollarCasesInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => AskIfCardContainsKeyword(c, "case") && AskIfCardContainsKeyword(c, "$400,000") && c.IsInPlayAndHasGameText).Count();
            }
        }
        private int NumberOf500000DollarCasesInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => AskIfCardContainsKeyword(c, "case") && AskIfCardContainsKeyword(c, "$500,000") && c.IsInPlayAndHasGameText).Count();
            }
        }
        private int NumberOf600000DollarCasesInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => AskIfCardContainsKeyword(c, "case") && AskIfCardContainsKeyword(c, "$600,000") && c.IsInPlayAndHasGameText).Count();

            }
        }
        private int NumberOf700000DollarCasesInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => AskIfCardContainsKeyword(c, "case") && AskIfCardContainsKeyword(c, "$700,000") && c.IsInPlayAndHasGameText).Count();
            }
        }
        private int NumberOf800000DollarCasesInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => AskIfCardContainsKeyword(c, "case") && AskIfCardContainsKeyword(c, "$800,000") && c.IsInPlayAndHasGameText).Count();
            }
        }
        private int NumberOf900000DollarCasesInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => AskIfCardContainsKeyword(c, "case") && AskIfCardContainsKeyword(c, "$900,000") && c.IsInPlayAndHasGameText).Count();
            }
        }
        private int NumberOf1000000DollarCasesInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => AskIfCardContainsKeyword(c, "case") && AskIfCardContainsKeyword(c, "$1,000,000") && c.IsInPlayAndHasGameText).Count();
            }
        }
        private int NumberOf1DollarCasesInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => AskIfCardContainsKeyword(c, "case") && AskIfCardContainsKeyword(c, "$1") && c.IsInPlayAndHasGameText).Count();
            }
        }
        private int NumberOfForgedAppraisalInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => c.Identifier == "ForgedAppraisal" && c.IsInPlayAndHasGameText && c.NativeDeck == Card.NativeDeck).Count();
            }
        }
        

        private Card Yoink1 => base.GameController.FindCardsWhere((Card c) => c.Identifier == "Yoink" && c.NativeDeck == Card.NativeDeck).FirstOrDefault();

        private Card Yoink2 => base.GameController.FindCardsWhere((Card c) => c.Identifier == "Yoink" && c != Yoink1 && c.NativeDeck == Card.NativeDeck).FirstOrDefault();

        private Location UnderYoink1 => Yoink1.UnderLocation;

        private Location UnderYoink2 => Yoink2.UnderLocation;

        private int NumberOfCardsUnderYoink1
        {
            get
            {
                return base.FindCardsWhere((Card c) => c.Location == UnderYoink1).Count();
            }
        }

        private int NumberOfCardsUnderYoink2
        {
            get
            {
                return base.FindCardsWhere((Card c) => c.Location == UnderYoink2).Count();
            }
        }


        private int NumberOfCardsUnderYoink
        {
            get
            {
                return NumberOfCardsUnderYoink1 + NumberOfCardsUnderYoink2;
            }
        }


        private int LootValue
        {

            get
            {
                return (                    
                    ((NumberOfForgedAppraisalInPlay + 1) * 
                    ((NumberOf100000DollarCasesInPlay * 100000) + (NumberOf200000DollarCasesInPlay * 200000) + (NumberOf300000DollarCasesInPlay * 300000) + (NumberOf400000DollarCasesInPlay * 400000) + (NumberOf500000DollarCasesInPlay * 500000) + (NumberOf600000DollarCasesInPlay * 600000) + (NumberOf700000DollarCasesInPlay * 700000) + (NumberOf800000DollarCasesInPlay * 800000) + (NumberOf900000DollarCasesInPlay * 900000) + (NumberOf1000000DollarCasesInPlay * 1000000) + NumberOf1DollarCasesInPlay))
                    + (NumberOfCardsUnderYoink * 50000)
                    );
            }
        }
        
        private const string FirstTimeDamageDealt = "FirstTimeDamageDealt";

        public override void AddSideTriggers()
        {
            //Front Side: Cunning Cat Burglar
            if (!Card.IsFlipped)
            {
                //The first time Lootcase would be dealt damage each turn, prevent that damage.
                base.AddSideTrigger(base.AddTrigger<DealDamageAction>((DealDamageAction action) => !base.HasBeenSetToTrueThisTurn(FirstTimeDamageDealt) && action.Target.IsVillainCharacterCard, (DealDamageAction action) => this.PreventDamageResponse(action), TriggerType.CancelAction, TriggerTiming.Before, isActionOptional: false));

                //At the start of the Villain Turn, if the total value of all Case cards in play is more than $1 500 000, flip Lootcase's character cards.
                base.AddSideTrigger(base.AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker && LootValue >= 1500001, base.FlipThisCharacterCardResponse, TriggerType.FlipCard));

                //At the end of the Villain turn, play the top card of the villain deck.
                base.AddSideTrigger(base.AddEndOfTurnTrigger((TurnTaker turnTaker) => turnTaker == base.TurnTaker, base.PlayTheTopCardOfTheVillainDeckWithMessageResponse, TriggerType.PlayCard));

                //Advanced - At the start of the game, one "Yoink!" is put into play.  The villain deck is shuffled.
                if (this.IsGameAdvanced)
                {
                    //Dealt with on Turn Taker
                }
                //Challenge - At the start of the Villain turn, if the total value of all Case cards in play is over $3 000 000, Lootcase wins.
                if (this.IsGameChallenge)
                {
                    //Effect on flipped side.
                }
            }
            //Flipped Side: Gentleman Thief
            else
            {
                //Reduce damage dealt to Case cards by 1.
                base.AddSideTrigger(base.AddReduceDamageTrigger((Card c) => c.DoKeywordsContain("case"), 1));

                //Increase damage dealt by Villain targets by 1.
                base.AddSideTrigger(base.AddIncreaseDamageTrigger((DealDamageAction action) => action.DamageSource.Card != null && IsVillainTarget(action.DamageSource.Card), 1));

                //At the start of the Villain turn, if the total value of all Case cards in play is equal to or less than $1 500 000, flip Lootcase's character cards.
                base.AddSideTrigger(base.AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker && LootValue <= 1500000, base.FlipThisCharacterCardResponse, TriggerType.FlipCard));

                //At the start of the Villain turn, if the total value of all Case cards in play is more than $3 500 000, Lootcase wins.
                base.AddSideTrigger(base.AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, this.MaybeGameOverResponse, TriggerType.GameOver));

                //At the end of the Villain turn, play the top card of the villain deck.
                base.AddSideTrigger(base.AddEndOfTurnTrigger((TurnTaker turnTaker) => turnTaker == base.TurnTaker, base.PlayTheTopCardOfTheVillainDeckWithMessageResponse, TriggerType.PlayCard));

                //Advanced - Whenever a case card is destroyed, Lootcase deals each hero target 1 projectile damage.
                if (this.IsGameAdvanced)
                {
                    base.AddSideTrigger(base.AddTrigger<DestroyCardAction>((DestroyCardAction action) => action.WasCardDestroyed && action.CardToDestroy.Card.DoKeywordsContain("case"), this.DestroyCaseResponse, new TriggerType[] { TriggerType.DealDamage }, TriggerTiming.After));
                }
                //Challenge - At the start of the Villain turn, if the total value of all Case cards in play is over $3 000 000, Lootcase wins.
                if (this.IsGameChallenge)
                {
                    base.AddSideTrigger(base.AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, this.AdvancedMaybeGameOverResponse, TriggerType.GameOver));
                }
            }
            //Both sides

            AddDefeatedIfDestroyedTriggers();
        }


        //The first time Lootcase would be dealt damage each turn, prevent that damage.
        private IEnumerator PreventDamageResponse(DealDamageAction action)
        {
        base.SetCardPropertyToTrueIfRealAction(FirstTimeDamageDealt);
        IEnumerator coroutine = base.CancelAction(action, isPreventEffect: true);
        if (base.UseUnityCoroutines)
        {
            yield return base.GameController.StartCoroutine(coroutine);
        }
        else
        {
            base.GameController.ExhaustCoroutine(coroutine);
        }
        yield break;
        }


        //...Lootcase deals each hero target 1 projectile damage.
        private IEnumerator DestroyCaseResponse(DestroyCardAction action)
        {
            IEnumerator coroutine;
            coroutine = base.DealDamage(base.Card, (Card c) => IsHeroTarget(c), 1, DamageType.Projectile);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }

            yield break;
        }

        //At the start of the Villain turn, if the total value of all Case cards in play is more than $3 500 000, Lootcase wins.
        private IEnumerator MaybeGameOverResponse(PhaseChangeAction action)
        {
            if (LootValue >= 3500001)
            {
                IEnumerator coroutine = base.GameController.GameOver(EndingResult.AlternateDefeat, "Lootcase has skipped town with his ill-gotten gains!", true, cardSource: base.GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }
            yield break;
        }

        //Advanced - At the start of the Villain turn, if the total value of all Case cards in play is more than $3 000 000, Lootcase wins.
        private IEnumerator AdvancedMaybeGameOverResponse(PhaseChangeAction action)
        {
            if (LootValue >= 3000001)
            {
                IEnumerator coroutine = base.GameController.GameOver(EndingResult.AlternateDefeat, "Lootcase has skipped town with his in ill-gotten gains!", true, cardSource: base.GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }
            yield break;
        }

    }
}