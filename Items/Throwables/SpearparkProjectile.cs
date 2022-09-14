using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

using ReLogic.Content;

namespace DarknessFallenMod.Items.Throwables
{
    public class SpearparkProjectile : ModProjectile
    {
        Asset<Texture2D> textureAsset;

        public override void SetDefaults()
        {
            textureAsset = ModContent.Request<Texture2D>(Texture);

            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 900;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }

        bool stopTileHit;
        int foldDir;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item51, Projectile.Center);

            for (int i = 0; i < 10; i++) Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone);

            Projectile.tileCollide = false;

            stopTileHit = true;
            return false;
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

                if (dissapear)
                {
                    Projectile.rotation += 0.015f * foldDir * MathF.Pow(Projectile.timeLeft * 0.1f, 3);
                }
                else if (jumpPreTimer >= jumpPreCooldown)
                {
                    Vector2 rotVectorNorm = Projectile.rotation.ToRotationVector2();
                    rotVectorNorm.Normalize();

                    Vector2 lineEnd = Projectile.Center - rotVectorNorm * 65;

                    foreach (Player player in Main.player)
                    {
                        if (player.controlJump && Collision.CheckAABBvLineCollision(player.Hitbox.TopLeft(), player.Hitbox.Size(), Projectile.Center, lineEnd))
                        {
                            SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);

                            int goreAmount = Main.rand.Next(2, 3);
                            for (int i = 0; i < goreAmount; i++) Gore.NewGore(
                                Projectile.GetSource_FromAI(),
                                player.Hitbox.BottomLeft() + Vector2.UnitX * (player.Hitbox.Width / goreAmount) * i - Vector2.UnitY * 15,
                                Vector2.UnitY * 2.3f,
                                GoreID.Smoke1 + Main.rand.Next(3),
                                Main.rand.NextFloat(0.5f, 1f)
                                );

                            for (int i = 0; i < 5; i++) Dust.NewDust(player.Hitbox.BottomLeft(), player.Hitbox.Width, 2, DustID.Smoke);

                            player.jump = 0;
                            player.fallStart = (int)player.position.Y / 16;

                            player.canJumpAgain_Cloud = true;

                            foldDir = Projectile.rotation % (MathHelper.Pi * 2) > MathHelper.PiOver2 || Projectile.rotation % (MathHelper.Pi * 2) < -MathHelper.PiOver2 ? 1 : -1;

                            float xModif = Math.Clamp(MathF.Sign(player.velocity.X) * MathF.Pow(player.velocity.X, 2), -7, 7);
                            if (Math.Abs(player.velocity.X) < Math.Abs(xModif)) player.velocity.X = xModif;

                            player.velocity.Y = -11;

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

        float drawAlpha = 1;
        public override bool PreDraw(ref Color lightColor)
        {
            /*
            Texture2D texture = textureAsset.Value;

            DrawData data = new(
                texture,
                Projectile.Center - Main.screenPosition + Projectile.rotation.ToRotationVector2() * 10,
                null,
                lightColor * drawAlpha,
                Projectile.rotation,
                new Vector2(texture.Width, texture.Height / 2),
                Projectile.scale,
                SpriteEffects.None,
                0
                );

            Main.EntitySpriteDraw(data);
            */

            Projectile.DrawProjectileInHBCenter(lightColor * drawAlpha, offset: Vector2.UnitX * 10);

            return false;
        }
    }
}