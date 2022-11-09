using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.AmethystSaber
{
    public class AmethystSpike : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            Projectile.alpha += 2;
            if (Projectile.alpha >= 220)
                Projectile.Kill();
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void Kill(int timeLeft)
        {
            Projectile.velocity = Projectile.oldVelocity * 0.2f;
            for (int num361 = 0; num361 < 30; num361++)
            {
                int num362 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.AmberBolt, 0f, 0f, 75, default(Color), 1.2f);
                Dust dust120;
                Dust dust2;
                if (Main.rand.NextBool(2))
                {
                    dust120 = Main.dust[num362];
                    dust2 = dust120;
                    dust2.alpha += 25;
                }
                if (Main.rand.NextBool(2))
                {
                    dust120 = Main.dust[num362];
                    dust2 = dust120;
                    dust2.alpha += 25;
                }
                if (Main.rand.NextBool(2))
                {
                    dust120 = Main.dust[num362];
                    dust2 = dust120;
                    dust2.alpha += 25;
                }
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num362].scale = 0.6f;
                }
                else
                {
                    Main.dust[num362].noGravity = true;
                }
                dust120 = Main.dust[num362];
                dust2 = dust120;
                dust2.velocity *= 0.3f;
                dust120 = Main.dust[num362];
                dust2 = dust120;
                dust2.velocity += Projectile.velocity;
                dust120 = Main.dust[num362];
                dust2 = dust120;
                dust2.velocity *= 1f + Main.rand.Next(-100, 101) * 0.01f;
                Main.dust[num362].velocity.X += Main.rand.Next(-50, 51) * 0.015f;
                Main.dust[num362].velocity.Y += Main.rand.Next(-50, 51) * 0.015f;
                Main.dust[num362].position = Projectile.Center;
            }
        }
    }
}
