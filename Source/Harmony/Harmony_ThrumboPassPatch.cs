using HarmonyLib;
using RimWorld;
using Verse;

namespace BMT_PollutedLands
{
    [HarmonyPatch(typeof(IncidentWorker_ThrumboPasses), nameof(IncidentWorker_ThrumboPasses.ChanceFactorNow))]
    public class Harmony_ThrumboPassPatch
    {
        public static void Postfix(IIncidentTarget target, ref float __result)
        {
            Map map = (Map)target;
            __result = 1 - map.pollutionGrid.TotalPollutionPercent;
        }
    }
}
