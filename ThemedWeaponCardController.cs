using System.Collections;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class ThemedWeaponCardController : CardController
    {
        public ThemedWeaponCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowSpecialString(() => "No boss has dealt damage since " + base.Card.Title + " entered play.", base.IsFirstOrOnlyCopyOfThisCardInPlay).Condition = () => !GetDamageTypeThathenchsDeal().HasValue;
            base.SpecialStringMaker.ShowSpecialString(() => "Damage by henches is changed to " + GetDamageTypeThathenchsDeal().ToString() + " damage.", base.IsFirstOrOnlyCopyOfThisCardInPlay).Condition = () => GetDamageTypeThathenchsDeal().HasValue;
        }

        public override void AddTriggers()
        {
            AddIncreaseDamageTrigger((DealDamageAction dealDamage) => dealDamage.DamageSource.Card != null && Ishench(dealDamage.DamageSource.Card), 1);
            AddTrigger((DealDamageAction dealDamage) => IsFirstOrOnlyCopyOfThisCardInPlay() && Ishench(dealDamage.DamageSource.Card) && GetDamageTypeThathenchsDeal().HasValue, (DealDamageAction dealDamage) => ChangeTypeResponse(dealDamage), TriggerType.ChangeDamageType, TriggerTiming.Before);
        }

        private bool Ishench(Card c)
        {
            return c.DoKeywordsContain("hench");
        }

        private bool IsBoss(Card c)
        {
            return c.DoKeywordsContain("boss");
        }

        private IEnumerator ChangeTypeResponse(DealDamageAction dealDamage)
        {
            DamageType damageTypeThathenchsDeal = GetDamageTypeThathenchsDeal().Value;
            if (dealDamage.DamageType != damageTypeThathenchsDeal)
            {
                IEnumerator coroutine = base.GameController.ChangeDamageType(dealDamage, damageTypeThathenchsDeal, GetCardSource());
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

        private DamageType? GetDamageTypeThathenchsDeal()
        {
            DealDamageJournalEntry dealDamageJournalEntry = base.GameController.Game.Journal.MostRecentDealDamageEntry((DealDamageJournalEntry e) => IsBoss(e.SourceCard) && e.Amount > 0);
            PlayCardJournalEntry playCardJournalEntry = base.GameController.Game.Journal.QueryJournalEntries((PlayCardJournalEntry e) => e.CardPlayed == base.Card).LastOrDefault();
            if (playCardJournalEntry != null && dealDamageJournalEntry != null)
            {
                int? entryIndex = base.GameController.Game.Journal.GetEntryIndex(dealDamageJournalEntry);
                int? entryIndex2 = base.GameController.Game.Journal.GetEntryIndex(playCardJournalEntry);
                if (entryIndex.HasValue && entryIndex2.HasValue && entryIndex.Value > entryIndex2.Value)
                {
                    return dealDamageJournalEntry.DamageType;
                }
            }
            return null;
        }
    }
}