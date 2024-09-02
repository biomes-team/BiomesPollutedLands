using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace BMT_PollutedLands
{

	public class JobDriver_BloodVomit : JobDriver_Vomit
	{

		public int vomitTicksLeft;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref vomitTicksLeft, "vomitTicksLeft", 0);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate
			{
				vomitTicksLeft = Rand.Range(300, 900);
				int num = 0;
				IntVec3 intVec;
				do
				{
					intVec = pawn.Position + GenAdj.AdjacentCellsAndInside[Rand.Range(0, 9)];
					num++;
					if (num > 12)
					{
						intVec = pawn.Position;
						break;
					}
				}
				while (!intVec.InBounds(pawn.Map) || !intVec.Standable(pawn.Map));
				job.targetA = intVec;
				pawn.pather.StopDead();
			};
			toil.tickAction = delegate
			{
				if (vomitTicksLeft % 150 == 149)
				{
					FilthMaker.TryMakeFilth(job.targetA.Cell, base.Map, pawn.RaceProps.BloodDef, pawn.LabelIndefinite());
					Need_Food need_Food = pawn.needs?.TryGetNeed<Need_Food>();
					if (need_Food != null && need_Food.CurLevelPercentage > 0.1f)
					{
						need_Food.CurLevel -= need_Food.MaxLevel * 0.04f;
					}
				}
				vomitTicksLeft--;
				if (vomitTicksLeft <= 0)
				{
					ReadyForNextToil();
					TaleRecorder.RecordTale(TaleDefOf.Vomited, pawn);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.WithEffect(PL_GenesDefOf.BMT_BloodVomitEffect, TargetIndex.A);
			toil.PlaySustainerOrSound(() => SoundDefOf.Vomit);
			yield return toil;
		}
	}

}
