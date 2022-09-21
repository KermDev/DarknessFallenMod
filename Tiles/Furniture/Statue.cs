using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ObjectData;

namespace DarknessFallenMod.Tiles.Furniture
{
    public abstract class Statue : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 0;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.InteractibleByNPCs[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleHorizontal = false;


			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			
			TileObjectData.addTile(Type);

			var name = CreateMapEntryName();
			name.SetDefault(MapName);
			AddMapEntry(new Color(66, 66, 66), name);
        }

		public virtual string MapName => GetType().Name;
		public virtual int StatueItem => 0;
		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, StatueItem);
	}
}
