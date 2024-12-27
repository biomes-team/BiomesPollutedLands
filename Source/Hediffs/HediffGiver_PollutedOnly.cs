using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Noise;

namespace BMT_PollutedLands
{
    // Old mutapox event
    public class HediffGiver_PollutedOnly : HediffGiver
    {

        public SimpleCurve pollutionAmountCurve;

        public float mtbDays;

        public override void OnIntervalPassed(Pawn pawn, Hediff cause)
        {
            // This event doesn't make sense to do often, as it is designed for the pawn to live in a toxic settlement, and not just pass by.
            //Log.Error("Tick");
            //if (!Rand.MTBEventOccurs(mtbDays, 60000f, 10000f))
            //{
            //    return;
            //}
            if (!pawn.IsHashIntervalTick(30000))
            {
                return;
            }
            //Log.Error("0");
            // Mutapox chance if mtbDays 240 => ~0.004
            if (!Rand.Chance(1f / mtbDays))
            {
                return;
            }
            //Log.Error("1");
            // Skip visitors factions
            if (pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer))
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
                //Log.Error("Mutapox");
                SendLetter(pawn, cause);
            }
        }

    }

    // New mutapox event
    public class IncidentWorker_Mutapox : IncidentWorker_DiseaseHuman
    {

        public GeneExtension Props => def?.GetModExtension<GeneExtension>();

        protected override IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target)
        {
            List<Pawn> newList = [];
            foreach (Pawn pawn in base.PotentialVictimCandidates(target))
            {
                if (CanGetMutapox(pawn, Props.pollutionAmountCurve))
                {
                    newList.Add(pawn);
                }
            }
            return newList;
        }

        public static bool CanGetMutapox(Pawn pawn, SimpleCurve pollutionAmountCurve)
        {
            if (pawn.Map == null)
            {
                return false;
            }
            // Immunity check
            if (pawn.health.immunity.AnyGeneMakesFullyImmuneTo(BMT_HediffDefOf.BMT_Mutapox))
            {
                return false;
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
                return false;
            }
            return true;
        }

    }

}
