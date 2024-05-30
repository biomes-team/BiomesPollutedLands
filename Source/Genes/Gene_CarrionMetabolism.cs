using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace BMT_PollutedLands
{

	public class Gene_CarrionMetabolism : Gene
	{

		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: TryHuntForFood",
					action = delegate
					{
						if (!TryHuntForFood())
						{
							Log.Error("Target is null");
						}
					}
				};
			}
		}

		public override void Tick()
		{
			base.Tick();
			if (!pawn.IsHashIntervalTick(21212))
			{
				return;
			}
			Need_Food food = pawn?.needs?.food;
			if (food == null)
			{
				return;
			}
			if (food.CurLevelPercentage >= pawn.RaceProps.FoodLevelPercentageWantEat * 0.10f)
			{
				return;
			}
			if (pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (pawn.Map == null)
			{
				// In caravan use
				// InCaravan();
				return;
			}
			if (pawn.Downed || pawn.Drafted)
			{
				return;
			}
			TryHuntForFood();
		}

		public virtual bool TryHuntForFood()
		{
			List<Thing> targets = pawn.Map.listerThings.AllThings;
			targets.Shuffle();
			// =
			foreach (Thing thing in targets)
			{
				if (thing is not Corpse corpse)
				{
					continue;
				}
				if (corpse.IsForbidden(pawn) || !pawn.CanReserveAndReach(corpse, PathEndMode.OnCell, pawn.NormalMaxDanger()))
				{
					continue;
				}
				if (!PL_GeneUtility.TryGetAbilityJob(pawn, corpse, def.abilities.RandomElement(), out Job job))
				{
					continue;
				}
				if (!PawnHaveThisJob(pawn, job))
				{
					pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc, true);
					// Log.Error("Target is " + thing.LabelCap);
					return true;
				}
				return false;
			}
			return false;
		}

		public static bool PawnHaveThisJob(Pawn pawn, Job job)
		{
			foreach (Job item in pawn.jobs.AllJobs().ToList())
			{
				if (item.def == job.def && item.ability == job.ability)
				{
					return true;
				}
			}
			return false;
		}

		public override void Notify_IngestedThing(Thing thing, int numTaken)
		{
			base.Notify_IngestedThing(thing, numTaken);
			if (!Active)
			{
				return;
			}
			if (thing is Corpse)
			{
				OffsetNeedFood(pawn, 1f * def.resourceLossPerDay);
				return;
			}
			IngestibleProperties ingestible = thing.def.ingestible;
			if (ingestible != null)
			{
				return;
			}
			float nutrition = ingestible.CachedNutrition;
			if (nutrition > 0f)
			{
				OffsetNeedFood(pawn, (-1f * def.resourceLossPerDay) * nutrition * (float)numTaken);
			}
		}

		public static void OffsetNeedFood(Pawn pawn, float offset)
		{
			Need_Food need_Food = pawn.needs?.food;
			if (need_Food != null)
			{
				need_Food.CurLevel += need_Food.MaxLevel >= (need_Food.CurLevel + offset) ? offset : (need_Food.MaxLevel - need_Food.CurLevel);
			}
		}

	}

}
