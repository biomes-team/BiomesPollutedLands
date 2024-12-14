using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BMT_PollutedLands
{
    internal class IngestionOutcomeDoer_RemoveHediff : IngestionOutcomeDoer
    {
        public HediffDef hediffDef;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            HediffUtility.TryRemoveHediff(hediffDef, pawn);
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
        {
            IngestionOutcomeDoer_RemoveHediff outcomeDoerGiveHediff = this;
            if (parentDef.IsDrug && (double)outcomeDoerGiveHediff.chance >= 1.0)
            {
                foreach (StatDrawEntry specialDisplayStat in outcomeDoerGiveHediff.hediffDef.SpecialDisplayStats(StatRequest.ForEmpty()))
                    yield return specialDisplayStat;
            }
        }
        
    }
}
