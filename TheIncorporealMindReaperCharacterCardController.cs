using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace IntriguingComics.MindReaper
{

	public class TheIncorporealMindReaperCharacterCardController : VillainCharacterCardController
{
	private Card Possession => base.GameController.FindCardsWhere((Card c) => c.Identifier == "Possession").FirstOrDefault();

	private Card PossessedHero => base.GameController.FindCardsWhere((Card c) => IsPossessed(c)).FirstOrDefault();

	private int NumberOfUnpossessedHeroes => FindCardsWhere((Card c) => c.IsHeroCharacterCard && c.IsTarget && !IsPossessed(c) && !c.IsIncapacitated && c.IsInPlayAndHasGameText).Count();

	private int PsychicBarriersInPlay => FindCardsWhere((Card c) => c.Identifier == "PsychicBarrier" && c.IsInPlayAndHasGameText).Count();

	public TheIncorporealMindReaperCharacterCardController(Card card, TurnTakerController turnTakerController)
		: base(card, turnTakerController)
	{
		base.SpecialStringMaker.ShowSpecialString(() => PossessedHero.Title + " is possessed.", () => true);
		base.SpecialStringMaker.ShowHeroCharacterCardWithHighestHP().Condition = () => !base.Card.IsFlipped;
		ShowNonPossessedHeroTargetWithHighestHP().Condition = () => !base.Card.IsFlipped;
		ShowNonPossessedHeroTargetWithLowestHP().Condition = () => base.Card.IsFlipped;
		base.SpecialStringMaker.ShowSpecialString(() => "Psychic Barrier is in play.").Condition = () => !base.Card.IsFlipped && PsychicBarriersInPlay == 1;
		base.SpecialStringMaker.ShowListOfCardsInPlay(new LinqCardCriteria((Card c) => c.IsProjection, "projection")).Condition = () => base.Card.IsFlipped && base.Game.IsAdvanced;
	}

	public override void AddTriggers()
	{
		AddTrigger((FlipCardAction c) => IsPossessed(c.CardToFlip.Card), VictoryResponse, TriggerType.GameOver, TriggerTiming.Before);
		AddTrigger((MoveCardAction moveCard) => IsPossessed(moveCard.CardToMove) && moveCard.Destination.Name == LocationName.OutOfGame, VictoryResponse, TriggerType.GameOver, TriggerTiming.Before);
		AddTrigger((FlipCardAction c) => c.CardToFlip.Card.IsHeroCharacterCard && !IsPossessed(c.CardToFlip.Card) && NumberOfUnpossessedHeroes == 0, (FlipCardAction c) => DestroyedHeroResponse(c), TriggerType.GameOver, TriggerTiming.After);
		AddTrigger((MoveCardAction moveCard) => moveCard.CardToMove.IsHeroCharacterCard && !IsPossessed(moveCard.CardToMove) && NumberOfUnpossessedHeroes == 0 && moveCard.Destination.Name == LocationName.OutOfGame, (MoveCardAction mca) => DestroyedHeroResponse(mca), TriggerType.GameOver, TriggerTiming.After);
		AddTrigger((DestroyCardAction c) => c.CardToDestroy.Card == Possession && c.CardSource.Card != base.CharacterCard, base.CancelResponse, TriggerType.CancelAction, TriggerTiming.Before);
		AddTrigger((MoveCardAction c) => c.CardToMove == Possession && c.CardSource.Card != base.CharacterCard, base.CancelResponse, TriggerType.CancelAction, TriggerTiming.Before);
		AddTrigger((FlipCardAction c) => c.CardToFlip.Card == Possession && c.CardSource.Card != base.CharacterCard, base.CancelResponse, TriggerType.CancelAction, TriggerTiming.Before);
		AddTrigger((CardEntersPlayAction p) => !base.Card.IsFlipped && p.CardEnteringPlay.IsVillain && p.CardEnteringPlay.IsOneShot, (CardEntersPlayAction p) => VillainOneShotResponse(p), TriggerType.MoveCard, TriggerTiming.After);
		AddReduceDamageTrigger((DealDamageAction dd) => !base.Card.IsFlipped, null, (DealDamageAction dd) => PsychicBarriersInPlay * 2, (Card c) => IsPossessed(c));
		AddFirstTimePerTurnRedirectTrigger((DealDamageAction dd) => dd.DamageSource.Card != null && IsPossessed(dd.Target) && IsPossessed(dd.DamageSource.Card) && !base.Card.IsFlipped, "PossessedHeroWouldDealSelfDamage", TargetType.HighestHP, (Card c) => IsHeroTarget(c) && !IsPossessed(c));
		AddFirstTimePerTurnRedirectTrigger((DealDamageAction dd) => dd.DamageSource.Card != null && IsPossessed(dd.Target) && IsPossessed(dd.DamageSource.Card) && base.Card.IsFlipped, "PossessedHeroWouldDealSelfDamage", TargetType.LowestHP, (Card c) => IsHeroTarget(c) && !IsPossessed(c));
		AddTrigger((DrawCardAction drawCard) => drawCard.IsSuccessful && drawCard.DidDrawCard && drawCard.DrawnCard.IsHero && PossessedHero.Owner == drawCard.HeroTurnTaker && base.Card.IsFlipped, PossessedHeroRegainHPResponse, TriggerType.GainHP, TriggerTiming.After);
		AddTrigger((UsePowerAction p) => IsPossessed(p.HeroCharacterCardUsingPower.Card) && base.Card.IsFlipped, PossessedHeroRegainHPResponse, TriggerType.GainHP, TriggerTiming.After);
		AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker && !base.Card.IsFlipped, PossessedHeroUsesBasePowerResponse, TriggerType.PhaseChange);
		AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker && base.Card.IsFlipped, base.FlipThisCharacterCardResponse, TriggerType.FlipCard);
		AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker && base.Card.IsFlipped, PossessedHeroDealsDamageResponse, TriggerType.PhaseChange);
		AddTrigger((GainHPAction g) => g.AmountActuallyGained > 0 && IsPossessed(g.HpGainer) && base.Game.IsAdvanced && base.Card.IsFlipped, (GainHPAction g) => GainHPResponse(g), TriggerType.GainHP, TriggerTiming.After);
		if (base.IsGameChallenge)
		{
			AddTrigger((DealDamageAction dd) => dd.DidDealDamage && IsPossessed(dd.DamageSource.Card), (DealDamageAction dd) => DealDamage(base.CharacterCard, dd.Target, 1, DamageType.Psychic, isIrreducible: true), TriggerType.DealDamage, TriggerTiming.After);
		}
	}

	private IEnumerator GainHPResponse(GainHPAction gain)
	{
		IEnumerator coroutine = base.GameController.GainHP(DecisionMaker, (Card card) => card.IsProjection, gain.AmountActuallyGained, null, optional: false, null, null, null, GetCardSource());
		if (base.UseUnityCoroutines)
		{
			yield return base.GameController.StartCoroutine(coroutine);
		}
		else
		{
			base.GameController.ExhaustCoroutine(coroutine);
		}
	}

	private IEnumerator PossessedHeroDealsDamageResponse(PhaseChangeAction action)
	{
		IEnumerator coroutine = DealDamage(FindCardsWhere((Card c) => IsPossessed(c)).FirstOrDefault(), (Card c) => IsHeroTarget(c) && !IsPossessed(c), 2, DamageType.Melee);
		if (base.UseUnityCoroutines)
		{
			yield return base.GameController.StartCoroutine(coroutine);
		}
		else
		{
			base.GameController.ExhaustCoroutine(coroutine);
		}
	}

	private IEnumerator PossessedHeroUsesBasePowerResponse(PhaseChangeAction action)
	{
		TurnTaker tt = PossessedHero.Owner;
		if (tt.IsHero)
		{
			HeroTurnTakerController heroTurnTakerController = base.GameController.FindHeroTurnTakerController(tt.ToHero());
			IEnumerator coroutine = base.GameController.SelectAndUsePower(heroTurnTakerController, optional: false, (Power p) => IsPossessed(p.CardController.Card), 1, eliminateUsedPowers: true, null, showMessage: false, allowAnyHeroPower: true, allowReplacements: true, canBeCancelled: true, null, forceDecision: true, allowOutOfPlayPower: false, GetCardSource());
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

	private IEnumerator VictoryResponse(GameAction ga)
	{
		IEnumerator coroutine = base.GameController.GameOver(EndingResult.AlternateVictory, PossessedHero.Title + " has been freed from Mind Reaper's clutches.", showEndingTextAsMessage: true, null, null, GetCardSource());
		if (base.UseUnityCoroutines)
		{
			yield return base.GameController.StartCoroutine(coroutine);
		}
		else
		{
			base.GameController.ExhaustCoroutine(coroutine);
		}
	}

	private IEnumerator PossessedHeroRegainHPResponse(GameAction ga)
	{
		IEnumerator coroutine = base.GameController.GainHP(PossessedHero, base.Game.H - 1);
		if (base.UseUnityCoroutines)
		{
			yield return base.GameController.StartCoroutine(coroutine);
		}
		else
		{
			base.GameController.ExhaustCoroutine(coroutine);
		}
	}

	private IEnumerator VillainOneShotResponse(CardEntersPlayAction cpa)
	{
		List<Card> highestHero = new List<Card>();
		IEnumerator coroutine3 = base.GameController.FindTargetWithHighestHitPoints(1, (Card c) => c.IsHeroCharacterCard, highestHero, null, null, evenIfCannotDealDamage: false, optional: false, null, ignoreBattleZone: false, GetCardSource());
		if (base.UseUnityCoroutines)
		{
			yield return base.GameController.StartCoroutine(coroutine3);
		}
		else
		{
			base.GameController.ExhaustCoroutine(coroutine3);
		}
		Card HighestHero = highestHero.FirstOrDefault();
		if (!IsPossessed(HighestHero))
		{
			GameController gameController = base.GameController;
			HeroTurnTakerController decisionMaker = DecisionMaker;
			Card possession = Possession;
			Location nextToLocation = HighestHero.NextToLocation;
			CardSource cardSource = GetCardSource();
			IEnumerator coroutine2 = gameController.MoveCard(decisionMaker, possession, nextToLocation, toBottom: false, isPutIntoPlay: false, playCardIfMovingToPlayArea: true, null, showMessage: false, null, null, null, evenIfIndestructible: false, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, cardSource);
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
			IEnumerator coroutine = base.GameController.FlipCard(base.CharacterCardController, treatAsPlayed: false, treatAsPutIntoPlay: false, null, null, GetCardSource());
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

	private IEnumerator DestroyedHeroResponse(GameAction ga)
	{
		if (NumberOfUnpossessedHeroes == 0)
		{
			IEnumerator coroutine = base.GameController.GameOver(EndingResult.AlternateDefeat, PossessedHero.Title + " and Mind Reaper are now one... forever.", showEndingTextAsMessage: true, null, null, GetCardSource());
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

	public bool IsPossessed(Card card)
	{
		return card.NextToLocation.Cards.Any((Card c) => c.Identifier == "Possession");
	}

	private SpecialString ShowNonPossessedHeroTargetWithHighestHP(int ranking = 1, int numberOfTargets = 1, bool bothBattleZones = false)
	{
		return base.SpecialStringMaker.ShowHighestHP(ranking, () => numberOfTargets, new LinqCardCriteria((Card c) => IsHero(c) && !IsPossessed(c), "non-possessed hero", useCardsSuffix: true, useCardsPrefix: false, "target", "targets"), null, bothBattleZones);
	}

	private SpecialString ShowNonPossessedHeroTargetWithLowestHP(int ranking = 1, int numberOfTargets = 1, bool bothBattleZones = false)
	{
		return base.SpecialStringMaker.ShowLowestHP(ranking, () => numberOfTargets, new LinqCardCriteria((Card c) => IsHero(c) && !IsPossessed(c), "non-possessed hero", useCardsSuffix: true, useCardsPrefix: false, "target", "targets"), null, bothBattleZones);
	}
	}
}
