using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Accessories
{
    public class EvilAmalgam : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("8% increased damage" +
                $"\n10% increased attack speed");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player Player, bool hideVisual)
        {
            Player.GetDamage(DamageClass.Generic) += 0.08f;
            Player.GetAttackSpeed(DamageClass.Generic) += 0.10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BloodyJaw>())
                .AddIngredient(ModContent.ItemType<CorruptedAntenna>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
