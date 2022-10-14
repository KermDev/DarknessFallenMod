using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using System;
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    public class ValikaProjectile : ModProjectile
    {
        List<NPC> HitNPCs = new List<NPC>();

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            DisplayName.SetDefault("Valika");

            ProjectileID.Sets.TrailCacheLength[Type] = 12;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.alpha = 140;

            Projectile.localNPCHitCooldown = 20;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.LightGreen;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if(target == null)
            {
                return;
            }

            if (target.CanBeChasedBy())
            {
                HitNPCs.Add(target);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        ref float circDustTimer => ref Projectile.ai[0];
        public override void AI()
        {
            Projectile.ManualFriendlyLocalCollision();
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.BasicAnimation(10);

            if (!Main.dedServ) Lighting.AddLight(Projectile.Center, 0.4f, 1.8f, 0);

            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BubbleBurst_Green).noGravity = true;
            /*
            circDustTimer++;
            if (circDustTimer > 20)
            {

                DarknessFallenUtils.NewDustCircular(Projectile.Center + Projectile.rotation.ToRotationVector2() * 70, ModContent.DustType<Dusts.TintableTrailingDust>(), 1, speedFromCenter: 13, amount: 48, scale: 4).ForEach(dust => dust.noGravity = true);

                circDustTimer = 0;
            }*/
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawAfterImage(prog => Color.Lerp(Color.Yellow, Color.Red, prog) * (MathF.Sin(Main.GameUpdateCount * 0.1f) * 0.2f + 0.6f), true, true);
            return true;
        }
    }
}
