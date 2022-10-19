using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Armor.Magmite
{
    [AutoloadEquip(EquipType.Legs)]
    public class MagmiteLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                "8% increased damage" +
                "\n5% increased movement speed"
                );
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = 2000;
            Item.rare = ItemRarityID.Red;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.08f;
            player.moveSpeed *= 1.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.MagmiteBar>(), 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
