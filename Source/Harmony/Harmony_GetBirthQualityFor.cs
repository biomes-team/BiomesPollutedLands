using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BMT_PollutedLands
{

	namespace HarmonyPatches
	{

		// Unused cause RitualOutcomeEffectDef
		[HarmonyPatch(typeof(PregnancyUtility), "GetBirthQualityFor")]
		public static class Patch_PregnancyUtility_GetBirthQualityFor
		{

			[HarmonyPostfix]
			public static void Postfix(ref float __result, ref Pawn mother)
			{
				if (mother?.genes?.GetFirstGeneOfType<Gene_MutagenicFertility>() != null)
				{
					__result *= 0.8f;
				}
			}
		}

	}

}
