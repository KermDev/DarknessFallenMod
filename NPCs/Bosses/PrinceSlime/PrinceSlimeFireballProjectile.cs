using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs.Bosses.PrinceSlime
{
    public class PrinceSlimeFireballProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Fireball;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Royal Fireball");

            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;

            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = false;
            Projectile.light = 0.4f;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
        }

        public const float GRAVITY = 0.4f;
        int dustTimer;
        public override void AI()
        {
            Projectile.velocity.Y += GRAVITY;
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (dustTimer > 4)
            {
                DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.InfernoFork, 4, speedFromCenter: 0, amount: 8).ForEach(d =>
                {
                    d.noGravity = true;
                    d.velocity -= Projectile.velocity * 0.2f;
                });

                dustTimer = 0;
            }

            dustTimer++;
        }

        public override void Kill(int timeLeft)
        {
            int damage = Projectile.damage;
            float rangeSQ = 6400;
            DarknessFallenUtils.ForeachNPCInRange(Projectile.Center, rangeSQ, npc =>
            {
                if (npc.friendly)
                {
                    npc.StrikeNPC(damage, Projectile.knockBack, Projectile.HitDirection(npc.Center));
                }
            });

            DarknessFallenUtils.ForeachPlayerInRange(Projectile.Center, rangeSQ, player =>
            {
                if (!player.immune)
                {
                    player.Hurt(PlayerDeathReason.ByProjectile(player.whoAmI, Projectile.whoAmI), damage, Projectile.HitDirection(player.Center));
                }
            });

            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.InfernoFork, 5, speedFromCenter: 8, amount: 24, dustVelocity: Projectile.velocity).ForEach(d =>
            {
                d.noGravity = true;
            });

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawAfterImage(prog => Color.White * 0.6f);
            Projectile.DrawProjectileInHBCenter(Color.White);
            return false;
        }
    }
}
