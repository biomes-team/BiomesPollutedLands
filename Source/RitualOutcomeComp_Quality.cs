using RimWorld;
using System.Collections.Generic;
using Verse;

// namespace WVC
namespace BMT_PollutedLands
{

	public class RitualOutcomeComp_PawnGenes : RitualOutcomeComp_QualitySingleOffset
	{

		[NoTranslate]
		public string roleId;

		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Pawn pawn = ritual?.PawnWithRole(roleId);
			if (pawn == null)
			{
				return 0f;
			}
			return GetBirthQualityOffsetFromGenes(pawn);
		}

		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			if (ritual == null)
			{
				return labelAbstract;
			}
			Pawn pawn = ritual?.PawnWithRole(roleId);
			if (pawn == null)
			{
				return null;
			}
			float offset = GetBirthQualityOffsetFromGenes(pawn);
			string text = ((offset < 0f) ? "" : "+");
			return LabelForDesc.Formatted(pawn.Named("PAWN")) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(text + offset.ToStringPercent()) + ".";
		}

		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Pawn pawn = assignments.FirstAssignedPawn(roleId);
			if (pawn == null)
			{
				return null;
			}
			float offset = GetBirthQualityOffsetFromGenes(pawn);
			if (offset == 0f)
			{
				return null;
			}
			return new QualityFactor
			{
				label = label.Formatted(pawn.Named("PAWN")),
				qualityChange = ExpectedOffsetDesc(offset > 0f, offset),
				positive = (offset > 0f),
				present = true,
				quality = offset,
				priority = 0f
			};
		}

		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return 1f;
		}

		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		public static float GetBirthQualityOffsetFromGenes(Pawn pawn)
		{
			if (pawn?.genes == null)
			{
				return 0f;
			}
			List<Gene> genesListForReading = pawn.genes.GenesListForReading;
			float offest = 0f;
			for (int j = 0; j < genesListForReading.Count; j++)
			{
				Gene gene = genesListForReading[j];
				if (!gene.Active)
				{
					continue;
				}
				GeneExtension general = gene.def?.GetModExtension<GeneExtension>();
				if (general == null)
				{
					continue;
				}
				offest += general.birthQualityOffset;
			}
			return offest;
		}

	}

}
