using System;
using System.Collections.Generic;
using Verse;

namespace BMT_PollutedLands
{

	public class HediffCompProperties_GeneHediff : HediffCompProperties
	{

		public GeneDef geneDef;

		public List<GeneDef> geneDefs;

		// ~1 day
		public IntRange checkInterval = new (56720, 72032);

		public HediffCompProperties_GeneHediff()
		{
			compClass = typeof(HediffComp_GeneHediff);
		}

	}

	public class HediffComp_GeneHediff : HediffComp
	{

		public GeneDef geneDef;

		public HediffCompProperties_GeneHediff Props => (HediffCompProperties_GeneHediff)props;

		public int nextTick = 60000;

		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			nextTick = Props.checkInterval.RandomInRange;
		}

		public override void CompPostTick(ref float severityAdjustment)
		{
			if (!Pawn.IsHashIntervalTick(nextTick))
			{
				return;
			}
			if (geneDef == null && Props.geneDef != null)
			{
				geneDef = Props.geneDef;
			}
			if (!PL_GeneUtility.HasActiveGene(geneDef, parent.pawn))
			{
				base.Pawn.health.RemoveHediff(parent);
			}
		}

		public override void CompPostPostRemoved()
		{
			if (geneDef == null)
			{
				geneDef = Props.geneDef;
			}
			if (PL_GeneUtility.HasActiveGene(geneDef, Pawn))
			{
				BodyPartDef bodyPart = parent?.Part?.def;
				List<BodyPartDef> bodyparts = new();
				if (bodyPart != null)
				{
					bodyparts.Add(bodyPart);
				}
				if (HediffUtility.TryAddHediff(Def, Pawn, geneDef, bodyparts))
				{
					if (DebugSettings.ShowDevGizmos)
					{
						Log.Warning("Trying to remove " + Def.label + " hediff, but " + Pawn.Name.ToString() + " has the required gene. Hediff is added back.");
					}
				}
			}
		}

		public override void CompExposeData()
		{
			Scribe_Defs.Look(ref geneDef, "geneDef");
			Scribe_Values.Look(ref nextTick, "nextTick", 60000);
		}

	}

}
