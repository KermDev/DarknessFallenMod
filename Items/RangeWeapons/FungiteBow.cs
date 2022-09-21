using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;

namespace DarknessFallenMod.Items.RangeWeapons
{
    public class FungiteBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
			Item.damage = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5;
			Item.value = 34320;
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
		}

        public override void UseItemFrame(Player player)
        {
            player.itemLocation += Vector2.UnitX.RotatedBy(player.itemRotation) * -8 * player.direction;
        }
    }

	public class FungiteArrowGlobalItem : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        bool shotFromFungiteBow;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource)
            {
                if (itemSource.Item.type == ModContent.ItemType<FungiteBow>())
                {
                    shotFromFungiteBow = true;
                }
            }
        }

        public override void AI(Projectile projectile)
        {
            if (shotFromFungiteBow && Main.rand.NextBool(3)) Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GlowingMushroom);
        }
    }
}
