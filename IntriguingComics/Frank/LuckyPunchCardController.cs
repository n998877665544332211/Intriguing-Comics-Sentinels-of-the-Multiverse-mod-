using System.Collections;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class LuckyPunchCardController : CardController
    {
        public LuckyPunchCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            base.SpecialStringMaker.ShowHeroCharacterCardWithLowestHP();
        }

        public override IEnumerator Play()
        {
            IEnumerator coroutine = DealDamageToLowestHP(base.CharacterCard, 1, (Card c) => c.IsHeroCharacterCard, (Card c) => base.H + 1, DamageType.Melee);
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
