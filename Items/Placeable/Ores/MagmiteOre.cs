using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Placeable.Ores
{
    public class MagmiteOre : ModItem
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
			Item.createTile = ModContent.TileType<Tiles.Ores.MagmiteOreTile>();
			Item.width = 15;
			Item.height = 15;
			Item.value = Item.buyPrice(silver: 16);
		}

		public override bool? CanBurnInLava() => false;
    }
}
