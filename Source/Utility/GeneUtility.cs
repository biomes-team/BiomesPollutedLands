using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace BMT_PollutedLands
{

	public static class PL_GeneUtility
	{

		public static bool TrySpawnFilth(Pawn victim, IntRange bloodFilthToSpawnRange, ThingDef filthDef)
		{
			if (victim?.Map == null)
			{
				return false;
			}
			int randomInRange = bloodFilthToSpawnRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 c = victim.Position;
				if (randomInRange > 1 && Rand.Chance(0.8888f))
				{
					c = victim.Position.RandomAdjacentCell8Way();
				}
				if (c.InBounds(victim.MapHeld))
				{
					FilthMaker.TryMakeFilth(c, victim.MapHeld, filthDef, victim.LabelShort);
				}
			}
			return true;
		}

		// Non-anomaly regen. With custom ticker
		// Regeneration
		public static void Regeneration(Pawn pawn, float regeneration = -1, bool ignoreScarification = true, int tick = 10)
		{
			List<Hediff_Injury> tmpHediffInjuries = new();
			List<Hediff_MissingPart> tmpHediffMissing = new();
			regeneration *= 0.000166666665f;
			// Anomaly regen ticks once every 10 ticks
			if (tick > 0f)
			{
				regeneration *= (tick / 10);
			}
			if (regeneration > 0f)
			{
				pawn.health.hediffSet.GetHediffs(ref tmpHediffInjuries, (Hediff_Injury h) => h.def != HediffDefOf.Scarification || !ignoreScarification);
				foreach (Hediff_Injury tmpHediffInjury in tmpHediffInjuries)
				{
					float num5 = Mathf.Min(regeneration, tmpHediffInjury.Severity);
					regeneration -= num5;
					tmpHediffInjury.Heal(num5);
					pawn.health.hediffSet.Notify_Regenerated(num5);
					if (regeneration <= 0f)
					{
						break;
					}
				}
				if (regeneration > 0f)
				{
					pawn.health.hediffSet.GetHediffs(ref tmpHediffMissing, (Hediff_MissingPart h) => h.Part.parent != null && !tmpHediffInjuries.Any((Hediff_Injury x) => x.Part == h.Part.parent) && pawn.health.hediffSet.GetFirstHediffMatchingPart<Hediff_MissingPart>(h.Part.parent) == null && pawn.health.hediffSet.GetFirstHediffMatchingPart<Hediff_AddedPart>(h.Part.parent) == null);
					using List<Hediff_MissingPart>.Enumerator enumerator3 = tmpHediffMissing.GetEnumerator();
					if (enumerator3.MoveNext())
					{
						Hediff_MissingPart current4 = enumerator3.Current;
						BodyPartRecord part = current4.Part;
						pawn.health.RemoveHediff(current4);
						Hediff hediff2 = pawn.health.AddHediff(HediffDefOf.Misc, part);
						float partHealth = pawn.health.hediffSet.GetPartHealth(part);
						hediff2.Severity = Mathf.Max(partHealth - 1f, partHealth * 0.9f);
						pawn.health.hediffSet.Notify_Regenerated(partHealth - hediff2.Severity);
					}
				}
			}
		}

		public static bool ReplaceGeneBackground(GeneDef geneDef)
		{
			if (geneDef.IsPollutedDef())
			{
				return true;
			}
			// if (geneDef.GetModExtension<GeneExtension>() != null)
			// {
				// return true;
			// }
			return false;
		}

		// Sergkart: A solution that uses VEF always patches background genes, regardless of the use of custom backks. I tried different workarounds, nothing worked, VEF always overwrote back.
		// Sergkart: This solution is as dumb as a wooden stick (poor 54%), but it guarantees a bypass of the VEF patch and opens up access to easier changes to some interface things related to genes.

		public static void DrawGeneBasics(GeneDef gene, Rect geneRect, GeneType geneType, bool doBackground, bool clickable, bool overridden)
		{
			GUI.BeginGroup(geneRect);
			Rect rect = geneRect.AtZero();
			if (doBackground)
			{
				Widgets.DrawHighlight(rect);
				GUI.color = new(1f, 1f, 1f, 0.05f);
				Widgets.DrawBox(rect);
				GUI.color = Color.white;
			}
			float num = rect.width - Text.LineHeight;
			Rect rect2 = new(geneRect.width / 2f - num / 2f, 0f, num, num);
			Color iconColor = gene.IconColor;
			if (overridden)
			{
				iconColor.a = 0.75f;
				GUI.color = ColoredText.SubtleGrayColor;
			}
			// Here we can insert any custom backgrounds. For example, in my mod I use different backgrounds for endo and xeno archites.
			// CachedTexture cachedTexture = BackgroundTexture(gene, geneType);
			CachedTexture cachedTexture = new("BMT_PollutedLands/UI/Icons/Genes/MutageneBackground");
			GUI.DrawTexture(rect2, cachedTexture.Texture);
			Widgets.DefIcon(rect2, gene, null, 0.9f, null, drawPlaceholder: false, iconColor);
			Text.Font = GameFont.Tiny;
			float num2 = Text.CalcHeight(gene.LabelCap, rect.width);
			Rect rect3 = new(0f, rect.yMax - num2, rect.width, num2);
			GUI.DrawTexture(new(rect3.x, rect3.yMax - num2, rect3.width, num2), TexUI.GrayTextBG);
			Text.Anchor = TextAnchor.LowerCenter;
			if (overridden)
			{
				GUI.color = ColoredText.SubtleGrayColor;
			}
			if (doBackground && num2 < (Text.LineHeight - 2f) * 2f)
			{
				rect3.y -= 3f;
			}
			Widgets.Label(rect3, gene.LabelCap);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			if (clickable)
			{
				if (Widgets.ButtonInvisible(rect))
				{
					Find.WindowStack.Add(new Dialog_InfoCard(gene));
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
			}
			GUI.EndGroup();
		}

		public static bool IsPollutedDef(this Def def)
		{
			return def?.modContentPack != null && def.modContentPack.PackageId.Contains("biomesteam.biomespollutedlands");
		}

	}
}
