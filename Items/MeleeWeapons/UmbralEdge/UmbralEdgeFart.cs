using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.UmbralEdge
{
    public class UmbralEdgeFart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        const int originalSize = 34;
        public override void SetDefaults()
        {
            Projectile.width = originalSize;
            Projectile.height = originalSize;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 240;
            Projectile.alpha = 10;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;

        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            /*
			Player player = Main.player[Projectile.owner];

			if (player.ownedProjectileCounts[Type] > 0)
			{
				Projectile proj = player.GetOldestProjectile(Type);
				if (proj.timeLeft > 50) proj.timeLeft = 50;
			}*/
        }

        const int dissapearTL = 50;
        const float scaleUp = 0.02f;
        public override void AI()
        {
            Projectile.velocity *= 0.93f;

            Projectile.BasicAnimation(8);

            if (Projectile.timeLeft <= dissapearTL)
            {
                Projectile.alpha += (int)(255f / dissapearTL);
            }

            if (Projectile.scale < 2.5f)
            {
                Projectile.scale += scaleUp;
                int newSize = (int)(Projectile.scale * originalSize);
                Projectile.Resize(newSize, newSize);
            }

            if (Main.rand.NextBool(18)) Dust.NewDust(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.Smoke,
                newColor: Color.Lerp(Color.Black, Color.Purple, Main.rand.NextFloat()),
                Scale: Main.rand.NextFloat(0.8f, Projectile.scale + 0.5f),
                Alpha: Projectile.alpha
                );

            DarknessFallenUtils.ForeachNPCInRange(Projectile.Center, 2 * Projectile.width * Projectile.width, npc =>
            {
                if (npc.CanBeChasedBy() && !npc.boss)
                {
                    npc.velocity += npc.Center.DirectionTo(Projectile.Center) * Math.Clamp(1000 / npc.DistanceSQ(Projectile.Center), 0, 0.2f);
                }
            });
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawAfterImage(prog => Color.Lerp(Color.Purple, Color.Black, 0.5f) * 0.3f * Math.Clamp(Projectile.velocity.LengthSquared(), 0.3f, 0.8f), true, true, true);
            Projectile.DrawProjectileInHBCenter(lightColor, true, centerOrigin: true);
            return false;
        }
    }
}
