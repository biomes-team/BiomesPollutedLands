using Verse;

namespace BMT_PollutedLands;

public class HediffComp_SeverityChangeWhenWet : HediffComp_SeverityModifierBase
{
    private HediffCompProperties_SeverityChangeWhenWet Props =>
        (HediffCompProperties_SeverityChangeWhenWet)props;

    public override float SeverityChangePerDay()
    {
        var changePerDay = Props.baseChangePerDay;

        if (Pawn.IsWet())
        {
            changePerDay += Props.isWetChangePerDay;
        }

        return changePerDay;
    }
}

public class HediffCompProperties_SeverityChangeWhenWet : HediffCompProperties
{
    public float isWetChangePerDay = -1f;
    public float baseChangePerDay = 0;


    public HediffCompProperties_SeverityChangeWhenWet()
    {
        compClass = typeof(HediffComp_SeverityChangeWhenWet);
    }
}