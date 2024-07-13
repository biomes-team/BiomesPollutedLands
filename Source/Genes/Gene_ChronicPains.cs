using System;
using System.Collections.Generic;
using Verse;

namespace BMT_PollutedLands
{

	public class Gene_AddHediffWithInterval : Gene
	{

		public GeneExtension Props => def.GetModExtension<GeneExtension>();

		public int nextTick = -1;

		public override void PostAdd()
		{
			base.PostAdd();
			ResetNextTick();
		}

		public override void Tick()
		{
			base.Tick();
			nextTick--;
			if (nextTick > 0)
			{
				return;
			}
			Pain();
			ResetNextTick();
		}

		public void ResetNextTick()
		{
			nextTick = Props.intervals.RandomInRange;
		}

		public void Pain()
		{
			HediffUtility.TryAddOrRemoveHediff(Props.hediffDef, pawn, this, Props.bodyparts, true);
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: ChronicPains",
					action = delegate
					{
						Pain();
					}
				};
			}
		}

	}

}
