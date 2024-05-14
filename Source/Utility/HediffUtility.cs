using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace BMT_PollutedLands
{

	public static class HediffUtility
	{

		public static void BodyPartsGiver(List<BodyPartDef> bodyparts, Pawn pawn, HediffDef hediffDef, GeneDef geneDef)
		{
			int num = 0;
			foreach (BodyPartDef bodypart in bodyparts)
			{
				if (!pawn.RaceProps.body.GetPartsWithDef(bodypart).EnumerableNullOrEmpty() && num <= pawn.RaceProps.body.GetPartsWithDef(bodypart).Count)
				{
					Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn);
					pawn.health.AddHediff(hediff, pawn.RaceProps.body.GetPartsWithDef(bodypart).ToArray()[num]);
					num++;
				}
			}
		}

		public static bool TryAddOrRemoveHediff(HediffDef hediffDef, Pawn pawn, Gene gene, List<BodyPartDef> bodyparts = null, bool randomizeSeverity = false)
		{
			if (hediffDef == null)
			{
				return false;
			}
			if (gene.Active)
			{
				return TryAddHediff(hediffDef, pawn, gene.def, bodyparts, randomizeSeverity);
			}
			else
			{
				TryRemoveHediff(hediffDef, pawn);
			}
			return false;
		}

		public static bool TryAddHediff(HediffDef hediffDef, Pawn pawn, GeneDef geneDef, List<BodyPartDef> bodyparts = null, bool randomizeSeverity = false)
		{
			if (!pawn.health.hediffSet.HasHediff(hediffDef))
			{
				if (!bodyparts.NullOrEmpty())
				{
					HediffUtility.BodyPartsGiver(bodyparts, pawn, hediffDef, geneDef);
					return true;
				}
				// pawn.health.AddHediff(hediffDef);
				Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn);
				if (randomizeSeverity)
				{
					FloatRange floatRange = new(hediffDef.minSeverity, hediffDef.maxSeverity);
					hediff.Severity = floatRange.RandomInRange;
				}
				pawn.health.AddHediff(hediff);
				return true;
			}
			return false;
		}

		public static bool TryRemoveHediff(HediffDef hediffDef, Pawn pawn)
		{
			if (hediffDef == null)
			{
				return false;
			}
			if (pawn.health.hediffSet.HasHediff(hediffDef))
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
				if (firstHediffOfDef != null)
				{
					pawn.health.RemoveHediff(firstHediffOfDef);
				}
				return true;
			}
			return false;
		}

	}
}
