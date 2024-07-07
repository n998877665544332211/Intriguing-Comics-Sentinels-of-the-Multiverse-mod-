using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class LootcaseCardController : CardController
    {
        public LootcaseCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            IEnumerator coroutine3 = base.GameController.SendMessageAction("Lootcase searches the villain deck for a copy of Get Them! to put into play.", Priority.Low, GetCardSource(), null, showCardSource: true);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine3);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine3);
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
                IEnumerator coroutine2 = base.GameController.SendMessageAction("There are no copies of Get Them! in the villain deck.", Priority.Low, GetCardSource(), null, showCardSource: true);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine2);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine2);
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
            AddTrigger((DealDamageAction dd) => dd.DamageSource.Card != null && Ishench(dd.DamageSource.Card) && dd.DidDealDamage && dd.Target.IsHeroCharacterCard, StealCardResponse, TriggerType.MoveCard, TriggerTiming.After, ActionDescription.DamageTaken);
            AddTrigger((DestroyCardAction destroyCard) => destroyCard.CardToDestroy.Card == base.Card, MoveOrDestroyThisCardResponse, TriggerType.Other, TriggerTiming.Before);
            AddTrigger((MoveCardAction moveCard) => moveCard.CardToMove == base.Card, MoveOrDestroyThisCardResponse, TriggerType.Other, TriggerTiming.Before);
        }

        private IEnumerator StealCardResponse(DealDamageAction dd)
        {
            TurnTaker owner = dd.Target.Owner;
            if (!owner.IsHero)
            {
                yield break;
            }
            List<Card> matchingCards = new List<Card>();
            List<Card> nonMatchingCards = new List<Card>();
            new List<Card>();
            List<RevealCardsAction> storedResultsAction = new List<RevealCardsAction>();
            IEnumerator e4 = base.GameController.RevealCards(base.TurnTakerController, owner.Deck, (Card c) => IsEquipment(c), 1, storedResultsAction, RevealedCardDisplay.None, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(e4);
            }
            else
            {
                base.GameController.ExhaustCoroutine(e4);
            }
            if (storedResultsAction.FirstOrDefault() != null)
            {
                matchingCards.AddRange(storedResultsAction.FirstOrDefault().MatchingCards);
                nonMatchingCards.AddRange(storedResultsAction.FirstOrDefault().NonMatchingCards);
            }
            Card StolenCard = matchingCards.FirstOrDefault();
            if (matchingCards.Count() > 0)
            {
                IEnumerator coroutine3 = base.GameController.SendMessageAction(base.Card.Title + " steals " + StolenCard.Title + " from " + owner.Name + "'s deck and stashes it beneath himself.", Priority.Low, GetCardSource(), null, showCardSource: true);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine3);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine3);
                }
                GameController gameController = base.GameController;
                TurnTakerController turnTakerController = base.TurnTakerController;
                Location underLocation = base.Card.UnderLocation;
                CardSource cardSource = GetCardSource();
                IEnumerator coroutine4 = gameController.MoveCard(turnTakerController, StolenCard, underLocation, toBottom: false, isPutIntoPlay: false, playCardIfMovingToPlayArea: true, null, showMessage: false, null, null, null, evenIfIndestructible: false, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, cardSource);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine4);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine4);
                }
            }
            IEnumerator coroutine5 = base.GameController.BulkMoveCards(base.TurnTakerController, nonMatchingCards, owner.Deck, toBottom: false, performBeforeDestroyActions: true, null, isDiscard: false, GetCardSource());
            IEnumerator e5 = base.GameController.ShuffleLocation(owner.Deck, null, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine5);
                yield return base.GameController.StartCoroutine(e5);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine5);
                base.GameController.ExhaustCoroutine(e5);
            }
        }

        private IEnumerator MoveOrDestroyThisCardResponse(GameAction gameAction)
        {
            while (base.Card.UnderLocation.Cards.Count() > 0)
            {
                Card topCard = base.Card.UnderLocation.TopCard;
                if (topCard.Owner.IsHero)
                {
                    IEnumerator coroutine = base.GameController.MoveCard(base.TurnTakerController, topCard, topCard.Owner.ToHero().Deck, toBottom: true, isPutIntoPlay: false, playCardIfMovingToPlayArea: true, null, showMessage: false, null, null, null, evenIfIndestructible: false, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, GetCardSource());
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

        private bool Ishench(Card c)
        {
            return c.DoKeywordsContain("hench");
        }
    }
}
