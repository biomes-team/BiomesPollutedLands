using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BMT_PollutedLands
{
    public class QuestNode_SetHediff : QuestNode
    {
        public SlateRef<IEnumerable<Pawn>> pawns;
        public SlateRef<HediffDef> hediff;

        protected override void RunInt()
        {
            Slate slate = RimWorld.QuestGen.QuestGen.slate;
            int var = 0;
            foreach (Pawn pawn in this.pawns.GetValue(slate))
            {
                pawn.health.AddHediff(hediff.GetValue(slate));
            }
            slate.Set<int>("childCount", var);
        }

        protected override bool TestRunInt(Slate slate) => true;
    }
}
