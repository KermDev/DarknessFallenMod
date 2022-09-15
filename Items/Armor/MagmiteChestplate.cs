using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class MagmiteChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                "8% increased damage"
                );
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = 2000;
            Item.rare = ItemRarityID.Red;
            Item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.08f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.MagmiteBar>(), 20)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
