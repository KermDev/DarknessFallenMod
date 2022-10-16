using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.Items.RangeWeapons.HolyBow
{
    public class LightArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.light = 0.4f;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 2;

        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileInHBCenter(lightColor, true, origin: new Vector2(5, 7));
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i <= 5; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FireworkFountain_Yellow);
            }
        }
    }
}