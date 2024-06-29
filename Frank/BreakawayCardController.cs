using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class BreakawayCardController : CardController
    {
        public BreakawayCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            IEnumerator coroutine2 = base.GameController.SetHP(base.Card, 40, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine2);
            }
            IEnumerator coroutine4 = base.GameController.SendMessageAction("Breakaway searches the villain deck for a copy of Get Them! to put into play.", Priority.Low, GetCardSource(), null, showCardSource: true);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine4);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine4);
            }
            IEnumerable<Card> GetThemInDeck = base.GameController.FindCardsWhere(new LinqCardCriteria((Card c) => c.Identifier == "GetThem" && c.Location.IsDeck && c.Location.IsVillain));
            if (GetThemInDeck.Any())
            {
                GameController gameController = base.GameController;
                TurnTakerController turnTakerController = base.TurnTakerController;
                Card cardToPlay = GetThemInDeck.FirstOrDefault();
                CardSource cardSource = GetCardSource();
                IEnumerator coroutine = gameController.PlayCard(turnTakerController, cardToPlay, isPutIntoPlay: true, null, optional: false, null, null, evenIfAlreadyInPlay: false, null, null, null, associateCardSource: false, fromBottom: false, canBeCancelled: true, cardSource);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }
            else
            {
                IEnumerator coroutine3 = base.GameController.SendMessageAction("There are no copies of Get Them! in the villain deck.", Priority.Low, GetCardSource(), null, showCardSource: true);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine3);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine3);
                }
            }
            IEnumerator shuffle = base.GameController.ShuffleLocation(base.TurnTaker.Deck, null, new CardSource(base.CharacterCardController));
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(shuffle);
            }
            else
            {
                base.GameController.ExhaustCoroutine(shuffle);
            }
        }

        public override void AddTriggers()
        {
            AddTrigger((DealDamageAction dd) => dd.DamageSource.Card != null && Ishench(dd.DamageSource.Card) && dd.DidDealDamage && dd.Target.IsHeroCharacterCard, RegainHPResponse, TriggerType.MoveCard, TriggerTiming.After, ActionDescription.DamageTaken);
            AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker && base.Card.HitPoints >= 50, GameOverResponse, TriggerType.GameOver);
        }

        private IEnumerator RegainHPResponse(DealDamageAction dd)
        {
            int amount = dd.Amount;
            if (amount > 0)
            {
                IEnumerator coroutine = base.GameController.GainHP(base.Card, base.H, null, null, GetCardSource());
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

        private IEnumerator GameOverResponse(PhaseChangeAction action)
        {
            IEnumerator coroutine = base.GameController.GameOver(EndingResult.AlternateDefeat, "While Frank distracted the heroes, Breakaway made the hand-off!", showEndingTextAsMessage: true, null, null, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
        }

        private bool Ishench(Card c)
        {
            return c.DoKeywordsContain("hench");
        }
    }
}
