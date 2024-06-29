using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;


namespace IntriguingComics.Spite
{
    public class StalkingTheStreetsSpiteCharacterCardController : VillainCharacterCardController
    {

        private List<ITrigger> _triggersChanged;

        private Dictionary<ITrigger, Func<PhaseChangeAction, bool>> _additions;

        private Dictionary<ITrigger, Func<PhaseChangeAction, bool>> _alternatives;

        public override bool DoesHaveActivePlayMethod => false;


        public StalkingTheStreetsSpiteCharacterCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            _additions = new Dictionary<ITrigger, Func<PhaseChangeAction, bool>>();
            _alternatives = new Dictionary<ITrigger, Func<PhaseChangeAction, bool>>();
            base.SpecialStringMaker.ShowHeroTargetWithLowestHP().Condition = () => !base.Card.IsFlipped;
            base.SpecialStringMaker.ShowNumberOfCardsInPlay(new LinqCardCriteria((Card c) => c.IsDrug, "drug")).Condition = () => !base.Card.IsFlipped;
            base.SpecialStringMaker.ShowNumberOfCardsAtLocations(() => new Location[1]
                    {
            base.TurnTaker.Trash
                    }, new LinqCardCriteria((Card c) => c.IsVictim, "victim"), () => true).Condition = () => !base.Card.IsFlipped;
            base.SpecialStringMaker.ShowNumberOfCardsUnderCard(SafeHouse, () => true).Condition = () => !base.Card.IsFlipped;
            base.SpecialStringMaker.ShowNumberOfCardsInPlay(new LinqCardCriteria((Card c) => c.IsVictim, "victim")).Condition = () => !base.Card.IsFlipped && base.IsGameAdvanced;
        }

        private Card SafeHouse => base.TurnTaker.GetCardByIdentifier("SafeHouse");
        private Location UnderTheSafeHouse => SafeHouse.UnderLocation;

        private int NumberOfCardsUnderSafeHouse
        {
            get
            {
                return base.FindCardsWhere((Card c) => c.Location == UnderTheSafeHouse).Count();
            }
        }

        private int NumberOfVictimsInTrash
        {
            get
            {
                return base.FindCardsWhere((Card c) => c.DoKeywordsContain("victim") && c.Location.IsTrash && c.Location.IsVillain).Count();
            }
        }

        private int NumberOfVictimsInPlay
        {
            get
            {
                return base.FindCardsWhere((Card c) => c.DoKeywordsContain("victim") && c.IsInPlayAndHasGameText).Count();
            }
        }


        public override void AddTriggers()
        {
            //back side

            //When Spite flips to this side, all villain cards in play other than drugs and this card are destroyed.
            //Any drug card which would act at the end of the villain turn instead acts at the start of the villain turn.
            if (!AddedFlipTriggers)
            {
                AddedFlipTriggers = true;
                AddTrigger((FlipCardAction f) => f.CardToFlip == this, AfterFlipCardResponse, TriggerType.GainHP, TriggerTiming.After);
            }
            AddSideTriggers();
        }

        public override void AddSideTriggers()
        {
            //Front Side: Mutating Murderer
            if (!Card.IsFlipped)
            {
                //When Spite deals damage, he regains that much HP.
                AddSideTrigger(AddTrigger((DealDamageAction dd) => !base.CharacterCardController.IsBeingDestroyed && dd.DamageSource.IsSameCard(base.CharacterCard) && dd.DidDealDamage, RegainHPDamageDealtResponse, TriggerType.GainHP, TriggerTiming.After));

                //At the start of the villain turn, if there are 5 or more villain drug cards in play, flip Spite's villain character cards.
                AddSideTrigger(AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, StartOfTurnCheckIfShouldFlipResponse, new TriggerType[1] { TriggerType.Hidden }));

                //At the end of the villain turn, the top card of the villain deck is played.
                //Then, Spite deals the hero target with the lowest HP H - 2 melee damage."
                AddSideTrigger(AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, EndOfTurnPlayAndDealDamageResponse, new TriggerType[2]
                {
                TriggerType.PlayCard,
                TriggerType.DealDamage
                }));

                //If there are ever 5 victims in the villain trash, Spite wins.
                AddSideTrigger(AddTrigger((DestroyCardAction d) => d.WasCardDestroyed && d.CardToDestroy.Card.IsVictim && NumberOfVictimsInTrash >= 4, (DestroyCardAction d) => this.MaybeDefeatResponse(d), TriggerType.GameOver, TriggerTiming.After));
                AddSideTrigger(AddTrigger((DiscardCardAction discard) => discard.CardToDiscard.IsVictim && NumberOfVictimsInTrash >= 4, (DiscardCardAction discard) => this.MaybeDefeatResponse(discard), TriggerType.GameOver, TriggerTiming.After));
                AddSideTrigger(AddTrigger((MoveCardAction moveCard) => moveCard.CardToMove.IsVictim && moveCard.Destination.IsTrash && NumberOfVictimsInTrash >= 4, (MoveCardAction move) => this.MaybeDefeatResponse(move), TriggerType.GameOver, TriggerTiming.After));

