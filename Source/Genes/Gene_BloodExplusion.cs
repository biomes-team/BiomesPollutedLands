using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace BMT_PollutedLands
{

	public class Gene_BloodExplusion : Gene
	{

		public GeneExtension Props => def.GetModExtension<GeneExtension>();

		public int nextTick = -1;

		public override void PostAdd()
		{
			base.PostAdd();
			ResetNextTick();
		}

		public override void Tick()
		{
			base.Tick();
			nextTick--;
			if (nextTick > 0)
			{
				return;
			}
			Vomit();
			ResetNextTick();
		}

		public void ResetNextTick()
		{
			nextTick = Props.intervals.RandomInRange;
		}

		public void Vomit()
		{
			if (!pawn.health.hediffSet.AnyHediffMakesSickThought)
			{
				return;
			}
			// pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, resumeCurJobAfterwards: true);
			pawn.jobs.StartJob(JobMaker.MakeJob(BMT_GenesDefOf.BMT_BloodVomit), JobCondition.InterruptForced, null, resumeCurJobAfterwards: true);
			if (!SanguophageUtility.WouldDieFromAdditionalBloodLoss(pawn, Props.bloodlossSeverity))
			{
				Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BloodLoss, pawn);
				hediff.Severity = Props.bloodlossSeverity;
				pawn.health.AddHediff(hediff);
				// PL_GeneUtility.TrySpawnFilth(pawn, new(0, 1), pawn.RaceProps.BloodDef);
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: BloodVomit",
					action = delegate
					{
						Vomit();
					}
				};
			}
		}

	}

}
