using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System.Collections;

namespace IntriguingComics.Frank
{
    public class TheRestOfTheGangCardController : CardController
    {
        public TheRestOfTheGangCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger((DealDamageAction dd) => dd.DidDealDamage && dd.DamageSource.Card != null && dd.DamageSource.Card == base.CharacterCard && (IsHeroTarget(dd.Target) || (!dd.Target.IsTarget && IsHero(dd.Target) && !dd.Target.IsEnvironmentTarget && !IsVillainTarget(dd.Target))), (DealDamageAction dd) => DealDamage(base.Card, (Card card) => IsHeroTarget(card) && card != dd.Target, 1, DamageType.Melee), TriggerType.DealDamage, TriggerTiming.After);
        }

    }
}
