using HarmonyLib;
using RimWorld;
using Verse;

namespace BMT_PollutedLands
{
    [HarmonyPatch(typeof(IncidentWorker), nameof(IncidentWorker.ChanceFactorNow))]
    public class Harmony_ThrumboPassPatch
    {
        public static void Postfix(IncidentWorker __instance, IIncidentTarget target, ref float __result)
        {
            if(__instance is IncidentWorker_ThrumboPasses)
            {
                Map map = (Map)target;
                __result *= 1 - map.pollutionGrid.TotalPollutionPercent;

            }
        }
    }
}
