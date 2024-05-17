using RimWorld;
using System.Collections.Generic;
using Verse;

// namespace WVC
namespace BMT_PollutedLands
{

	[StaticConstructorOnStartup]
	public static class PL_PostInitialization
	{

		static PL_PostInitialization()
		{
			HarmonyPatches.HarmonyUtility.PostInitialPatches();
		}

	}

}
