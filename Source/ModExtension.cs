using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

// namespace WVC
namespace BMT_PollutedLands
{

	public class GeneExtension : DefModExtension
	{
		public HediffDef hediffDef;
		public List<HediffDef> hediffDefs;
		public List<BodyPartDef> bodyparts;
		public IntRange intervals = new(45000, 64000);
		public float chance = 0.21f;
		public float maxSeverity = 0.72f;
		public ThingDef filthDef;
	}

}
