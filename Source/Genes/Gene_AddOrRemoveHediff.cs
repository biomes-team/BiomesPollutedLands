using System;
using System.Collections.Generic;
using Verse;

namespace BMT_PollutedLands
{

	public class Gene_AddOrRemoveHediff : Gene, IGeneOverridden
	{

		public GeneExtension Props => def.GetModExtension<GeneExtension>();

		public override void PostAdd()
		{
			base.PostAdd();
			Local_AddOrRemoveHediff();
		}

		public void Local_AddOrRemoveHediff()
		{
			HediffUtility.TryAddOrRemoveHediff(Props.hediffDef, pawn, this, Props.bodyparts);
		}

		public void Notify_OverriddenBy(Gene overriddenBy)
		{
			HediffUtility.TryRemoveHediff(Props.hediffDef, pawn);
		}

		public void Notify_Override()
		{
			HediffUtility.TryAddOrRemoveHediff(Props.hediffDef, pawn, this, Props.bodyparts);
		}

		public override void Tick()
		{
			base.Tick();
			if (!pawn.IsHashIntervalTick(67200))
			{
				return;
			}
			Local_AddOrRemoveHediff();
		}

		public override void PostRemove()
		{
			base.PostRemove();
			HediffUtility.TryRemoveHediff(Props.hediffDef, pawn);
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Add Or Remove Hediff",
					action = delegate
					{
						if (Active)
						{
							Local_AddOrRemoveHediff();
						}
					}
				};
			}
		}

	}

}
