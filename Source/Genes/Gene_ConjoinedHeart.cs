using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace BMT_PollutedLands
{

	public class Gene_ConjoinedHeart : Gene
	{

		public GeneExtension Props => def.GetModExtension<GeneExtension>();

		public int cooldown = -1;

		public override void Tick()
		{
			base.Tick();
			cooldown--;
			if (!pawn.IsHashIntervalTick(1667))
			{
				return;
			}
			HeartCheck();
		}

		public void HeartCheck()
		{
			if (!pawn.health.hediffSet.TryGetHediff(HediffDefOf.BloodLoss, out Hediff bloodloss) || cooldown > 0f)
			{
				return;
			}
			if (bloodloss.Severity > Props.maxSeverity)
			{
				pawn.health.RemoveHediff(bloodloss);
				List<BodyPartDef> bodyparts = new() { BodyPartDefOf.Heart };
				if (Rand.Chance(Props.chance))
				{
					HediffUtility.TryAddOrRemoveHediff(Props.hediffDef, pawn, this, bodyparts);
				}
				cooldown = Props.intervals.RandomInRange;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref cooldown, "cooldown", -1);
		}

	}

}
