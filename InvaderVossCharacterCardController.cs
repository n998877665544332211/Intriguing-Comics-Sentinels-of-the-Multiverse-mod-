using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;


namespace IntriguingComics.GrandWarlordVoss
{
    public class InvaderVossCharacterCardController : VillainCharacterCardController
    {


        
        public InvaderVossCharacterCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.MakesIndestructible);
            base.SpecialStringMaker.ShowHeroTargetWithHighestHP(numberOfTargets: 2).Condition = () => base.Card.IsFlipped;
        }

        public override void AddSideTriggers()
        {
            //Front Side: Fleet Admiral
            if (!Card.IsFlipped)
            {
                AddSideTrigger(AddTrigger((DestroyCardAction d) => d.CardToDestroy.Card.Identifier == "ForcedDeployment", FlipThisCharacterCardResponse, TriggerType.MoveCard, TriggerTiming.After));
                if (!this.IsGameAdvanced)
                {
                    AddSideTrigger(AddIncreaseDamageTrigger((DealDamageAction dd) => IsVillainTarget(dd.Target) && dd.Target.Identifier != "TcfStalwart", 5 - base.Game.H));
                }
            }
            //Back Side: Invasion Leader
            else
            {
                SideTriggers.Add(AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, FlippedEndOfTurnResponse, TriggerType.DealDamage));
                AddSideTrigger(AddReduceDamageTrigger((DealDamageAction dd) => NumberOfMinionsInPlay > 0, null, (DealDamageAction dd) => NumberOfMinionsInPlay, (Card c) => c == base.Card));

                if (!this.IsGameAdvanced)
                {
                    AddSideTrigger(AddTrigger((DestroyCardAction d) => d.WasCardDestroyed && d.CardToDestroy.Card.IsMinion && d.WasCardDestroyed, (DestroyCardAction d) => DealDamage(base.CharacterCard, (Card c) => c == base.CharacterCard, 1, DamageType.Psychic, isIrreducible: true), TriggerType.DealDamage, TriggerTiming.After));
                }

            }
            AddDefeatedIfDestroyedTriggers();

        }

        private int NumberOfMinionsInPlay => FindCardsWhere((Card c) => IsMinion(c) && c.IsInPlay).Count();

        private IEnumerator FlippedEndOfTurnResponse(PhaseChangeAction phaseChange)
        {
            IEnumerator coroutine = DealDamageToHighestHP(base.Card, 1, (Card c) => IsHeroTarget(c), (Card c) => base.GameController.Game.H - 1, DamageType.Energy, numberOfTargets: () => 2);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
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
            coroutine = base.GameController.MakeTargettable(base.CharacterCard, 55, 55, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            IEnumerator coroutine2 = base.AfterFlipCardImmediateResponse();
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine2);
            }
            coroutine2 = base.GameController.DestroyAnyCardsThatShouldBeDestroyed(ignoreBattleZone: false, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine2);
            }

        }


              
        public override bool AskIfCardIsIndestructible(Card card)
        {
            if (!base.CharacterCard.IsFlipped && card.Identifier == "TcfStalwart")
            {
                return true;
            }
            if (base.IsGameChallenge && card.Identifier == "ForcedDeployment" && !base.GameController.ActiveTurnPhase.TurnTaker.IsVillain)
            {
                return true;
            }
            return false;
        }

    }
}