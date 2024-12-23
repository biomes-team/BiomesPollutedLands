using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BMT_PollutedLands
{
    public class CompDisableComplexInsectSpawn : ThingComp
    {
    }

    public class CompProperties_DisableComplexInsectSpawn : CompProperties
    {
        public CompProperties_DisableComplexInsectSpawn()
        {
            compClass = typeof(CompDisableComplexInsectSpawn);
        }
    }
}
