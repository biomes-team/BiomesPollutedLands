using RimWorld;
using System.Collections.Generic;
using Verse;

// namespace WVC
namespace BMT_PollutedLands
{

	[DefOf]
	public static class PL_GenesDefOf
	{
		public static GeneCategoryDef BMT_MutaGenes;
	}

	[StaticConstructorOnStartup]
	public static class PL_PostInitialization
	{

		static PL_PostInitialization()
		{
			HarmonyPatches.HarmonyUtility.PostInitialPatches();
		}

	}

}
