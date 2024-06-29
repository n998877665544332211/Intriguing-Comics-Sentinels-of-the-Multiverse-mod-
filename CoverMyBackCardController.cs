using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class CoverMyBackCardController : CardController
    {
        public CoverMyBackCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger((DealDamageAction dd) => dd.DidDealDamage && IsBoss(dd.Target), (DealDamageAction dd) => DealDamage(base.CharacterCard, dd.DamageSource.Card, 1, DamageType.Melee), TriggerType.DealDamage, TriggerTiming.After);
        }

        private bool IsBoss(Card c)
        {
            return c.DoKeywordsContain("boss");
        }
    }
}