using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace BMT_PollutedLands
{

	public class CompProperties_AbilityHideIfPawnMultiSelected : CompProperties_AbilityEffect
	{

		public CompProperties_AbilityHideIfPawnMultiSelected()
		{
			compClass = typeof(CompAbilityEffect_HideIfPawnMultiSelected);
		}

	}

	public class CompAbilityEffect_HideIfPawnMultiSelected : CompAbilityEffect
	{

		public override bool ShouldHideGizmo => Find.Selector.SelectedPawns.Count > 1;

	}

}
