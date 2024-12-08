using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BMT_PollutedLands
{
    public class IncidentWorker_HungryLocusts : IncidentWorker
    {
        private const float PointsFactor = 1f;
        private const int AnimalsStayDurationMin = 60000;
        private const int AnimalsStayDurationMax = 120000;

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms))
                return false;
            Map target = (Map)parms.target;
            return RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 _, target, CellFinder.EdgeRoadChance_Animal);
        }
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            PawnKindDef animalKind = BMT_AnimalsDefOf.BMT_FamineLocust;
            IntVec3 result = parms.spawnCenter;
            if (!result.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out result, target, CellFinder.EdgeRoadChance_Animal))
                return false;
            List<Pawn> animals = AggressiveAnimalIncidentUtility.GenerateAnimals(animalKind, target.Tile, parms.points * PointsFactor, parms.pawnCount);
            Rot4 rot = Rot4.FromAngleFlat((target.Center - result).AngleFlat);
            for (int index = 0; index < animals.Count; ++index)
            {
                Pawn newThing = animals[index];
                QuestUtility.AddQuestTag((object)GenSpawn.Spawn((Thing)newThing, CellFinder.RandomClosewalkCellNear(result, target, 10), target, rot), parms.questTag);
                newThing.health.AddHediff(BMT_HediffDefOf.BMT_HungeringHediff);
                newThing.mindState.mentalStateHandler.TryStartMentalState(BMT_MentalStateDefOf.BMT_Hungering);
                newThing.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(AnimalsStayDurationMin, AnimalsStayDurationMax);
            }

            this.SendStandardLetter("BMT_LetterLabelHungryLocustsArrived".Translate(), "BMT_HungryLocustsArrived".Translate(), LetterDefOf.ThreatBig, parms, (LookTargets)(Thing)animals[0]);
            Find.TickManager.slower.SignalForceNormalSpeedShort();
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Important);
            return true;
        }
    }
}
