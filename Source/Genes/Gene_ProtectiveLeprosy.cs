using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace BMT_PollutedLands
{

	public class Gene_ProtectiveLeprosy : Gene
	{

		public GeneExtension Props => def.GetModExtension<GeneExtension>();

		public int cooldown = -1;

		public override void Tick()
		{
			base.Tick();
			cooldown--;
			if (!pawn.IsHashIntervalTick(2416))
			{
				return;
			}
			Infection();
		}

		public void Infection()
		{
			if (cooldown > 0f || !pawn.health.hediffSet.TryGetHediff(HediffDefOf.WoundInfection, out Hediff bloodloss))
			{
				return;
			}
			if (bloodloss.Severity > Props.maxSeverity && bloodloss.Part != null)
			{
				pawn.health.RemoveHediff(bloodloss);
				if (bloodloss.Part != pawn.RaceProps.body.corePart)
				{
					pawn.health.AddHediff(HediffDefOf.MissingBodyPart, bloodloss.Part);
				}
				cooldown = Props.intervals.RandomInRange;
				Messages.Message("BMT_PL_ProtectiveLeprosyDestroyPart".Translate(pawn.Name.ToStringFull), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref cooldown, "cooldown", -1);
		}

	}

}
