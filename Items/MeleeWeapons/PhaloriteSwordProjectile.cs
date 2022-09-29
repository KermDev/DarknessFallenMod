using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    public class PhaloriteSwordProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phalorite Orb");
            Main.projFrames[Projectile.type] = 1;

            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        
        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.penetrate = 10;
            Projectile.aiStyle = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 190;
            Projectile.extraUpdates = 2;

            Projectile.localNPCHitCooldown = 20;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void AI()
        {
            Projectile.ManualFriendlyLocalCollision();

            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff);
            }

            if (Main.dedServ) Lighting.AddLight(Projectile.Center, 0.4f, 0.2f, 0.9f);

            Projectile.rotation += 0.1f;
        }

        public override void Kill(int timeLeft)
        {
            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.ShadowbeamStaff, 5, speedFromCenter: 5, amount: 16);
            DarknessFallenUtils.ForeachNPCInRange(Projectile.Center, 2500, npc =>
            {
                if (!npc.friendly && Projectile.localNPCImmunity[npc.whoAmI] <= 0) npc.StrikeNPC(Projectile.damage, 0, 0);
            });

            SoundEngine.PlaySound(SoundID.Item30 with { Volume = 0.1f }, Projectile.Center);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.BeginShader();

            Projectile.DrawAfterImage(prog => Color.White * 0.6f);

            Main.spriteBatch.End();
            Main.spriteBatch.BeginDefault();
            return true;
        }
    }
}