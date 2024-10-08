using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace BMT_PollutedLands
{

	// DEV
	public class Gene_EvermutatingCells : Gene
    {

        public override void Tick()
        {
            base.Tick();
            if (!pawn.IsHashIntervalTick(67200))
            {
                return;
            }
            if (!Rand.Chance(0.1f))
            {
                return;
            }
            pawn.genes.AddGene(DefDatabase<GeneDef>.AllDefsListForReading.Where((GeneDef gene) => gene.displayCategory == BMT_GenesDefOf.BMT_MutaGenes && !pawn.genes.HasXenogene(gene)).RandomElement(), xenogene: true);
        }



    }

}
