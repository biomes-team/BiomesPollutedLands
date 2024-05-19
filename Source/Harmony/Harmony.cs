using HarmonyLib;
using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace BMT_PollutedLands
{

	public class BMT_PollutedLands_Main : Mod
	{
		public BMT_PollutedLands_Main(ModContentPack content)
			: base(content)
		{
			new Harmony("biomesteam.biomespollutedlands").PatchAll();
		}
	}

	namespace HarmonyPatches
	{

		public static class HarmonyUtility
		{

			public static void PostInitialPatches()
			{
				var harmony = new Harmony("biomesteam.biomespollutedlands");
				// Background can be disabled
				// This is necessary in case there are conflicts with other mods
				if (!BMT_PollutedLands.settings.disableUniqueGeneInterface)
				{
					harmony.Patch(AccessTools.Method(typeof(GeneUIUtility), "DrawGene"), prefix: new HarmonyMethod(typeof(HarmonyUtility).GetMethod("PL_DrawGene")));
					harmony.Patch(AccessTools.Method(typeof(GeneUIUtility), "DrawGeneDef"), prefix: new HarmonyMethod(typeof(HarmonyUtility).GetMethod("PL_DrawGeneDef")));
				}
			}

			// Backgroud

			// This is a very cumbersome solution, but it allows us to bypass the VEF background patch and, if necessary, add custom information to genes.

			public static bool PL_DrawGene(ref Gene gene, ref Rect geneRect, ref GeneType geneType, ref bool doBackground, ref bool clickable)
			{
				if (PL_GeneUtility.ReplaceGeneBackground(gene.def))
				{
					PL_GeneUtility.DrawGeneBasics(gene.def, geneRect, geneType, doBackground, clickable, !gene.Active);
					if (Mouse.IsOver(geneRect))
					{
						string text = gene.LabelCap.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + gene.def.DescriptionFull;
						// text += PL_GeneUtility.AdditionalInfo_Gene(gene);
						// text += PL_GeneUtility.AdditionalInfo_GeneDef(gene.def);
						if (gene.Overridden)
						{
							text += "\n\n";
							text = ((gene.overriddenByGene.def != gene.def) ? (text + ("OverriddenByGene".Translate() + ": " + gene.overriddenByGene.LabelCap).Colorize(ColorLibrary.RedReadable)) : (text + ("OverriddenByIdenticalGene".Translate() + ": " + gene.overriddenByGene.LabelCap).Colorize(ColorLibrary.RedReadable)));
						}
						// Useful for debug
						if (Prefs.DevMode)
						{
							text += "\n\n DevMode:".Colorize(ColoredText.TipSectionTitleColor);
							text += "\n - defName: " + gene.def.defName.ToString();
							text += "\n - geneClass: " + gene.GetType().ToString();
						}
						if (clickable)
						{
							text += "\n\n" + "ClickForMoreInfo".Translate().ToString().Colorize(ColoredText.SubtleGrayColor);
						}
						TooltipHandler.TipRegion(geneRect, text);
					}
					return false;
				}
				return true;
			}

			public static bool PL_DrawGeneDef(ref GeneDef gene, ref Rect geneRect, ref GeneType geneType, ref Func<string> extraTooltip, ref bool doBackground, ref bool clickable, ref bool overridden)
			{
				if (PL_GeneUtility.ReplaceGeneBackground(gene))
				{
					PL_GeneUtility.DrawGeneBasics(gene, geneRect, geneType, doBackground, clickable, overridden);
					if (Mouse.IsOver(geneRect))
					{
						string text = gene.LabelCap.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + gene.DescriptionFull;
						// text += PL_GeneUtility.AdditionalInfo_GeneDef(gene);
						if (extraTooltip != null)
						{
							string text2 = extraTooltip();
							if (!text2.NullOrEmpty())
							{
								text = text + "\n\n" + text2.Colorize(ColorLibrary.RedReadable);
							}
						}
						if (clickable)
						{
							text += "\n\n" + "ClickForMoreInfo".Translate().ToString().Colorize(ColoredText.SubtleGrayColor);
						}
						TooltipHandler.TipRegion(geneRect, text);
					}
					return false;
				}
				return true;
			}

		}

	}

}
