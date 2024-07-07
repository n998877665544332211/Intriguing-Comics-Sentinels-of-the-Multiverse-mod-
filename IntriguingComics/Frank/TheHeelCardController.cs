using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class TheHeelCardController : CardController
    {
        public TheHeelCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowNumberOfCardsAtLocation(base.TurnTaker.Deck, new LinqCardCriteria((Card c) => IsOrder(c), "order"));
            base.SpecialStringMaker.ShowNumberOfCardsAtLocation(base.TurnTaker.Deck, new LinqCardCriteria((Card c) => c.IsDevice, "device"));
            base.SpecialStringMaker.ShowNumberOfCardsAtLocation(base.TurnTaker.Deck, new LinqCardCriteria((Card c) => Ishench(c), "hench"));
            base.SpecialStringMaker.ShowHeroTargetWithLowestHP();
            base.SpecialStringMaker.ShowDamageDealt(new LinqCardCriteria((Card c) => c.IsHeroCharacterCard, "heroes", useCardsSuffix: false), null, null, showTotalAmountOfDamageDealt: false, () => true, null, thisRound: true, thisTurn: false, base.Card);
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, StartOfTurnResponse, TriggerType.PhaseChange);
            AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, EndOfTurnResponse, TriggerType.PhaseChange);
        }

        private IEnumerator EndOfTurnResponse(PhaseChangeAction phaseChange)
        {
            IEnumerator coroutine = RevealCards_MoveMatching_ReturnNonMatchingCards(base.TurnTakerController, base.TurnTaker.Deck, playMatchingCards: false, putMatchingCardsIntoPlay: true, moveMatchingCardsToHand: false, new LinqCardCriteria((Card c) => IsOrder(c) || Ishench(c) || c.IsDevice, "order, hench, or device"), null, 1, shuffleSourceAfterwards: true, showMessage: false, RevealedCardDisplay.Message);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
        }

        private IEnumerator StartOfTurnResponse(PhaseChangeAction phaseChange)
        {
            DealDamageJournalEntry dealDamageJournalEntry = (from d in EntriesWhenHeroesDealtDamageLastRound()
                                                             where d.Amount > 0
                                                             select d).LastOrDefault();
            if (dealDamageJournalEntry == null)
            {
                yield break;
            }
            int? entryIndex = base.GameController.Game.Journal.GetEntryIndex(dealDamageJournalEntry);
            PlayCardJournalEntry playCardJournalEntry = (from c in base.GameController.Game.Journal.PlayCardEntries()
                                                         where c.CardPlayed == base.Card
                                                         select c).LastOrDefault();
            if (entryIndex > base.GameController.Game.Journal.GetEntryIndex(playCardJournalEntry))
            {
                IEnumerator coroutine = DealDamageToLowestHP(base.Card, 1, (Card c) => IsHeroTarget(c), (Card c) => base.Game.H, DamageType.Projectile);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }
        }

        private bool IsOrder(Card c)
        {
            return c.DoKeywordsContain("order");
        }

        private bool Ishench(Card c)
        {
            return c.DoKeywordsContain("hench");
        }

        public IEnumerable<DealDamageJournalEntry> EntriesWhenHeroesDealtDamageLastRound()
        {
            return base.Journal.QueryJournalEntries((DealDamageJournalEntry e) => e.Round == base.Game.Round - 1 && e.SourceCard.IsHeroCharacterCard);
        }
    }
}
