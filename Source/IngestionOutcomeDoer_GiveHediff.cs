using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BMT_PollutedLands
{
    public class IngestionOutcomeDoer_GiveHediffWithStat : IngestionOutcomeDoer_GiveHediff
    {

        public StatDef statDef;

        public bool divideByBodySize = true;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn);
            float effect = ((!(severity > 0f)) ? hediffDef.initialSeverity : severity);
            if (statDef != null)
            {
                effect *= 1f - pawn.GetStatValue(statDef);
            }
            AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, toleranceChemical, ref effect, multiplyByGeneToleranceFactors, divideByBodySize);
            hediff.Severity = effect;
            pawn.health.AddHediff(hediff);
        }

    }

}
