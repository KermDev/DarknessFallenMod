using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Utils;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace DarknessFallenMod.Items.MeleeWeapons.MoltenUchigatana
{
    public class MoltenUchigatanaFireProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 99;

            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 900;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 600;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.timeLeft = Main.projFrames[Type] * animSpeed;
        }

        const int animSpeed = 5;
        public override void AI()
        {
            Projectile.velocity *= 0.8f;
            Projectile.scale += 0.02f;

            Projectile.BasicAnimation(animSpeed);

            if (Projectile.velocity.LengthSquared() > 0.1f)
            {
                Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    Main.rand.NextFromList(new int[] { DustID.InfernoFork, DustID.AmberBolt }),
                    Projectile.velocity.X,
                    Projectile.velocity.Y
                    );
            }


            if (!Main.dedServ)
                Lighting.AddLight(Projectile.Center, 8f, 1.5f, 1.5f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawAfterImage(prog => Color.White * Projectile.velocity.LengthSquared() * 0.4f * Main.rand.Next(2), animated: true);
            Projectile.DrawProjectileInHBCenter(lightColor, true, centerOrigin: true);

            return false;
        }
    }
}
