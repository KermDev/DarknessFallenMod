using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Throwables
{
    public class GearsparkProjectile : SpearparkProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2; 
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1400;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (stopTileHit) return false;

            Projectile.frame = 1;

            SoundEngine.PlaySound(SoundID.Research, Projectile.Center);

            for (int i = 0; i < 10; i++) Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone);

            Projectile.tileCollide = false;

            stopTileHit = true;
            return false;
        }

        NPC hitNPC;
        Vector2 hitOffset;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (stopTileHit) return;

            hitNPC = target;
            hitOffset = Projectile.Center - target.Center;

            Projectile.frame = 1;

            SoundEngine.PlaySound(SoundID.Research, Projectile.Center);

            for (int i = 0; i < 10; i++) Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone);

            Projectile.tileCollide = false;

            stopTileHit = true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.oldVelocity == Vector2.Zero && stopTileHit) return false;
            return null;
        }

        public override bool CanHitPlayer(Player target)
        {
            return !stopTileHit;
        }

        bool stopTileHit;
        int foldDir;
        const int afterJumpTimeLeft = 20;
        bool dissapear;

        const int jumpPreCooldown = 20;
        int jumpPreTimer;
        public override void AI()
        {
            if (Projectile.timeLeft <= afterJumpTimeLeft)
            {
                drawAlpha -= 1f / afterJumpTimeLeft;
            }

            if (stopTileHit)
            {
                Projectile.velocity = Vector2.Zero;

                if (hitNPC is not null)
                {
                    if (hitNPC.life <= 0)
                    {
                        hitNPC = null;
                        stopTileHit = false;
                        Projectile.tileCollide = true;
                        Projectile.frame = 0;
                        return;
                    }
                    Projectile.Center = hitNPC.Center + hitOffset;
                }

                if (dissapear)
                {
                    Projectile.rotation += 0.015f * foldDir * MathF.Pow(Projectile.timeLeft * 0.1f, 3);
                }
                else if (jumpPreTimer >= jumpPreCooldown)
                {
                    Vector2 rotVectorNorm = Projectile.rotation.ToRotationVector2();
                    rotVectorNorm.Normalize();

                    Vector2 lineEnd = Projectile.Center - rotVectorNorm * 85;

                    foreach (Player player in Main.player)
                    {
                        if (player.controlJump && Collision.CheckAABBvLineCollision(player.Hitbox.TopLeft(), player.Hitbox.Size(), Projectile.Center, lineEnd))
                        {
                            Explode();

                            SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);
                            SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Projectile.Center);

                            player.jump = 0;
                            player.fallStart = (int)player.position.Y / 16;

                            player.canJumpAgain_Cloud = true;

                            foldDir = Projectile.rotation % (MathHelper.Pi * 2) > MathHelper.PiOver2 || Projectile.rotation % (MathHelper.Pi * 2) < -MathHelper.PiOver2 ? 1 : -1;

                            player.velocity = Projectile.Center.DirectionTo(player.Center) * 7;
                            Projectile.timeLeft = afterJumpTimeLeft;
                            dissapear = true;
                        }
                    }
                }
                else
                {
                    jumpPreTimer++;
                }

                return;
            }

            Projectile.velocity.Y += 0.2f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        const int explosionDamage = 60;
        void Explode()
        {
            SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

            int radius = 90;
            Rectangle rect = new((int)Projectile.Center.X - radius, (int)Projectile.Center.Y - radius, 2 * radius, 2 * radius);
            DarknessFallenUtils.ForeachNPCInRectangle(rect, npc =>
            {
                if (!npc.friendly) npc.StrikeNPC(explosionDamage, 2, Math.Sign((npc.Center - Projectile.Center).X));
            });

            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.Torch, 1, speedFromCenter: 13, amount: 8);
            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.Smoke, 1, speedFromCenter: 13, amount: 8);
            DarknessFallenUtils.NewGoreCircular(Projectile.Center, GoreID.Smoke1 + Main.rand.Next(3), 50, speedFromCenter: 4, rotation: Main.rand.NextFloat(MathHelper.TwoPi), scale: Main.rand.NextFloat(0.6f, 1.2f));
        }

        float drawAlpha = 1;
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileInHBCenter(lightColor * drawAlpha, true, offset: Vector2.UnitX * 22);
            return false;
        }
    }
}
