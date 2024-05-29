using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BMT_PollutedLands
{

	public class HediffCompProperties_OverdevelopedOrgan : HediffCompProperties
	{

		public float hp_percent = 0.5f;

		public float absorb_percent = 0.4f;

		public float absorb_chance = 0.25f;

		public float hp_perDay = 3f;

		// public DamageDef damageDef;

		public HediffCompProperties_OverdevelopedOrgan()
		{
			compClass = typeof(HediffComp_OverdevelopedOrgan);
		}

	}

	public class HediffComp_OverdevelopedOrgan : HediffComp
	{

		public HediffCompProperties_OverdevelopedOrgan Props => (HediffCompProperties_OverdevelopedOrgan)props;

		private float partMaxHP;

		private float partCurrentHP;

		// private float cachedPartHPBeforeHit;

		private List<Hediff_Injury> tmpHediffInjuries = new();

		public override string CompLabelInBracketsExtra => GetLabel();

		public string GetLabel()
		{
			if (partCurrentHP > 0f)
			{
				// return "BMT_PL_OverdevelopedOrganHP".Translate() + " " + ((int)partCurrentHP).ToString() + " / " + ((int)partMaxHP).ToString();
				return ((int)partCurrentHP).ToString() + "/" + ((int)partMaxHP).ToString();
			}
			return null;
		}

		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (parent.Part == null)
			{
				Pawn.health.RemoveHediff(parent);
				return;
			}
			partMaxHP = parent.Part.def.hitPoints * Props.hp_percent;
			partCurrentHP = partMaxHP;
		}

		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			// Log.Error("Damage to part " + dinfo.HitPart.LabelCap);
			if (parent.Part == null)
			{
				return;
			}
			// if (dinfo.HitPart == parent.Part)
			// {
			// }
			if (partCurrentHP > 0f && Rand.Chance(Props.absorb_chance))
			{
				float absorb = totalDamageDealt * Props.absorb_percent;
				partCurrentHP -= absorb;
				// Pawn.health.hediffSet.GetHediffs(ref tmpHediffInjuries, (Hediff_Injury h) => h.Part == parent.Part && !h.IsPermanent());
				Pawn.health.hediffSet.GetHediffs(ref tmpHediffInjuries, (Hediff_Injury h) => !h.IsPermanent());
				foreach (Hediff_Injury tmpHediffInjury in tmpHediffInjuries)
				{
					float num5 = Mathf.Min(absorb, tmpHediffInjury.Severity);
					absorb -= num5;
					tmpHediffInjury.Heal(num5);
					Pawn.health.hediffSet.Notify_Regenerated(num5);
					if (absorb <= 0f)
					{
						break;
					}
				}
			}
		}

		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Pawn.IsHashIntervalTick(13626) && partMaxHP > partCurrentHP)
			{
				partCurrentHP += (Props.hp_perDay / 60000) * 13626;
			}
		}

		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look(ref partMaxHP, "partMaxHP", 0);
			Scribe_Values.Look(ref partCurrentHP, "partCurrentHP", 0);
		}

	}

}
