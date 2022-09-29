using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Tiles.Terrain
{
    public abstract class Tree : ModTree
    {
		public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 11f / 72f,
			SpecialGroupMaximumHueValue = 0.25f,
			SpecialGroupMinimumSaturationValue = 0.88f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		protected virtual int[] GrowOn => new int[] { ItemID.DirtWall };

		public override void SetStaticDefaults()
		{
			GrowsOnTileId = GrowOn;
		}

		public override Asset<Texture2D> GetTexture()
		{
			return ModContent.Request<Texture2D>($"ExampleMod/Content/Tiles/Plants/{this.GetType().Name}");
		}

        /*
		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return ModContent.TileType<Plants.ExampleSapling>();
		}
		*/

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {

        }


        public override Asset<Texture2D> GetBranchTextures()
		{
			return ModContent.Request<Texture2D>($"ExampleMod/Content/Tiles/Plants/{this.GetType().Name}_Branches");
		}

		
		public override Asset<Texture2D> GetTopTextures()
		{
			return ModContent.Request<Texture2D>($"ExampleMod/Content/Tiles/Plants/{this.GetType().Name}_Tops");
		}

		public override int DropWood()
		{
			return ItemID.Wood;
		}

		public override int TreeLeaf()
		{
			return ItemID.BambooLeaf;
		}
	}
}
