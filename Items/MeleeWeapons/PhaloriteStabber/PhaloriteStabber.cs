using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Items.Materials;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace DarknessFallenMod.Items.MeleeWeapons.PhaloriteStabber
{
    public class PhaloriteStabber : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phalorite Stabber");
            Tooltip.SetDefault("The shortsword crafted from Phalorite Bars");
        }

        public override void SetDefaults()
        {
            Item.damage = 164;
            Item.DamageType = DamageClass.Melee;
            Item.width = 36;
            Item.height = 36;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.knockBack = 8;
            Item.value = 76320;
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PhaloriteStabberProjectile>();
            Item.shootSpeed = 9.5f;

            Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
            Item.noMelee = true; // The projectile will do the damage and not the item

            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 10);

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.shootSpeed = 1.5f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
        }

        /*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MeleeWeapons.PhaloriteSword.PhaloriteSwordProjectile>(), damage, knockback);
            return true;
        }*/


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<PhaloriteBar>(15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}