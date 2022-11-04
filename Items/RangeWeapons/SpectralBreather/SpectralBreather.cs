using DarknessFallenMod.Items.MagicWeapons;
using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.RangeWeapons.SpectralBreather
{
    public class SpectralBreather : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 43;
            Item.height = 43;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0;
            Item.value = 18764;
            Item.rare = 3;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SpectralBreatherProjectile>();
            Item.shootSpeed = 14f;
            Item.noMelee = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            DarknessFallenUtils.OffsetShootPos(ref position, velocity, Vector2.UnitX * 55);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextBool(18)) Gore.NewGore(source, position, velocity.RotatedByRandom(0.3f) * 0.2f, GoreID.Smoke1 + Main.rand.Next(3), Main.rand.NextFloat(0.5f, 0.8f));
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void UseItemFrame(Player player)
        {
            player.itemLocation += Vector2.UnitX.RotatedBy(player.itemRotation) * -10 * player.direction;
        }
    }
}
