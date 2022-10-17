using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Pets.Sniffer
{
    public class SnifferPet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sniffer");

            Main.projFrames[Projectile.type] = 11;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 26;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        Player Player => Main.player[Projectile.owner];

        const float _GRAVITY = 0.2f;
        const float _ACCELERATION = 0.6f;
        const float _DEACCELERATIONMULT = 0.7f;
        public override void AI()
        {
            Projectile.timeLeft = 10;

            int dirXToPlayer = (Projectile.DirectionTo(Player.Center).X > 0 ? 1 : -1);
            float distSQToPlayer = Projectile.Center.DistanceSQ(Player.Center);
            float distToPlayerX = Math.Abs(Projectile.Center.X - Player.Center.X);

            if (distSQToPlayer > 640000)
            {
                DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.ShadowbeamStaff, 10, speedFromCenter: 10, amount: 16);

                Vector2 teleportPos = Player.Center;
                for (int i = 0; i < 10; i++)
                {
                    teleportPos = Player.Center + Main.rand.NextVector2Unit() * 100;

                    if (!Collision.SolidTiles(teleportPos - new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), Projectile.width, Projectile.height)) break;
                }

                Projectile.Center = teleportPos;

                DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.ShadowbeamStaff, 10, speedFromCenter: 10, amount: 16);
            }
            else if (distToPlayerX > 60)
            {
                Projectile.velocity.X += dirXToPlayer * _ACCELERATION;
            }

            Projectile.velocity.X *= _DEACCELERATIONMULT;

            float collOffset = Projectile.spriteDirection == 1 ? Projectile.width + Projectile.velocity.X : Projectile.velocity.X;

            if (Projectile.velocity.Y <= 0)
            {
                if (DarknessFallenUtils.SolidTerrain(Projectile.BottomLeft + Vector2.UnitX * collOffset - Vector2.UnitY * 18))
                {
                    if (DarknessFallenUtils.SolidTerrain(new Rectangle(Projectile.Hitbox.X, Projectile.Hitbox.Y + 16, Projectile.width, Projectile.height)))
                    {
                        Projectile.velocity.Y -= 1f;
                    }
                }
                else if (DarknessFallenUtils.SolidTerrain(Projectile.BottomLeft + Vector2.UnitX * collOffset - Vector2.UnitY * 2))
                {
                    Projectile.velocity.Y -= 0.5f;
                }
            }

            Projectile.velocity.Y += _GRAVITY;

            Projectile.spriteDirection = dirXToPlayer;

            Animate(7);
        }

        void Animate(int speed)
        {
            if (Math.Abs(Projectile.velocity.X) > 0.1f)
            {
                if (Projectile.velocity.Y == _GRAVITY)
                {
                    Projectile.frameCounter++;
                    if (Projectile.frameCounter >= 10 * speed)
                    {
                        Projectile.frameCounter = 0;
                    }

                    Projectile.frame = Projectile.frameCounter / speed;
                }
                else Projectile.frame = 8;
            }
            else
            {
                Projectile.frame = 10;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileInHBCenter(lightColor, true, origin: Projectile.spriteDirection == 1 ? new Vector2(20, 20) : new Vector2(36, 20));
            return false;
        }
    }
}
