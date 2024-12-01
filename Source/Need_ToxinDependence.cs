using RimWorld;
using Verse;

namespace BMT_PollutedLands;

public class Need_ToxinDependence : Need
{
    private const float GainPerTick = 0.0001f;

    public override int GUIChangeArrow => -1;

    public DrugDesireCategory CurCategory
    {
        get
        {
            if (this.CurLevel > 0.1) return DrugDesireCategory.Satisfied;
            return this.CurLevel > 0.01 ? DrugDesireCategory.Desire : DrugDesireCategory.Withdrawal;
        }
    }

    public override float CurLevel
    {
        get => base.CurLevel;
        set
        {
            var curCategory = this.CurCategory;
            base.CurLevel = value;
            if (this.CurCategory == curCategory) return;
            this.CategoryChanged();
        }
    }

    public Hediff_ToxinDependence Hediff
    {
        get
        {
            foreach (var hediff in this.pawn.health.hediffSet.hediffs)
                if (hediff is Hediff_ToxinDependence dependence)
                    return dependence;

            return null;
        }
    }

    private float FallPerTick => this.def.fallPerDay / 60000f;

    public Need_ToxinDependence(Pawn pawn) : base(pawn)
    {
        this.threshPercents = [0.1f];
    }

    public override void SetInitialLevel()
    {
        this.CurLevelPercentage = Rand.Range(0.8f, 1f);
    }

    public override void NeedInterval()
    {
        if (this.IsFrozen) return;

        var toxicBuildup = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup);

        if (toxicBuildup != null || (pawn.Spawned && pawn.Position.IsPolluted(pawn.Map)))
        {
            this.CurLevel += GainPerTick * 150f;
        }
        else
        {
            this.CurLevel -= FallPerTick * 150f;
        }
    }

    private void CategoryChanged()
    {
        this.Hediff?.Notify_NeedCategoryChanged();
    }
}
