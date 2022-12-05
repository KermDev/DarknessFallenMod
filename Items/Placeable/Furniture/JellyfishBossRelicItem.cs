using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Placeable.Furniture
{
    public class JellyfishBossRelicItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jellyfish Relic");
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.JellyfishBossRelicTile>());
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Master;
            Item.value = Item.buyPrice(gold: 5);
        }
    }
}
