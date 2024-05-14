using HarmonyLib;
using Verse;

namespace BMT_PollutedLands
{

	public class BMT_PollutedLands_Main : Mod
	{
		public BMT_PollutedLands_Main(ModContentPack content)
			: base(content)
		{
			new Harmony("biomesteam.biomespollutedlands").PatchAll();
		}
	}

}