                //If there are ever 6 cards under the Safe House, the heroes win.
                AddSideTrigger(AddTrigger((MoveCardAction m) => m.CardToMove.IsVillain && !m.Destination.IsTrash && NumberOfCardsUnderSafeHouse >= 5, (MoveCardAction d) => this.MaybeVictoryResponse(d), TriggerType.GameOver, TriggerTiming.After));

                //Advanced - At the start of the villain turn, if there are 2 or more victim cards in play, destroy 1 Victim card.
                if (this.IsGameAdvanced)
                {
                    AddSideTrigger(base.AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, DestroyVictimResponse, TriggerType.DestroyCard));
                }
                if (this.IsGameChallenge)
                {
                    //Increase damage dealt by environment cards by 1.
                    AddSideTrigger(AddIncreaseDamageTrigger((DealDamageAction action) => action.DamageSource.IsEnvironmentCard, 1));
                }
            }
            else
            {
                //back side

                //When Spite flips to this side, all villain cards in play other than drugs and this card are destroyed.
                //Any drug card which would act at the end of the villain turn instead acts at the start of the villain turn.

                //See above.



                //Whenever a villain card would enter play, instead play the top card of the environment deck.
                AddSideTrigger(AddTrigger((PlayCardAction p) => p.CardToPlay.IsVillain, PlayEnvironmentInsteadOfPlayCardResponse, TriggerType.PlayCard, TriggerTiming.Before));
                AddSideTrigger(AddTrigger((ShuffleTrashIntoDeckAction s) => s.TurnTakerController == base.TurnTakerController && base.TurnTaker.Deck.Cards.Count() == 0 && s.NecessaryToPlayCard, PlayEnvironmentInsteadOfPlayCardResponse, TriggerType.PlayCard, TriggerTiming.Before));

                //Advanced - At the end of the Villain turn, play the top card of the environment deck.
                if (this.IsGameAdvanced)
                {
                    base.AddSideTrigger(base.AddEndOfTurnTrigger((TurnTaker turnTaker) => turnTaker == base.TurnTaker, base.PlayTheTopCardOfTheEnvironmentDeckWithMessageResponse, TriggerType.PlayCard));
                }
                if (this.IsGameChallenge)
                {
                    //Increase damage dealt by environment cards by 1.
                    AddSideTrigger(AddIncreaseDamageTrigger((DealDamageAction action) => action.DamageSource.IsEnvironmentCard, 1));
                }
            }
            AddDefeatedIfDestroyedTriggers();

        }

        private IEnumerator RegainHPDamageDealtResponse(DealDamageAction dd)
        {
            int amount = dd.Amount;
            if (amount > 0)
            {
                IEnumerator coroutine = base.GameController.GainHP(base.CharacterCard, amount, null, null, GetCardSource());
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


        private IEnumerator DestroyVictimResponse(PhaseChangeAction phaseChange)
        {
            //Advanced - At the start of the villain turn, if there are 2 or more victim cards in play, destroy 1 Victim card.
            if (NumberOfVictimsInPlay >= 2)
            {
            IEnumerator coroutine = GameController.SelectAndDestroyCards(null, new LinqCardCriteria((Card c) => c.IsVillain && c.IsVictim, "victim"), 1, cardSource: GetCardSource());
            if (UseUnityCoroutines)
            {
                yield return GameController.StartCoroutine(coroutine);
            }
            else
            {
                GameController.ExhaustCoroutine(coroutine);
            }
            }
            else
            {
            }
        }



        private IEnumerator StartOfTurnCheckIfShouldFlipResponse(PhaseChangeAction phaseChange)
        {
            if (FindCardsWhere((Card c) => c.IsInPlayAndHasGameText && c.IsDrug).Count() == 5)
            {
                IEnumerator coroutine = base.GameController.FlipCard(base.CharacterCardController, treatAsPlayed: false, treatAsPutIntoPlay: false, null, null, GetCardSource());
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
        private IEnumerator EndOfTurnPlayAndDealDamageResponse(PhaseChangeAction phaseChange)
        {
            IEnumerator coroutine = PlayTheTopCardOfTheVillainDeckWithMessageResponse(phaseChange);
            IEnumerator e2 = DealDamageToLowestHP(base.CharacterCard, 1, (Card c) => IsHeroTarget(c), (Card c) => base.H - 2, DamageType.Melee);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
                yield return base.GameController.StartCoroutine(e2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
                base.GameController.ExhaustCoroutine(e2);
            }
        }

        private IEnumerator MaybeVictoryResponse(MoveCardAction moveCard)
        {
            if (NumberOfCardsUnderSafeHouse >= 6)
            {
                IEnumerator coroutine = base.GameController.GameOver(EndingResult.AlternateVictory, "Spite's rampage has been thwarted.", true, cardSource: base.GetCardSource());
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

        private IEnumerator MaybeDefeatResponse(GameAction action)
        {
            if (NumberOfVictimsInTrash >= 5)
            {
                IEnumerator coroutine = base.GameController.GameOver(EndingResult.AlternateDefeat, "Spite has claimed too many innocent lives.", true, cardSource: base.GetCardSource());

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




        public override IEnumerator AfterFlipCardImmediateResponse()
        {
            RemoveSideTriggers();
            AddTriggers();
            yield return null;
        }


        private bool IsVillainNotDrugNotSpite(Card c)
        {
            return (c.IsVillain && !c.IsDrug && c != base.CharacterCard);
        }

        protected IEnumerator AfterFlipCardResponse(FlipCardAction flipCard)
        {
            IEnumerator coroutine = base.GameController.DestroyCards(DecisionMaker, new LinqCardCriteria((Card c) => IsVillainNotDrugNotSpite(c)), autoDecide: false, null, null, null, SelectionType.DestroyCard, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            AddLastTriggers();
            IEnumerator coroutine2 = ApplyChangesResponse(null);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine2);
            }

        }

        public override void AddLastTriggers()
        {
            if (Card.IsFlipped)
            {
                _triggersChanged = new List<ITrigger>();
            AddTrigger((PhaseChangeAction p) => IsVillain(p.ToPhase.TurnTaker) && base.GameController.IsTurnTakerVisibleToCardSource(p.ToPhase.TurnTaker, GetCardSource()), ApplyChangesResponse, TriggerType.HiddenLast, TriggerTiming.Before);
            }
            else
            {
            }
        }

        private IEnumerator ApplyChangesResponse(GameAction action)
        {
            foreach (TurnTakerController item in base.GameController.TurnTakerControllers.Where((TurnTakerController ttc) => IsVillain(ttc.TurnTaker) && base.GameController.IsTurnTakerVisibleToCardSource(ttc.TurnTaker, GetCardSource()) && base.BattleZone == ttc.BattleZone))
            {
                TurnPhase end = item.TurnTaker.TurnPhases.Where((TurnPhase phase) => phase.IsEnd).First();
                PhaseChangeAction endOfVillainTurn = new PhaseChangeAction(GetCardSource(), null, end, end.IsEphemeral);
                IEnumerable<ITrigger> changeMe = FindTriggersWhere((ITrigger t) => t is PhaseChangeTrigger && t.CardSource.Card.IsDrug && IsVillain(t.CardSource.Card.Owner) && t.DoesMatchTypeAndCriteria(endOfVillainTurn));
                changeMe.ForEach(delegate (ITrigger t)
                {
                    ApplyChangeTo((PhaseChangeTrigger)t, end);
                });
                if (base.Game.ActiveTurnPhase != null && base.Game.ActiveTurnPhase.IsEnd)
                {
                    base.GameController.AddTemporaryTriggerInhibitor((ITrigger t) => changeMe.Contains(t), (GameAction ga) => (ga is PhaseChangeAction && (ga as PhaseChangeAction).FromPhase.IsEnd) || !base.Card.IsInPlayAndHasGameText, GetCardSource());
                }
            }
            yield return null;
        }
        private void ApplyChangeTo(PhaseChangeTrigger trigger, TurnPhase oldPhase)
        {
            Func<PhaseChangeAction, bool> func = (PhaseChangeAction p) => p.ToPhase.IsStart && IsVillain(p.ToPhase.TurnTaker) && p.ToPhase.TurnTaker == oldPhase.TurnTaker;
            Func<PhaseChangeAction, bool> func2 = (PhaseChangeAction p) => !IsVillain(p.ToPhase.TurnTaker);
            trigger.AddAdditionalCriteria(func2);
            _additions.Add(trigger, func2);
            trigger.AddAlternativeCriteria(func);
            _alternatives.Add(trigger, func);
            _triggersChanged.Add(trigger);
        }

        private IEnumerator PlayEnvironmentInsteadOfPlayCardResponse(GameAction action)
        {
            string message = base.TurnTaker.Name + " would have played a villain card, but will instead play the top card of the environment deck.";
            IEnumerator coroutine = base.GameController.SendMessageAction(message, Priority.Low, GetCardSource());
            IEnumerator e2 = CancelAction(action, showOutput: false);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
                yield return base.GameController.StartCoroutine(e2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
                base.GameController.ExhaustCoroutine(e2);
            }
            IEnumerator coroutine2 = PlayTheTopCardOfTheEnvironmentDeckResponse(action);
            if (base.UseUnityCoroutines)
            {
            yield return base.GameController.StartCoroutine(coroutine2);
            }
            else
            {
            base.GameController.ExhaustCoroutine(coroutine2);
            }
        }
    }
}
