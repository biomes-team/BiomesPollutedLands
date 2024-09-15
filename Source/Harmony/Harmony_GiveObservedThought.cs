using HarmonyLib;
using RimWorld;
using Verse;

namespace BMT_PollutedLands;

[HarmonyPatch(typeof(Pawn), "GiveObservedThought")]
public class Harmony_GiveObservedThought
{
    public static void Postfix(Pawn __instance, ref Thought_Memory __result)
    {
        if (__instance.def == BMT_AnimalsDefOf.BMT_Maligoat)
        {
            var thought = (Thought_MemoryObservation)ThoughtMaker.MakeThought(BMT_AnimalsDefOf.BMT_ObservedMaligoat);
            thought.Target = __instance;
            __result = thought;
        }
    }
}