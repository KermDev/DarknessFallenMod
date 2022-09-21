using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Tools
{
    public class FungiteAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fungite Axe");
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.scale = 1f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.autoReuse = true;
            Item.axe = 59;
            Item.useTurn = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 11;
            Item.knockBack = 5f;
            Item.crit = 4;

            Item.value = Item.buyPrice(silver: 44);
            Item.rare = ItemRarityID.LightPurple;

            Item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<Materials.FungiteBar>(12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
