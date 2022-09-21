using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Placeable.Furniture
{
    public class YanagidakoStatue : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.YanagidakoStatueTile>());
            Item.width = 31;
            Item.height = 47;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.White;
            Item.value = Item.buyPrice(gold: 5);
        }
    }
}
