using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class FollowMyLeadCardController : CardController
    {
        public FollowMyLeadCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger((DealDamageAction dd) => dd.DidDealDamage && dd.DamageSource.Card != null && IsBoss(dd.DamageSource.Card), (DealDamageAction dd) => DealDamage(base.CharacterCard, dd.Target, 1, DamageType.Melee), TriggerType.DealDamage, TriggerTiming.After);
        }

        private bool IsBoss(Card c)
        {
            return c.DoKeywordsContain("boss");
        }
    }
}
