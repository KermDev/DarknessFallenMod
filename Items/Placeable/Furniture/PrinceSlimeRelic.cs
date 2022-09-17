using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Placeable.Furniture
{
    public class PrinceSlimeRelic : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.PrinceSlimeRelicTile>());
            Item.width = 30;
            Item.height = 40;
            Item.maxStack = 99;
            Item.rare = -13;
            Item.master = true;
            Item.value = Item.buyPrice(gold: 5);
        }
    }
}
