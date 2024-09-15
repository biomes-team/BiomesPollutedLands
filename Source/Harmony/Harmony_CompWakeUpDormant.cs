using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BMT_PollutedLands;

[HarmonyPatch(typeof(CompWakeUpDormant), "TickRareWorker")]
public static class Harmony_CompWakeUpDormant
{
    private static MethodInfo canTargetMethod = AccessTools.Method(
        typeof(TargetingParameters), "CanTarget");

    private static MethodInfo activateMethod = AccessTools.Method(
        typeof(CompWakeUpDormant), "Activate");


    private static MethodInfo swarmCallerMethod =
        AccessTools.Method(typeof(Harmony_CompWakeUpDormant),
            nameof(SwarmCall));


    public static void SwarmCall(Thing thing, CompWakeUpDormant instance)
    {
        //TODO: move this to a comp and check for the existence of said comp instead of checking for def
        if (thing.def == PL_AnimalsDefOf.BMT_Swarmcaller)
        {
            instance.Activate(thing);
        }
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
    {
        var checkedFlag = true;
        var codes = new List<CodeInstruction>(codeInstructions);
        foreach (var code in codes)
        {
            //if at can target call (opcode) AND has not been checked before (checkedFlag)
            if (code.opcode == OpCodes.Callvirt && code.operand == canTargetMethod &&
                checkedFlag)
            {
                //indicate to not check again
                checkedFlag = false;
                //load waker
                yield return new CodeInstruction(OpCodes.Ldloc_S, 4);
                //load CompWakeUpDormant
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                //call swarmcall method
                yield return new CodeInstruction(OpCodes.Call, swarmCallerMethod);
            }

            yield return code;
        }
    }
}