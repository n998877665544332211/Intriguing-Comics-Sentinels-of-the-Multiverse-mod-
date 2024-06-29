using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.Frank
{
    public class AssignedOutfitCardController : CardController
    {
        public AssignedOutfitCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddReduceDamageTrigger((Card c) => Ishench(c), 1);
        }

        private bool Ishench(Card c)
        {
            return c.DoKeywordsContain("hench");
        }
    }
}