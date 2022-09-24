using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;
using System.Runtime.CompilerServices;

namespace DarknessFallenMod.Items.MagicWeapons
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class BloodWaveProjectile : ModProjectile
    {
        public object dust { get; private set; }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Wave");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.scale = 0.2f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Red;
        }

        int maxHB = 42;
        float scaleResize = 0.03f;
        public override void AI()
        {
            Projectile.BasicAnimation(10);
            
            if (Projectile.scale < 1)
            {
                Projectile.scale += scaleResize;
                Projectile.Center -= Vector2.One * (int)(maxHB * scaleResize);
            }
            else
            {
                Projectile.scale = 1;
            }

            Projectile.width = (int)(Projectile.scale * maxHB);
            Projectile.height = (int)(Projectile.scale * maxHB);
            

            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);

            if (!Main.dedServ) Lighting.AddLight(Projectile.Center, 0.6f, 0.2f, 0.2f);

            Projectile.rotation += 0.2f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.Blood, 20, speedFromCenter: 6, amount: 48);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileInHBCenter(Color.White, true, centerOrigin: true);
            return false;
        }
    }
}