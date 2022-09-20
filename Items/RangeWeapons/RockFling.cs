using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.RangeWeapons
{
    public class RockFling : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rockfling");
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 2;

			Item.width = 29;
			Item.height = 29;

			Item.useTime = 45;
			Item.useAnimation = 45;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 1025;
			Item.rare = ItemRarityID.Gray;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.Boulder;
			Item.shootSpeed = 10f;
			Item.noMelee = true;
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			position.Y -= 35;
			velocity = position.DirectionTo(Main.MouseWorld) * Item.shootSpeed;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			proj.friendly = true;
			proj.hostile = false;
			proj.DamageType = DamageClass.Ranged;
			proj.netUpdate = true;

            return false;
        }
    }
}
