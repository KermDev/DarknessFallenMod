using DarknessFallenMod.Core;
using DarknessFallenMod.Items.MagicWeapons;
using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.RangeWeapons.SpectralBreather
{
    public class SpectralBreather : ModItem, IGlowmask
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Breaths purple and orange flames".GetColored(Color.BlueViolet));
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 43;
            Item.height = 43;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0;
            Item.value = 18764;
            Item.rare = ModContent.RarityType<SpectralBreatherRarity>();
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SpectralBreatherProjectile>();
            Item.shootSpeed = 18f;
            Item.noMelee = true;
            Item.channel = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            DarknessFallenUtils.OffsetShootPos(ref position, velocity, Vector2.UnitX * 55, true);
        }

        /*
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "DisplayName")
            {
                DarknessFallenUtils.DrawTooltipLineEffect(line, line.X, line.Y, DarknessFallenUtils.TooltipLineEffectStyle.Epileptic);
            }

            return base.PreDrawTooltipLine(line, ref yOffset);
        }
        */

        int soundTimer;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (soundTimer-- < 0)
            {
                SoundEngine.PlaySound(SoundID.Item100, position);
                soundTimer = 30;
            }

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            if (Main.rand.NextBool(4))
            {
                Vector2 vel1 = velocity.RotatedByRandom(0.1f) * 0.2f;
                Dust.NewDustDirect(position, 0, 0, DustID.InfernoFork, vel1.X, vel1.Y, Scale: Main.rand.NextFloat() * 2).noGravity = true;

                Dust.NewDust(position, 0, 0, DustID.Smoke, 0, -4, Scale: Main.rand.NextFloat());
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void UseItemFrame(Player player)
        {
            player.itemLocation += Vector2.UnitX.RotatedBy(player.itemRotation) * -10 * player.direction;
        }
    }
}
