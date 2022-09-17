using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Placeable.Furniture
{
    public class PrinceSlimeTrophy : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 31;
            Item.height = 31;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 50000;
            Item.rare = ItemRarityID.Green;
            Item.createTile = ModContent.TileType<Tiles.Furniture.PrinceSlimeTrophyTile>();
        }
    }
}
