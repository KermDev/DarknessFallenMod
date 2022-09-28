using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Placeable.Terrain
{
    public class EmptyCobble : ModItem
    {
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Terrain.EmptyCobbleTile>();
			Item.width = 15;
			Item.height = 15;
			Item.value = Item.buyPrice(silver: 8);
			Item.rare = ItemRarityID.Gray;
		}
	}
}
