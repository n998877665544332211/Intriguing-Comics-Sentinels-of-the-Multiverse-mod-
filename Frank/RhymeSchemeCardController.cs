using System.Collections;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class RhymeSchemeCardController : CardController
    {
        private int NumberOfOrderslInPlay => FindCardsWhere((Card c) => c.DoKeywordsContain("order") && c.IsInPlayAndHasGameText).Count();

        public RhymeSchemeCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.MakesIndestructible);
            base.SpecialStringMaker.ShowHeroTargetWithHighestHP(1, 2);
            base.SpecialStringMaker.ShowNumberOfCardsInPlay(new LinqCardCriteria((Card c) => c.DoKeywordsContain("order"), "order"));
        }

        public override bool AskIfCardIsIndestructible(Card card)
        {
            if (IsVillain(card) && FindCardsWhere((Card c) => c.NativeDeck == card.NativeDeck && c.Identifier == card.Identifier && c.IsInPlayAndHasGameText).Count() >= 2)
            {
                return true;
            }
            return false;
        }

        public override void AddTriggers()
        {
            AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, DealDamageResponse, TriggerType.DealDamage);
            AddAfterLeavesPlayAction((GameAction g) => base.GameController.DestroyAnyCardsThatShouldBeDestroyed(ignoreBattleZone: false, GetCardSource()), TriggerType.DestroyCard);
            AddTrigger((BulkMoveCardsAction bmc) => bmc.CardsToMove.Contains(base.Card) && (bmc.Destination.IsOutOfGame || !bmc.Destination.IsInPlayAndNotUnderCard), (BulkMoveCardsAction bmc) => base.GameController.DestroyAnyCardsThatShouldBeDestroyed(ignoreBattleZone: true, GetCardSource()), TriggerType.DestroyCard, TriggerTiming.After, ActionDescription.Unspecified, isConditional: false, requireActionSuccess: true, null, outOfPlayTrigger: true);
            AddTrigger((MoveCardAction mc) => mc.CardToMove == base.Card && (mc.Destination.Name == LocationName.OutOfGame || !mc.Destination.IsInPlayAndNotUnderCard), (MoveCardAction mc) => base.GameController.DestroyAnyCardsThatShouldBeDestroyed(ignoreBattleZone: true, GetCardSource()), TriggerType.DestroyCard, TriggerTiming.After, ActionDescription.Unspecified, isConditional: false, requireActionSuccess: true, null, outOfPlayTrigger: false, null, null, ignoreBattleZone: true);
        }

        private IEnumerator DealDamageResponse(PhaseChangeAction phaseChange)
        {
            IEnumerator coroutine = DealDamageToHighestHP(base.Card, 1, (Card c) => IsHeroTarget(c), (Card c) => 1 + NumberOfOrderslInPlay, DamageType.Sonic, isIrreducible: false, optional: false, null, () => 2);
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
}
