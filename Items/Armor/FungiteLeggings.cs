using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class FungiteLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                ""
                );
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 72);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.FungiteBar>(), 25)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
