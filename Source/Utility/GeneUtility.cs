using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace BMT_PollutedLands
{

	public static class PL_GeneUtility
	{

		// Non-anomaly regen. With custom ticker
		// Regeneration
		public static void Regeneration(Pawn pawn, float regeneration = -1, bool ignoreScarification = true, int tick = 10)
		{
			List<Hediff_Injury> tmpHediffInjuries = new();
			List<Hediff_MissingPart> tmpHediffMissing = new();
			regeneration *= 0.000166666665f;
			// Anomaly regen ticks once every 10 ticks
			if (tick > 0f)
			{
				regeneration *= (tick / 10);
			}
			if (regeneration > 0f)
			{
				pawn.health.hediffSet.GetHediffs(ref tmpHediffInjuries, (Hediff_Injury h) => h.def != HediffDefOf.Scarification || !ignoreScarification);
				foreach (Hediff_Injury tmpHediffInjury in tmpHediffInjuries)
				{
					float num5 = Mathf.Min(regeneration, tmpHediffInjury.Severity);
					regeneration -= num5;
					tmpHediffInjury.Heal(num5);
					pawn.health.hediffSet.Notify_Regenerated(num5);
					if (regeneration <= 0f)
					{
						break;
					}
				}
				if (regeneration > 0f)
				{
					pawn.health.hediffSet.GetHediffs(ref tmpHediffMissing, (Hediff_MissingPart h) => h.Part.parent != null && !tmpHediffInjuries.Any((Hediff_Injury x) => x.Part == h.Part.parent) && pawn.health.hediffSet.GetFirstHediffMatchingPart<Hediff_MissingPart>(h.Part.parent) == null && pawn.health.hediffSet.GetFirstHediffMatchingPart<Hediff_AddedPart>(h.Part.parent) == null);
					using List<Hediff_MissingPart>.Enumerator enumerator3 = tmpHediffMissing.GetEnumerator();
					if (enumerator3.MoveNext())
					{
						Hediff_MissingPart current4 = enumerator3.Current;
						BodyPartRecord part = current4.Part;
						pawn.health.RemoveHediff(current4);
						Hediff hediff2 = pawn.health.AddHediff(HediffDefOf.Misc, part);
						float partHealth = pawn.health.hediffSet.GetPartHealth(part);
						hediff2.Severity = Mathf.Max(partHealth - 1f, partHealth * 0.9f);
						pawn.health.hediffSet.Notify_Regenerated(partHealth - hediff2.Severity);
					}
				}
			}
		}

	}
}
