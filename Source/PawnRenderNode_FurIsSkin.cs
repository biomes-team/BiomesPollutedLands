using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using Verse;

namespace BMT_PollutedLands
{

	public class PawnRenderNode_FurIsSkin : PawnRenderNode
	{

		protected override Shader DefaultShader => ShaderDatabase.CutoutSkinOverlay;

		public PawnRenderNode_FurIsSkin(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
			: base(pawn, props, tree)
		{
		}

		public override Graphic GraphicFor(Pawn pawn)
		{
			string bodyPath = TexPathFor(pawn);
			if (bodyPath == null)
			{
				return null;
			}
			GeneExtension modExtension = pawn?.story?.furDef?.GetModExtension<GeneExtension>();
			if (modExtension == null || modExtension.furIsSkin)
			{
				return GraphicDatabase.Get<Graphic_Multi>(bodyPath, ShaderFor(pawn), Vector2.one, ColorFor(pawn));
			}
			else if (modExtension.furIsSkinWithHair)
			{
				return GraphicDatabase.Get<Graphic_Multi>(bodyPath, ShaderDatabase.CutoutComplex, Vector2.one, pawn.story.SkinColor, pawn.story.HairColor);
			}
			return null;
		}

		// public Graphic DefaultGraphic(Pawn pawn, string bodyPath)
		// {
			// return GraphicDatabase.Get<Graphic_Multi>(bodyPath, ShaderFor(pawn), Vector2.one, ColorFor(pawn));
		// }

		protected override string TexPathFor(Pawn pawn)
		{
			return pawn?.story?.furDef?.GetFurBodyGraphicPath(pawn);
		}

		public override Color ColorFor(Pawn pawn)
		{
			return pawn.story.SkinColor;
		}

	}


}
