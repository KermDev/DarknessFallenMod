using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Items.Materials;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.Items.MeleeWeapons.UmbralEdge
{
    public class UmbralEdge : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The sword powered by Darkness ");
        }

        public override void SetDefaults()
        {
            Item.damage = 94;
            Item.DamageType = DamageClass.Melee;
            Item.width = 2;
            Item.height = 2;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = 1;
            Item.knockBack = 5;
            Item.value = 4300;
            Item.rare = 0;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<UmbralEdgeProjectile>();
            Item.shootSpeed = 20;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            float speed = Math.Clamp(Main.MouseWorld.DistanceSQ(player.Center) * 0.0004f, 6f, Item.shootSpeed);

            velocity.Normalize();
            velocity *= speed;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int fartType = ModContent.ProjectileType<UmbralEdgeFart>();

            Projectile.NewProjectile(source, position, velocity, fartType, 10, 0, player.whoAmI);
            return true;
        }

        /*
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient <SoulOfDestruction> (5); 
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}*/
    }
}