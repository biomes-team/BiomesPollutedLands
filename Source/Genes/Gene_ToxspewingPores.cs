using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace BMT_PollutedLands
{

	public class Gene_ToxspewingPores : Gene
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
			ToxGas();
			ResetNextTick();
		}

		public void ResetNextTick()
		{
			nextTick = Props.intervals.RandomInRange;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: ReleaseGas",
					action = delegate
					{
						ToxGas();
						ResetNextTick();
					}
				};
			}
		}

		public void ToxGas()
		{
			if (pawn.Map == null)
			{
				return;
			}
			for (int i = 0; i < 6; i++)
			{
				IntVec3 c = pawn.Position;
				if (6 > 1 && Rand.Chance(0.8888f))
				{
					c = pawn.Position.RandomAdjacentCell8Way();
				}
				if (c.InBounds(pawn.MapHeld))
				{
					GasUtility.AddGas(pawn.PositionHeld, pawn.MapHeld, GasType.ToxGas, 8);
				}
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref nextTick, "nextTick", -1);
		}

	}

}
