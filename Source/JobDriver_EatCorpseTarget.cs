using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace BMT_PollutedLands
{

	public class JobDriver_CastAbilityMeleeWithReservation : JobDriver_CastAbility
	{

		public Thing Victim => job.GetTarget(TargetIndex.A).Thing;

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(Victim, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => !job.ability.CanCast && !job.ability.Casting);
			Ability ability = ((Verb_CastAbility)job.verbToUse).ability;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn(() => !ability.CanApplyOn(job.targetA));
			yield return Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, canHitNonTargetPawns: false);
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			job.ability?.Notify_StartedCasting();
		}

	}


}
