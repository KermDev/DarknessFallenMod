using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;
using System.Runtime.CompilerServices;

namespace DarknessFallenMod.Items.MagicWeapons
{
    public class FungiteStaffProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 9;
            Projectile.height = 9;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
        }


        public override void AI()
        {
            AnimateProjectile();

            Projectile.rotation = Projectile.velocity.ToRotation();
            FX();
        }

        void FX()
        {
            if (!Main.dedServ) Lighting.AddLight(Projectile.Center, 0.2f, 0.1f, 0.5f);
            if (Main.rand.NextBool(10)) Dust.NewDust(Projectile.Center, 1, 1, DustID.GlowingMushroom);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.GlommerBounce, Projectile.Center);
            return true;
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3)
            {
                Projectile.frame++;
                Projectile.frame %= 5;
                Projectile.frameCounter = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i <= 3; i++)
            {
                Dust.NewDust(Projectile.Center + Main.rand.NextVector2Unit() * 8, 8, 8, DustID.GlowingMushroom);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DarknessFallenUtils.DrawProjectileInHBCenter(Projectile, Color.White, true, offset: Vector2.UnitX * -5);
            return false;
        }
    }
}