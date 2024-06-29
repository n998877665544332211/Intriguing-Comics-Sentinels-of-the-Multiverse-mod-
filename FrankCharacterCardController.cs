using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class FrankCharacterCardController : VillainCharacterCardController
    {
        public override bool CanBeDestroyed => NumberOfBossesInPlay == 0;

        private int NumberOfBossesInPlay => FindCardsWhere((Card c) => c.IsInPlayAndHasGameText && c.DoKeywordsContain("boss")).Count();

        public FrankCharacterCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.MakesIndestructible);
            base.SpecialStringMaker.ShowNumberOfCardsAtLocation(base.TurnTaker.Deck, new LinqCardCriteria((Card c) => IsOrder(c), "order")).Condition = () => !base.Card.IsFlipped;
            base.SpecialStringMaker.ShowNumberOfCardsAtLocation(base.TurnTaker.Deck, new LinqCardCriteria((Card c) => IsBoss(c), "boss")).Condition = () => !base.Card.IsFlipped;
            base.SpecialStringMaker.ShowNumberOfCardsInPlay(new LinqCardCriteria((Card c) => IsBoss(c), "boss"));
        }

        public override void AddSideTriggers()
        {
            if (!base.Card.IsFlipped)
            {
                AddSideTrigger(AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker && NumberOfBossesInPlay == 0, (PhaseChangeAction p) => RevealCards_MoveMatching_ReturnNonMatchingCards(base.TurnTakerController, base.TurnTaker.Deck, playMatchingCards: false, putMatchingCardsIntoPlay: true, moveMatchingCardsToHand: false, new LinqCardCriteria((Card c) => IsBoss(c), "boss"), 1, null, shuffleSourceAfterwards: true, showMessage: true), TriggerType.PlayCard));
                AddSideTrigger(AddTrigger((CardEntersPlayAction p) => IsBoss(p.CardEnteringPlay) && !base.Game.IsChallenge && NumberOfBossesInPlay >= 2, (CardEntersPlayAction p) => BossEntersPlayResponse(p), TriggerType.MoveCard, TriggerTiming.After));
                AddSideTrigger(AddTrigger((CardEntersPlayAction p) => IsBoss(p.CardEnteringPlay) && base.Game.IsChallenge && NumberOfBossesInPlay >= 3, (CardEntersPlayAction p) => BossEntersPlayChallengeResponse(p), TriggerType.MoveCard, TriggerTiming.After));
                if (base.IsGameAdvanced)
                {
                    AddSideTrigger(AddMakeDamageIrreducibleTrigger((DealDamageAction dd) => dd.DamageSource.Card == base.CharacterCard));
                }
            }
            else
            {
                AddSideTrigger(AddTrigger((MoveCardAction moveCard) => IsBoss(moveCard.CardToMove) && !moveCard.CardToMove.IsInPlayAndNotUnderCard && NumberOfBossesInPlay == 0, (MoveCardAction m) => HeroesWinResponse(m), new TriggerType[2]
                {
                TriggerType.GameOver,
                TriggerType.Hidden
                }, TriggerTiming.After));
                AddSideTrigger(AddTrigger((DestroyCardAction d) => IsBoss(d.CardToDestroy.Card) && d.CardToDestroy.CanBeDestroyed && d.WasCardDestroyed && NumberOfBossesInPlay == 1, HeroesWinResponse, new TriggerType[2]
                {
                TriggerType.GameOver,
                TriggerType.Hidden
                }, TriggerTiming.After));
                AddSideTrigger(AddTrigger((FlipCardAction flipCard) => NumberOfBossesInPlay == 0, (FlipCardAction m) => HeroesWinResponse(m), new TriggerType[2]
{
                TriggerType.GameOver,
                TriggerType.Hidden
}, TriggerTiming.After));
                SideTriggers.Add(AddCannotDealDamageTrigger((Card c) => c == base.CharacterCard && base.CharacterCard.IsFlipped));
                CannotPlayCards(null, (Card c) => c.IsVillain && base.CharacterCard.IsFlipped);
                SideTriggers.Add(AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, StartOfTurnFlipSResponse, TriggerType.FlipCard));
                if (base.IsGameAdvanced)
                {
                    AddSideTrigger(AddTrigger((DealDamageAction dd) => IsBoss(dd.Target) && dd.DidDealDamage && !dd.DidDestroyTarget && dd.DamageSource.Card != null && IsHeroTarget(dd.DamageSource.Card), (DealDamageAction dd) => DealDamage(dd.Target, dd.DamageSource.Card, 2, DamageType.Melee), TriggerType.DealDamage, TriggerTiming.After));
                }
            }
            AddDefeatedIfDestroyedTriggers();
        }

        private IEnumerator BossEntersPlayResponse(CardEntersPlayAction p)
        {
            LinqCardCriteria linqCardCriteria = new LinqCardCriteria((Card c) => IsBoss(c) && c.IsInPlayAndHasGameText && c != p.CardEnteringPlay);
            if (FindCardsWhere(linqCardCriteria.Criteria, realCardsOnly: true).Count() > 0)
            {
                SelectCardsDecision selectCardsDecision = new SelectCardsDecision(base.GameController, null, linqCardCriteria.Criteria, SelectionType.MoveCard, null, isOptional: false, null, eliminateOptions: true, allowAutoDecide: true, allAtOnce: false, null, null, null, null, GetCardSource());
                List<SelectCardDecision> storedResults2 = new List<SelectCardDecision>();
                IEnumerator coroutine = base.GameController.SelectCardsAndDoAction(selectCardsDecision, delegate (SelectCardDecision d)
                {
                    GameController gameController = base.GameController;
                    TurnTakerController turnTakerController = base.TurnTakerController;
                    Card selectedCard = d.SelectedCard;
                    Location trash = base.TurnTaker.Trash;
                    CardSource cardSource = GetCardSource();
                    return gameController.MoveCard(turnTakerController, selectedCard, trash, toBottom: false, isPutIntoPlay: false, playCardIfMovingToPlayArea: true, null, showMessage: false, null, null, null, evenIfIndestructible: true, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, cardSource);
                }, storedResults2, null, GetCardSource());
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

        private IEnumerator BossEntersPlayChallengeResponse(CardEntersPlayAction p)
        {
            int num = FindCardsWhere((Card c) => IsBoss(c) && c.IsInPlayAndHasGameText).Count();
            int num2 = FindCardsWhere((Card c) => IsBoss(c) && c.IsInPlayAndHasGameText && c != p.CardEnteringPlay).Count();
            if (num < 3)
            {
                yield break;
            }
            Card bossToMove;
            if (num2 == 1)
            {
                IEnumerable<Card> possibleBosses = base.GameController.FindCardsWhere((Card c) => IsBoss(c) && c.IsInPlayAndHasGameText && c != p.CardEnteringPlay);
                bossToMove = possibleBosses.FirstOrDefault();
            }
            else
            {
                List<Card> selectedCards = null;
                List<SelectCardsDecision> storedResults = new List<SelectCardsDecision>();
                IEnumerator coroutine = base.GameController.SelectCardsAndStoreResults(null, SelectionType.MoveCard, (Card c) => IsBoss(c) && c.IsInPlayAndHasGameText && c != p.CardEnteringPlay, 1, storedResults, optional: false, null, eliminateOptions: true, allowAutoDecide: false, allAtOnce: true, null, GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
                SelectCardsDecision selectCardsDecision = storedResults.FirstOrDefault();
                if (selectCardsDecision != null)
                {
                    selectedCards = new List<Card>(selectCardsDecision.SelectCardDecisions.Select((SelectCardDecision d) => d.SelectedCard));
                }
                bossToMove = selectedCards.FirstOrDefault();
            }
            GameController gameController = base.GameController;
            TurnTakerController turnTakerController = base.TurnTakerController;
            Card cardToMove = bossToMove;
            Location trash = base.TurnTaker.Trash;
            CardSource cardSource = GetCardSource();
            IEnumerator coroutine2 = gameController.MoveCard(turnTakerController, cardToMove, trash, toBottom: false, isPutIntoPlay: false, playCardIfMovingToPlayArea: true, null, showMessage: false, null, null, null, evenIfIndestructible: true, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, cardSource);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine2);
            }
        }

        private IEnumerator HeroesWinResponse(GameAction ga)
        {
            IEnumerator coroutine = base.GameController.GameOver(EndingResult.AlternateVictory, "Frank and his boss have both been defeated!", showEndingTextAsMessage: true, null, null, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
        }

        private IEnumerator StartOfTurnFlipSResponse(PhaseChangeAction phaseChange)
        {
            IEnumerator coroutine = base.GameController.FlipCard(this, treatAsPlayed: false, treatAsPutIntoPlay: false, null, null, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
        }

        public override IEnumerator DestroyAttempted(DestroyCardAction action)
        {
            if (!base.Card.IsFlipped)
            {
                IEnumerator coroutine = base.GameController.FlipCard(this, treatAsPlayed: false, treatAsPutIntoPlay: false, null, null, GetCardSource());
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

        public override IEnumerator AfterFlipCardImmediateResponse()
        {
            IEnumerator coroutine = base.AfterFlipCardImmediateResponse();
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            coroutine = (base.Card.IsFlipped ? base.GameController.RemoveTarget(base.CharacterCard, leavesPlayIfInPlay: true, GetCardSource()) : base.GameController.MakeTargettable(base.CharacterCard, 13, 13, GetCardSource()));
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            if (base.CharacterCard.IsFlipped)
            {
                IEnumerable<Card> ordersInPlay = base.GameController.FindCardsWhere(new LinqCardCriteria((Card c) => IsOrder(c) && !c.IsOneShot && c.IsInPlayAndHasGameText));
                if (ordersInPlay.Any())
                {
                    IEnumerator coroutine3 = base.GameController.MoveCards(base.TurnTakerController, ordersInPlay, base.TurnTaker.Deck, toBottom: false, isPutIntoPlay: false, playIfMovingToPlayArea: false, null, showIndividualMessages: false, isDiscard: false, null, GetCardSource());
                    if (base.UseUnityCoroutines)
                    {
                        yield return base.GameController.StartCoroutine(coroutine3);
                    }
                    else
                    {
                        base.GameController.ExhaustCoroutine(coroutine3);
                    }
                    IEnumerator coroutine4 = base.GameController.ShuffleLocation(base.TurnTaker.Deck);
                    if (base.UseUnityCoroutines)
                    {
                        yield return base.GameController.StartCoroutine(coroutine4);
                    }
                    else
                    {
                        base.GameController.ExhaustCoroutine(coroutine4);
                    }
                }
            }
            if (!base.CharacterCard.IsFlipped)
            {
                IEnumerator coroutine5 = RevealCards_MoveMatching_ReturnNonMatchingCards(base.TurnTakerController, base.TurnTaker.Deck, playMatchingCards: false, putMatchingCardsIntoPlay: true, moveMatchingCardsToHand: false, new LinqCardCriteria((Card c) => IsOrder(c), "order"), 1, null, shuffleSourceAfterwards: true, showMessage: true);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine5);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine5);
                }
                base.GameController.ShuffleLocation(base.TurnTaker.Deck);
            }
        }

        public override bool AskIfCardIsIndestructible(Card card)
        {
            if (IsBoss(card) && card.HitPoints.Value >= 10)
            {
                return true;
            }
            return false;
        }

        private bool IsBoss(Card c)
        {
            return c.DoKeywordsContain("boss");
        }

        private bool IsOrder(Card c)
        {
            return c.DoKeywordsContain("order");
        }
    }
}