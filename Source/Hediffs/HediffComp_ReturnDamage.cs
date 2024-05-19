using System;
using System.Collections.Generic;
using Verse;

namespace BMT_PollutedLands
{

	public class HediffCompProperties_ReturnDamage : HediffCompProperties
	{

		public int damageAmount = 8;

		public float armorPenetration = 0f;

		public DamageDef damageDef;

		public HediffCompProperties_ReturnDamage()
		{
			compClass = typeof(HediffComp_ReturnDamage);
		}

	}

	public class HediffComp_ReturnDamage : HediffComp
	{

		public HediffCompProperties_ReturnDamage Props => (HediffCompProperties_ReturnDamage)props;

		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (dinfo.Instigator != null && dinfo.Instigator is Pawn instigator && !dinfo.Def.isRanged)
			{
				DamageInfo damageInfo = new(Props.damageDef, Props.damageAmount, Props.armorPenetration, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, false, false);
				// damageInfo.SetAllowDamagePropagation(false);
				instigator.TakeDamage(damageInfo);
			}
		}

	}

}
