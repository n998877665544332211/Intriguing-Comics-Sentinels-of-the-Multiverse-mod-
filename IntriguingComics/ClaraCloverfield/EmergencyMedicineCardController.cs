using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.ClaraCloverfield
{
    public class EmergencyMedicineCardController : RescueCardController
    {
        public EmergencyMedicineCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }


        public override void AddTriggers()
        {
            //Whenever a hero would gain HP, they may have Clara Cloverfield gain that HP instead.
            AddTrigger((GainHPAction g) => g.HpGainer.IsHeroCharacterCard, HealClaraInsteadResponse, new TriggerType[]
                    {
                        TriggerType.WouldGainHP,
                        TriggerType.CancelAction,
                        TriggerType.GainHP
                    }, TriggerTiming.Before);
        }

        private Card Clara => base.GameController.FindCardsWhere((Card c) => c.Identifier == "ClaraCloverfield").FirstOrDefault();

        private IEnumerator HealClaraInsteadResponse(GainHPAction g)
        {

            TurnTaker tt = g.HpGainer.Owner;
            if (!tt.IsHero)
            {
                yield break;
            }
            HeroTurnTakerController heroTurnTakerController = base.GameController.FindHeroTurnTakerController(tt.ToHero());



                YesNoCardDecision yesNo = new YesNoCardDecision(GameController, heroTurnTakerController, SelectionType.Custom, Clara, null, cardSource: GetCardSource());
            IEnumerator coroutine3 = base.GameController.MakeDecisionAction(yesNo);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine3);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine3);
            }

            if (DidPlayerAnswerYes(yesNo))
            {



                IEnumerator coroutine2 = base.CancelAction(g, showOutput: false, isPreventEffect: false);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine2);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine2);
                }
                IEnumerator coroutine = base.GameController.GainHP(Clara, g.Amount, cardSource: g.CardSource);
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

        public override CustomDecisionText GetCustomDecisionText(IDecision decision)
        {
            if (decision is YesNoCardDecision)
            {
                return new CustomDecisionText("Should Clara Cloverfield gain this HP instead?", TurnTaker.Name + " is choosing whether Clara Cloverfield should gain this HP instead.", "Vote to have Clara Cloverfield gain this HP instead", "Have Clara Cloverfield gain HP instead");
            }
            return null;
        }
    }
}
