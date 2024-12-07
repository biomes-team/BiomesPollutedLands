using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BMT_PollutedLands
{
    public class Hediff_Mutapox : HediffWithComps
    {
        public float mtbMutDays = 0.6f;

        float threshold = 0.4f;

        public override void Tick()
        {
            if(Severity < threshold)
            {
                return;
            }
            float num = (Severity - threshold)/(1-threshold);

            if (Rand.MTBEventOccurs(mtbMutDays / num, 60000f, 1f))
            {
                Log.Message("chance: " + 1 / (60000f *(mtbMutDays / num)));
                List<GeneDef> defs = DefDatabase<GeneDef>.AllDefsListForReading;


                Random r = new Random();
                foreach (GeneDef def in defs.OrderBy(x => r.Next()))
                {
                    if (pawn.genes.HasEndogene(def))
                    {
                        continue;
                    }

                    pawn.genes.AddGene(def, false);
                    SendLetter(def);
                    
                    return;
                }
            }
        }

        void SendLetter(GeneDef gene)
        {
            if (!PawnUtility.ShouldSendNotificationAbout(pawn))
                return;

                Find.LetterStack.ReceiveLetter("BMT_LetterLabelMutaPoxMutation".Translate().CapitalizeFirst(), "BMT_LetterMutaPoxMutation".Translate((NamedArgument)gene.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NeutralEvent, (LookTargets)(Thing)pawn);
        }
    }
}
