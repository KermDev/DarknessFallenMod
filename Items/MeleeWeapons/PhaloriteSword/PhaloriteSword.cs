using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Items.Materials;
using System;
using Microsoft.Xna.Framework;

namespace DarknessFallenMod.Items.MeleeWeapons.PhaloriteSword
{
    public class PhaloriteSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phalorite Sword");
            Tooltip.SetDefault("The sword crafted from Phalorite Bars");
        }

        public override void SetDefaults()
        {
            Item.damage = 190;
            Item.DamageType = DamageClass.Melee;
            Item.width = 62;
            Item.height = 62;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = 1;
            Item.knockBack = 8;
            Item.value = 76320;
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PhaloriteSwordProjectile>();
            Item.shootSpeed = 9.5f;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<PhaloriteBar>(25);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}