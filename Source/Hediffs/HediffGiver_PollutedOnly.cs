using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace BMT_PollutedLands
{
    public class HediffGiver_PollutedOnly : HediffGiver
    {
        public SimpleCurve pollutionAmountCurve = new SimpleCurve
        {
            new CurvePoint(0, 0f),
            new CurvePoint(0.25f, 0.25f),
            new CurvePoint(0.5f, 0.4f),
            new CurvePoint(0.8f, 0.6f),
            new CurvePoint(1f, 0.8f),
        };

        public float mtbDays;
        public override void OnIntervalPassed(Pawn pawn, Hediff cause)
        {
            if(pawn.Map == null) return;

            float pollutionLevel = pawn.Map.pollutionGrid.TotalPollutionPercent;
            if(pollutionLevel <= 0.01f)
            {
                return;
            }
            float num = mtbDays;

            //pollution percentage minus resistance
            float x = pollutionLevel - pawn.GetStatValue(StatDefOf.ToxicEnvironmentResistance);

            x = pollutionAmountCurve.Evaluate(pollutionLevel);

            if (x != 0f && Rand.MTBEventOccurs(num / x, 60000f, 60f) && TryApply(pawn))
            {
                SendLetter(pawn, cause);
            }
        }
    }
}
