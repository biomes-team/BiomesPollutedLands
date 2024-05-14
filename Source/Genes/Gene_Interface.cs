using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace BMT_PollutedLands
{

	public interface IGeneOverridden
	{

		void Notify_OverriddenBy(Gene overriddenBy);

		void Notify_Override();

	}

}
