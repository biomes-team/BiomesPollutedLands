using RimWorld;
using System.Collections.Generic;
using Verse;

namespace BMT_PollutedLands
{

	public class CompProperties_AutoTame : CompProperties
	{
		public CompProperties_AutoTame()
		{
			compClass = typeof(CompAutoTame);
		}
	}

	public class CompAutoTame : ThingComp
	{

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			//base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				CheckFaction();
			}
		}

		public void CheckFaction()
		{
			Pawn pawn = parent as Pawn;
			if (pawn.Faction != Faction.OfPlayer)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(null, pawn, false);
            }
		}
	}

}
