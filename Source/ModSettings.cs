using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using Verse;

namespace BMT_PollutedLands
{

	public class BMT_PollutedLandsSettings : ModSettings
	{

		public bool disableUniqueGeneInterface = false;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref disableUniqueGeneInterface, "disableUniqueGeneInterface", defaultValue: false);
		}
	}

	public class BMT_PollutedLands : Mod
	{
		public static BMT_PollutedLandsSettings settings;

		private int PageIndex = 0;

		private static Vector2 scrollPosition = Vector2.zero;

		public BMT_PollutedLands(ModContentPack content) : base(content)
		{
			settings = GetSettings<BMT_PollutedLandsSettings>();
            HarmonyPatches.HarmonyUtility.PostInitialPatches();
        }

		public override void DoSettingsWindowContents(Rect inRect)
		{
			Rect rect = new(inRect);
			rect.y = inRect.y + 40f;
			rect = new Rect(inRect)
			{
				height = inRect.height - 40f,
				y = inRect.y + 40f
			};
			Widgets.DrawMenuSection(rect);
			List<TabRecord> tabs = new()
			{
				new TabRecord("BMT_PollutedLandsSettings_Tab_General".Translate(), delegate
				{
					PageIndex = 0;
					WriteSettings();
				}, PageIndex == 0),
			};
			TabDrawer.DrawTabs(rect, tabs);
			switch (PageIndex)
			{
				case 0:
					GeneralSettings(rect.ContractedBy(15f));
					break;
			}
		}

		public override string SettingsCategory()
		{
			return "BMT_PollutedLandsSettings".Translate();
		}

		private void GeneralSettings(Rect inRect)
		{
			Rect outRect = new(inRect.x, inRect.y, inRect.width, inRect.height);
			Rect rect = new(0f, 0f, inRect.width - 30f, inRect.height * 2.0f);
			Widgets.BeginScrollView(outRect, ref scrollPosition, rect);
			Listing_Standard listingStandard = new();
			listingStandard.Begin(rect);
			listingStandard.CheckboxLabeled("BMT_PL_Label_disableUniqueGeneInterface".Translate(), ref settings.disableUniqueGeneInterface, "BMT_PL_ToolTip_disableUniqueGeneInterface".Translate());
			listingStandard.End();
			Widgets.EndScrollView();
		}

	}

}
