using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Armor.Sandscale
{
    [AutoloadEquip(EquipType.Body)]
    public class SandscaleChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                ""
                );
        }

        public override void SetDefaults()
        {
            Item.width = 29;
            Item.height = 29;
            Item.value = Item.sellPrice(silver: 72);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SandstoneScales>(), 25)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
