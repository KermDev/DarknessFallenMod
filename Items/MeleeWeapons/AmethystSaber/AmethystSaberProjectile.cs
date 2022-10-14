using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.Items.MeleeWeapons.AmethystSaber
{
    internal class AmethystSaberProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Amethyst saber projectile");
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 600;
            Projectile.light = 2.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.alpha = 255;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void AI()
        {
            Projectile.ManualFriendlyLocalCollision();

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst, 0f, 0f, 0, default, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.2f;
            Main.dust[dust].scale = Main.rand.Next(100, 135) * 0.013f;

            int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkTorch, 0f, 0f, 0, default, 1f);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 0.2f;
            Main.dust[dust2].scale = Main.rand.Next(100, 135) * 0.013f;
        }

    }
}
