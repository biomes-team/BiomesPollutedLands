using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Noise;

namespace BMT_PollutedLands
{
    public class HediffGiver_PollutedOnly : HediffGiver
    {

        public SimpleCurve pollutionAmountCurve;

        public float mtbDays;

        public override void OnIntervalPassed(Pawn pawn, Hediff cause)
        {
            // This event doesn't make sense to do often, as it is designed for the pawn to live in a toxic settlement, and not just pass by.
            if (!Rand.MTBEventOccurs(mtbDays, 60000f, 10000f))
            {
                return;
            }
            if (pawn.Map == null)
            {
                return;
            }
            // Immunity check
            if (pawn.health.immunity.AnyGeneMakesFullyImmuneTo(hediff))
            {
                return;
            }
            // Vanilla usually relies on Pollution Level, we will do the same.
            PollutionLevel pollutionLevel1 = Find.WorldGrid[pawn.Map.Tile].PollutionLevel();
            if (pollutionLevel1 == PollutionLevel.Light || pollutionLevel1 == PollutionLevel.None)
            {
                return;
            }
            // Calculate the impact of pollution on the pawn.
            float pollutionLevel = pawn.Map.pollutionGrid.TotalPollutionPercent;
            // Check both stats and take the best one.
            float toxResist = pawn.GetStatValue(StatDefOf.ToxicResistance);
            float enviResist = pawn.GetStatValue(StatDefOf.ToxicEnvironmentResistance);
            float finalPollutionLevel = pollutionAmountCurve.Evaluate(pollutionLevel) * (1f - (toxResist > enviResist ? toxResist : enviResist));
            // We use the level of pollution vs resistance for randomization. It should give a fairer chance to get a mutapox, but not make it an assache.
            if (!Rand.Chance(finalPollutionLevel))
            {
                return;
            }
            if (TryApply(pawn))
            {
                SendLetter(pawn, cause);
            }
        }

    }
}
