using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace BMT_PollutedLands
{

	public class Gene_MoltingRegeneration : Gene
	{

		public GeneExtension Props => def.GetModExtension<GeneExtension>();

		private int ticksToHeal;

		// private static readonly IntRange HealingIntervalTicksRange = new(450000, 900000);

		public override void PostAdd()
		{
			base.PostAdd();
			ResetInterval();
		}

		public override void Tick()
		{
			base.Tick();
			ticksToHeal--;
			if (ticksToHeal <= 0)
			{
				TryHealWound();
			}
		}

		private void ResetInterval()
		{
			ticksToHeal = Props.intervals.RandomInRange;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: HealPermanentWound",
					action = delegate
					{
						TryHealWound();
					}
				};
			}
		}

		private void TryHealWound()
		{
			HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(pawn, LabelCap);
			PL_GeneUtility.TrySpawnFilth(pawn, new(1,2), Props.filthDef);
			ResetInterval();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref ticksToHeal, "ticksToHeal", 0);
		}

	}

}
