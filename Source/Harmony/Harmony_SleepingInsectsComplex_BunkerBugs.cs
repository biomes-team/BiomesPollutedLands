using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BMT_PollutedLands
{
    [HarmonyPatch(typeof(ComplexThreatWorker_SleepingInsects), "GetPawnKindsForPoints")]
    public class Harmony_SleepingInsectsComplex_BunkerBugs
    {
        public static IEnumerable<PawnKindDef> Postfix(IEnumerable<PawnKindDef> values)
        {
            foreach (PawnKindDef p in values)
            {
                if(!p.race.HasComp<CompDisableComplexInsectSpawn>())
                    yield return p;
            }
        }
    }


}
