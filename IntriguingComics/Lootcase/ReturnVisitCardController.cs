using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Lootcase
{
    public class ReturnVisitCardController : CardController
    {
        public ReturnVisitCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowNumberOfCardsAtLocation(base.TurnTaker.Trash, new LinqCardCriteria((Card c) => c.DoKeywordsContain("case"), "case"));
        }


        public override IEnumerator Play()
        {
            //Destroy 2 hero ongoing cards.


            IEnumerator coroutine = base.GameController.SelectAndDestroyCards(null, new LinqCardCriteria((Card c) => IsHero(c) && IsOngoing(c), "hero ongoing"), 2, cardSource: base.GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }



            //Lootcase steals the (H-2) case cards with the highest loot values from the villain trash.

            int message = 0;

            int cardsToRetrieve = base.Game.H - 2;



            if (cardsToRetrieve == 1)
            {
                IEnumerator coroutine2 = base.GameController.SendMessageAction("Lootcase steals the case card with the highest loot values from the villain trash.", Priority.Low, GetCardSource(), null, showCardSource: true);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine2);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine2);
                }
            }
            else
            {
                IEnumerator coroutine2 = base.GameController.SendMessageAction("Lootcase steals the " + cardsToRetrieve + " case cards with the highest loot values from the villain trash.", Priority.Low, GetCardSource(), null, showCardSource: true);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine2);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine2);
                }
            }




            IEnumerator coroutine3;
            for (int i = 1; i <= cardsToRetrieve; i++)
            {


                IEnumerable<Card> OneMillionDollarsTrash = base.GameController.FindCardsWhere(new LinqCardCriteria((Card c) => c.Identifier == "OneMillionDollars" && c.Location.IsTrash && c.Location.IsVillain));

                IEnumerable<Card> MasterpieceTrash = base.GameController.FindCardsWhere(new LinqCardCriteria((Card c) => c.Identifier == "Masterpiece" && c.Location.IsTrash && c.Location.IsVillain));

                IEnumerable<Card> PricelessAntiquesTrash = base.GameController.FindCardsWhere(new LinqCardCriteria((Card c) => c.Identifier == "PricelessAntiques" && c.Location.IsTrash && c.Location.IsVillain));

                IEnumerable<Card> FamilyHeirloomsTrash = base.GameController.FindCardsWhere(new LinqCardCriteria((Card c) => c.Identifier == "FamilyHeirlooms" && c.Location.IsTrash && c.Location.IsVillain));

                IEnumerable<Card> ShinyJewelsTrash = base.GameController.FindCardsWhere(new LinqCardCriteria((Card c) => c.Identifier == "ShinyJewels" && c.Location.IsTrash && c.Location.IsVillain));

                IEnumerable<Card> GoldBarsTrash = base.GameController.FindCardsWhere(new LinqCardCriteria((Card c) => c.Identifier == "GoldBars" && c.Location.IsTrash && c.Location.IsVillain));

                IEnumerable<Card> CollectorsItemTrash = base.GameController.FindCardsWhere(new LinqCardCriteria((Card c) => c.Identifier == "CollectorsItem" && c.Location.IsTrash && c.Location.IsVillain));

                IEnumerable<Card> BelovedChildhoodMementoTrash = base.GameController.FindCardsWhere(new LinqCardCriteria((Card c) => c.Identifier == "BelovedChildhoodMemento" && c.Location.IsTrash && c.Location.IsVillain));


                if (OneMillionDollarsTrash.Any())
                {
                    coroutine3 = base.GameController.PlayCard(TurnTakerController, OneMillionDollarsTrash.FirstOrDefault(), isPutIntoPlay: true, cardSource: GetCardSource());
                    if (UseUnityCoroutines)
                    {
                        yield return GameController.StartCoroutine(coroutine3);
                    }
                    else
                    {
                        GameController.ExhaustCoroutine(coroutine3);
                    }
                }
                else
                {
                    if (MasterpieceTrash.Any())
                    {
                        coroutine3 = base.GameController.PlayCard(TurnTakerController, MasterpieceTrash.FirstOrDefault(), isPutIntoPlay: true, cardSource: GetCardSource());
                        if (UseUnityCoroutines)
                        {
                            yield return GameController.StartCoroutine(coroutine3);
                        }
                        else
                        {
                            GameController.ExhaustCoroutine(coroutine3);
                        }
                    }
                    else
                    {
                        if (PricelessAntiquesTrash.Any())
                        {
                            coroutine3 = base.GameController.PlayCard(TurnTakerController, PricelessAntiquesTrash.FirstOrDefault(), isPutIntoPlay: true, cardSource: GetCardSource());
                            if (UseUnityCoroutines)
                            {
                                yield return GameController.StartCoroutine(coroutine3);
                            }
                            else
                            {
                                GameController.ExhaustCoroutine(coroutine3);
                            }
                        }
                        else
                        {
                            if (FamilyHeirloomsTrash.Any())
                            {
                                coroutine3 = base.GameController.PlayCard(TurnTakerController, FamilyHeirloomsTrash.FirstOrDefault(), isPutIntoPlay: true, cardSource: GetCardSource());
                                if (UseUnityCoroutines)
                                {
                                    yield return GameController.StartCoroutine(coroutine3);
                                }
                                else
                                {
                                    GameController.ExhaustCoroutine(coroutine3);
                                }
                            }
                            else
                            {
                                if (ShinyJewelsTrash.Any())
                                {
                                    if (GoldBarsTrash.Any())
                                    {
                                        coroutine3 = base.GameController.SelectAndPlayCard(null, ShinyJewelsAndGoldBarsInTrash);

                                        if (UseUnityCoroutines)
                                        {
                                            yield return GameController.StartCoroutine(coroutine3);
                                        }
                                        else
                                        {
                                            GameController.ExhaustCoroutine(coroutine3);
                                        }

                                    }
                                    else
                                    {
                                        coroutine3 = base.GameController.PlayCard(TurnTakerController, ShinyJewelsTrash.FirstOrDefault(), isPutIntoPlay: true, cardSource: GetCardSource());
                                        if (UseUnityCoroutines)
                                        {
                                            yield return GameController.StartCoroutine(coroutine3);
                                        }
                                        else
                                        {
                                            GameController.ExhaustCoroutine(coroutine3);
                                        }
                                    }
                                }
                                else
                                {
                                    if (GoldBarsTrash.Any())
                                    {
                                        coroutine3 = base.GameController.PlayCard(TurnTakerController, GoldBarsTrash.FirstOrDefault(), isPutIntoPlay: true, cardSource: GetCardSource());
                                        if (UseUnityCoroutines)
                                        {
                                            yield return GameController.StartCoroutine(coroutine3);
                                        }
                                        else
                                        {
                                            GameController.ExhaustCoroutine(coroutine3);
                                        }
                                    }
                                    else
                                    {

                                        if (CollectorsItemTrash.Any())
                                        {
                                            coroutine3 = base.GameController.PlayCard(TurnTakerController, CollectorsItemTrash.FirstOrDefault(), isPutIntoPlay: true, cardSource: GetCardSource());
                                            if (UseUnityCoroutines)
                                            {
                                                yield return GameController.StartCoroutine(coroutine3);
                                            }
                                            else
                                            {
                                                GameController.ExhaustCoroutine(coroutine3);
                                            }
                                        }
                                        else
                                        {
                                            if (BelovedChildhoodMementoTrash.Any())
                                            {
                                                coroutine3 = base.GameController.PlayCard(TurnTakerController, BelovedChildhoodMementoTrash.FirstOrDefault(), isPutIntoPlay: true, cardSource: GetCardSource());
                                                if (UseUnityCoroutines)
                                                {
                                                    yield return GameController.StartCoroutine(coroutine3);
                                                }
                                                else
                                                {
                                                    GameController.ExhaustCoroutine(coroutine3);
                                                }
                                            }
                                            else
                                            {

                                                message++;

                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }






            if (message == cardsToRetrieve)
            {
                IEnumerator coroutine6 = base.GameController.SendMessageAction($"There are no case cards in the villain trash.", Priority.Medium, GetCardSource());
                if (UseUnityCoroutines)
                {
                    yield return GameController.StartCoroutine(coroutine6);
                }
                else
                {
                    GameController.ExhaustCoroutine(coroutine6);
                }
            }
            else
            {
                if (message != 0)
                {
                    IEnumerator coroutine6 = base.GameController.SendMessageAction($"There are no more case cards in the villain trash.", Priority.Medium, GetCardSource());
                    if (UseUnityCoroutines)
                    {
                        yield return GameController.StartCoroutine(coroutine6);
                    }
                    else
                    {
                        GameController.ExhaustCoroutine(coroutine6);
                    }
                }
            }



        }
        private bool ShinyJewelsAndGoldBarsInTrash(Card c)
        {
            return ((c.Identifier == "ShinyJewels" || c.Identifier == "GoldBars") && c.Location.IsTrash && c.Location.IsVillain);
        }



    }
}
