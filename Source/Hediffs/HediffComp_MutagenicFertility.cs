using RimWorld;
using System.Collections.Generic;
using Verse;

namespace BMT_PollutedLands
{

	public class HediffCompProperties_MutagenicFertility : HediffCompProperties
	{

		public List<GeneDef> anyGeneDefs = new();

		public IntRange mutagenesCountRange = new(1, 2);

		public HediffCompProperties_MutagenicFertility()
		{
			compClass = typeof(HediffComp_MutagenicFertility);
		}

	}

	public class HediffComp_MutagenicFertility : HediffComp
	{

		public HediffCompProperties_MutagenicFertility Props => (HediffCompProperties_MutagenicFertility)props;

		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			// Log.Error("0");
			if (parent is Hediff_Pregnant pregnancy && PL_GeneUtility.HasAnyActiveGene(Props.anyGeneDefs, parent.pawn))
			{
				// Log.Error("1");
				// GeneSet newGeneSet = new();
				// AddParentGenes(pregnancy.Mother, newGeneSet);
				// AddParentGenes(pregnancy.Father, newGeneSet);
				// if (!parent.pawn.Spawned || Props.addSurrogateGenes)
				// {
					// AddParentGenes(parent.pawn, newGeneSet);
				// }
				// newGeneSet.SortGenes();
				if (pregnancy?.geneSet == null)
				{
					return;
				}
				List<GeneDef> possibleGenes = PL_GeneUtility.GetAllMutagenes();
				// IntRange range = new(1, 2);
				int maxGenes = Props.mutagenesCountRange.RandomInRange;
				if (PL_GeneUtility.HasAnyActiveGene(Props.anyGeneDefs, pregnancy.Father) && PL_GeneUtility.HasAnyActiveGene(Props.anyGeneDefs, pregnancy.Mother))
				{
					maxGenes++;
				}
				AddParentGenes(maxGenes, pregnancy.geneSet, possibleGenes);
			}
		}

		public static void AddParentGenes(int maxGenes, GeneSet geneSet, List<GeneDef> possibleGenes)
		{
			// List<GeneDef> genes = XaG_GeneUtility.ConvertGenesInGeneDefs(parent.genes.Endogenes);
			possibleGenes.Shuffle();
			int count = 0;
			foreach (GeneDef gene in possibleGenes)
			{
				if (count >= maxGenes)
				{
					break;
				}
				if (geneSet.GenesListForReading.Contains(gene))
				{
					continue;
				}
				geneSet.AddGene(gene);
				count++;
			}
		}

	}

}
