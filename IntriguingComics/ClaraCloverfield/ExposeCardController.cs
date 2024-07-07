using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;


namespace IntriguingComics.ClaraCloverfield
{
    public class ExposeCardController : ScoopCardController
    {
        public ExposeCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {

            base.SpecialStringMaker.ShowVillainTargetWithLowestHP(numberOfTargets: 1);

        }

        public override void AddTriggers()
        {

            //At the end of the environment turn, destroy a villain ongoing card.
            base.AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, DestroyVillainOngoingCardResponse, TriggerType.DestroyCard);


            //At the start of the environment turn, destroys the villain target with the lowest HP, other than a character card, if it has 10 or fewer HP.
            base.AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, ExposeDestroyVillainTargetCardResponse, TriggerType.DestroyCard);

        }
        private IEnumerator DestroyVillainOngoingCardResponse(PhaseChangeAction phaseChange)
        {
            //At the end of the environment turn, destroy a villain ongoing card.
            IEnumerator coroutine = GameController.SelectAndDestroyCards(null, new LinqCardCriteria((Card c) => IsVillain(c) && IsOngoing(c), "villain ongoing"), 1, cardSource: GetCardSource());
            if (UseUnityCoroutines)
            {
                yield return GameController.StartCoroutine(coroutine);
            }
            else
            {
                GameController.ExhaustCoroutine(coroutine);
            }
        }

        //At the start of the environment turn, destroys the villain target with the lowest HP, other than a character card, if it has 10 or fewer HP.
        private IEnumerator ExposeDestroyVillainTargetCardResponse(PhaseChangeAction phaseChange)
        {                 
                    List<Card> storedVillain = new List<Card>();
                    IEnumerator coroutine2 = base.GameController.FindTargetWithLowestHitPoints(ranking: 1, additionalCriteria: (Card c) => !c.IsCharacter && IsVillainTarget(c) && c.IsInPlayAndHasGameText && c.HitPoints.Value <= 10, storedVillain, gameAction: null, dealDamageInfo: null, evenIfCannotDealDamage: true, optional: false, beforeDecisionAction: null, ignoreBattleZone: false, cardSource: null);
                    if (base.UseUnityCoroutines)
                    {
                        yield return base.GameController.StartCoroutine(coroutine2);
                    }
                    else
                    {
                        base.GameController.ExhaustCoroutine(coroutine2);
                    }
                if (DidFindCard(storedVillain))
                {
                        Card villain = storedVillain.First();

                    if (base.GameController.IsCardIndestructible(villain))
                    {
                        IEnumerator coroutine3 = base.GameController.SendMessageAction(Card.Title + " cannot destroy " + villain.Title + " because it is indestructible.", Priority.Low, GetCardSource(), associatedCards: null, showCardSource: true);
                        if (base.UseUnityCoroutines)
                        {
                            yield return base.GameController.StartCoroutine(coroutine3);
                        }
                        else
                        {
                            base.GameController.ExhaustCoroutine(coroutine3);
                        }

                    }

                    else
                    {
 
                    IEnumerator coroutine3 = base.GameController.DestroyCard(DecisionMaker, villain, optional: false, storedResults: null, Card.Title + " destroys " + villain.Title + "!", showOutput: null, actionSource: null, responsibleCard: null, overrideDestroyLocation: null, postDestroyAction: null, associatedCards: null, cardSource: GetCardSource());
                    if (base.UseUnityCoroutines)
                    {
                        yield return base.GameController.StartCoroutine(coroutine3);
                    }
                    else
                    {
                        base.GameController.ExhaustCoroutine(coroutine3);
                    }

                    }



                }
                else
                {
                    IEnumerator coroutine3 = base.GameController.SendMessageAction("There are no non-character card villain targets with 10 or fewer HP in play.", Priority.Low, GetCardSource(), null, showCardSource: true);
                    if (base.UseUnityCoroutines)
                    {
                        yield return base.GameController.StartCoroutine(coroutine3);
                    }
                    else
                    {
                        base.GameController.ExhaustCoroutine(coroutine3);
                    }
                }
                
            
        }
    }
}
