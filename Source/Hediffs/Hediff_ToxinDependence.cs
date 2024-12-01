using System.Linq;
using RimWorld;
using Verse;

namespace BMT_PollutedLands;

public class Hediff_ToxinDependence : HediffWithComps
{
    public Need_ToxinDependence Need
    {
        get
        {
            if (this.pawn.Dead) return null;
            return this.pawn.needs.AllNeeds
                .Where(n => n.def == this.def.causesNeed)
                .Cast<Need_ToxinDependence>()
                .FirstOrDefault();
        }
    }

    public override string LabelInBrackets
    {
        get
        {
            var labelInBrackets = base.LabelInBrackets;
            var stringPercent = (1f - this.Severity).ToStringPercent("F0");
            if (this.def.CompProps<HediffCompProperties_SeverityPerDay>() == null) return labelInBrackets;
            return !labelInBrackets.NullOrEmpty() ? labelInBrackets + " " + stringPercent : stringPercent;
        }
    }

    public override string TipStringExtra
    {
        get
        {
            var str = base.TipStringExtra;

            var need = this.Need;
            if (need != null)
            {
                if (!str.NullOrEmpty()) str += "\n";
                str += "CreatesNeed".Translate() + ": " + need.LabelCap + " (" + need.CurLevelPercentage.ToStringPercent("F0") + ")";
            }

            return str;
        }
    }

    public override int CurStageIndex
    {
        get
        {
            var need = this.Need;
            return need is not { CurCategory: DrugDesireCategory.Withdrawal } ? 0 : 1;
        }
    }

    public void Notify_NeedCategoryChanged()
    {
        this.pawn.health.Notify_HediffChanged(this);
    }
}
