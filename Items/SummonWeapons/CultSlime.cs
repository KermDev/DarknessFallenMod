using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.SummonWeapons
{
    public class CultSlime : ModItem
    {
        public override void SetDefaults()
        {
			Item.damage = 12;
			Item.DamageType = DamageClass.Summon;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = 34320;
			Item.rare = 8;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CultSlimeMinion>();
			Item.shootSpeed = 0;
		}

		readonly int[] damageNumbers = new int[] { 6, 4, 2 };

        public override bool CanUseItem(Player player)
        {
			return player.ownedProjectileCounts[Item.shoot] == 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			foreach(int dmg in damageNumbers)
            {
				Projectile.NewProjectile(source, position, Vector2.Zero, type, dmg, knockback, player.whoAmI);
            }

            return false;
        }
    }
}
