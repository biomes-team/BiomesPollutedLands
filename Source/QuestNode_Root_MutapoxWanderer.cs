using RimWorld.QuestGen;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld.Planet;

namespace BMT_PollutedLands
{
    public class QuestNode_Root_MutapoxWanderer : QuestNode_Root_WandererJoin
    {
        private const int TimeoutTicks = 60000;
        public const float RelationWithColonistWeight = 20f;
        private string signalAccept;
        private string signalReject;

        protected override void RunInt()
        {
            base.RunInt();
            Quest quest = RimWorld.QuestGen.QuestGen.quest;
            quest.Delay(60000, (Action)(() => QuestGen_End.End(quest, QuestEndOutcome.Fail)));
        }

        public override Pawn GeneratePawn()
        {
            Slate slate = RimWorld.QuestGen.QuestGen.slate;
            Gender? fixedGender = new Gender?();
            PawnGenerationRequest request;
            if (!slate.TryGet<PawnGenerationRequest>("overridePawnGenParams", out request))
            {
                request = new PawnGenerationRequest(PawnKindDefOf.Villager, forceGenerateNewPawn: true, colonistRelationChanceFactor: 20f, allowPregnant: true, fixedGender: fixedGender, forceRecruitable: true);
            }

            if (Find.Storyteller.difficulty.ChildrenAllowed)
            {
                request.AllowedDevelopmentalStages |= DevelopmentalStage.Child;
            }

            Pawn pawn = PawnGenerator.GeneratePawn(request);
            pawn.health.AddHediff(BMT_HediffDefOf.BMT_Mutapox);
            if (!pawn.IsWorldPawn())
                Find.WorldPawns.PassToWorld(pawn);
            return pawn;
        }

        protected override void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
        {
            this.signalAccept = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
            this.signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
            quest.Signal(this.signalAccept, (Action)(() =>
            {
                quest.SetFaction((IEnumerable<Thing>)Gen.YieldSingle<Pawn>(pawn), Faction.OfPlayer);
                quest.PawnsArrive(Gen.YieldSingle<Pawn>(pawn), mapParent: map.Parent);
                QuestGen_End.End(quest, QuestEndOutcome.Success);
            }));
            quest.Signal(this.signalReject, (Action)(() =>
            {
                quest.GiveDiedOrDownedThoughts(pawn, PawnDiedOrDownedThoughtsKind.DeniedJoining);
                QuestGen_End.End(quest, QuestEndOutcome.Fail);
            }));
        }

        public override void SendLetter(Quest quest, Pawn pawn)
        {
            TaggedString title = "BMT_LetterLabelMutapoxJoins".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            TaggedString text = "BMT_LetterMutapoxJoins".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            QuestNode_Root_WandererJoin_WalkIn.AppendCharityInfoToLetter("JoinerCharityInfo".Translate((NamedArgument)(Thing)pawn), ref text);
            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref title, pawn);
            if (pawn.DevelopmentalStage.Juvenile())
            {
                string stringTicksToPeriod = (pawn.ageTracker.AgeBiologicalYears * 3600000).ToStringTicksToPeriod();
                text += "\n\n" + "RefugeePodCrash_Child".Translate(pawn.Named("PAWN"), stringTicksToPeriod.Named("AGE"));
            }
            ChoiceLetter_AcceptJoiner let = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(title, text, LetterDefOf.AcceptJoiner);
            let.signalAccept = this.signalAccept;
            let.signalReject = this.signalReject;
            let.quest = quest;
            let.StartTimeout(60000);
            Find.LetterStack.ReceiveLetter((Letter)let);
        }

        public static void AppendCharityInfoToLetter(
          TaggedString charityInfo,
          ref TaggedString letterText)
        {
            if (!ModsConfig.IdeologyActive)
                return;
            IEnumerable<Pawn> source1 = IdeoUtility.AllColonistsWithCharityPrecept();
            if (!source1.Any<Pawn>())
                return;
            letterText += "\n\n" + charityInfo + "\n\n" + "PawnsHaveCharitableBeliefs".Translate() + ":";
            foreach (IGrouping<Ideo, Pawn> source2 in source1.GroupBy<Pawn, Ideo>((Func<Pawn, Ideo>)(c => c.Ideo)))
                letterText += "\n  - " + "BelieversIn".Translate((NamedArgument)source2.Key.name.Colorize(source2.Key.TextColor), (NamedArgument)source2.Select<Pawn, string>((Func<Pawn, string>)(f => f.NameShortColored.Resolve())).ToCommaList());
        }
    }
}
