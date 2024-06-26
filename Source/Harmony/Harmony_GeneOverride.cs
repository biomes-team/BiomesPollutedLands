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

		[HarmonyPatch(typeof(Gene), "OverrideBy")]
		public static class Patch_Gene_OverrideBy
		{
			public static void Postfix(Gene __instance, Gene overriddenBy)
			{
				if (__instance is IGeneOverridden geneOverridden)
				{
					if (overriddenBy != null)
					{
						geneOverridden.Notify_OverriddenBy(overriddenBy);
					}
					else
					{
						geneOverridden.Notify_Override();
					}
				}
			}
		}

	}

}
