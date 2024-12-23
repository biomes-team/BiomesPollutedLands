using RimWorld;
using RimWorld.QuestGen;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace BMT_PollutedLands
{

	public class GenStep_PollutedHives : GenStep
    {
        public ThingDef thingDef;
        public FactionDef factionDef;

        //private List<IntVec3> rockCells = new List<IntVec3>();

		private List<IntVec3> possibleSpawnCells = new List<IntVec3>();

		private List<Thing> spawnedHives = new();

        //private const int MinDistToOpenSpace = 10;

        //private const int MinDistFromFactionBase = 50;

        //private const float CaveCellsPerHive = 1000f;

        public bool ignoreTempMaps = true;

        public int pollutionNone = 0;

        public int pollutionLight = 1;

        public int pollutionModerate = 2;

        public int pollutionExtreme = 3;

        public override int SeedPart => 379641510;

		public override void Generate(Map map, GenStepParams parms)
        {
            if (ignoreTempMaps && map.IsTempIncidentMap)
            {
                return;
            }
            TryFindFaction(out Faction faction, factionDef);
			//Log.Error("0");
            if (faction == null)
            {
                return;
            }
            possibleSpawnCells = map.AllCells.Where((IntVec3 x) => x.Standable(map) && x.GetFirstItem(map) == null && x.GetFirstBuilding(map) == null && x.GetFirstPawn(map) == null).ToList();
            //Log.Error(possibleSpawnCells.Count.ToString());
            //Log.Error("1");
            spawnedHives.Clear();
            int hives = DesiredHives(map);
            //Log.Error("Desired hives: " + hives);
            for (int j = 0; j < hives; j++)
            {
                TrySpawnHive(map, faction);
            }
            spawnedHives.Clear();
        }

        private int DesiredHives(Map map)
        {
            return Find.WorldGrid[map.Tile].PollutionLevel() switch
            {
                PollutionLevel.None => pollutionNone,
                PollutionLevel.Light => pollutionLight,
                PollutionLevel.Moderate => pollutionModerate,
                PollutionLevel.Extreme => pollutionExtreme,
                _ => 1,
            };
        }

        private void TrySpawnHive(Map map, Faction faction)
        {
            //Log.Error("3");
            if (TryFindHiveSpawnCell(map, out var spawnCell))
            {
                //Log.Error("5");
                possibleSpawnCells.Remove(spawnCell);
				Thing hive = GenSpawn.Spawn(ThingMaker.MakeThing(thingDef), spawnCell, map);
				hive.SetFaction(faction);
				spawnedHives.Add(hive);
			}
        }

        private bool TryFindFaction(out Faction faction, FactionDef factionDef)
        {
            return Find.FactionManager.GetFactions(allowHidden: true).Where((Faction faction) => faction.def == factionDef).ToList().TryRandomElement(out faction);
        }

        private bool TryFindHiveSpawnCell(Map map, out IntVec3 spawnCell)
		{
			float num = -1f;
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < 3; i++)
			{
                //Log.Error("0");
                if (!possibleSpawnCells.TryRandomElement(out var result))
				{
					break;
                }
                //Log.Error("1");
                float num2 = -1f;
				for (int j = 0; j < spawnedHives.Count; j++)
				{
					float num3 = result.DistanceToSquared(spawnedHives[j].Position);
					if (num2 < 0f || num3 < num2)
					{
						num2 = num3;
					}
                }
                //Log.Error("2");
                if (!intVec.IsValid || num2 > num)
				{
					intVec = result;
					num = num2;
                }
                //Log.Error("3");
            }
            //Log.Error("4");
            spawnCell = intVec;
			return spawnCell.IsValid;
		}
	}

}
