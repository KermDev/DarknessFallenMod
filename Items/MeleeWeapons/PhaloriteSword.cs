using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Items.Materials;
using DarknessFallenMod.Items.MeleeWeapons;
using System;
using Microsoft.Xna.Framework;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    public class PhaloriteSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phalorite Sword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("The sword crafted from Phalorite Bars");
        }

        public override void SetDefaults()
        {
            Item.damage = 190;
            Item.DamageType = DamageClass.Melee;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = 1;
            Item.knockBack = 8;
            Item.value = 76320;
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PhaloriteSwordProjectile>();
            Item.shootSpeed = 21f;
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