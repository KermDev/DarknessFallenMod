using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    internal class AmethystSaberProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("amethyst saber projectile");
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 600;
            Projectile.light = 2.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;



        }


        
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.GemAmethyst, 0f, 0f, 0, default(Color), 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.2f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.013f;

            int dust2 = Dust.NewDust(Projectile.Center, 1, -1, DustID.PinkTorch, 0f, 0f, 0, default(Color), 1f);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 0.2f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 135) * 0.013f;

        }

    }
}
