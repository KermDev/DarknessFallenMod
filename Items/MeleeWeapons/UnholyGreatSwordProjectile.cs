using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    public class UnholyGreatSwordProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unholy Greatsword Projectile");
            Main.projFrames[Projectile.type] = 1;
        }
        
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;

            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.light = 0.4f;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[0];

            for (int i = 0; i < Main.rand.Next(2, 6); i++)
            {
                Vector2 SpawnPoint = target.Center + new Vector2(Main.rand.Next(30, 80) / 10, Main.rand.Next(30, 80)).RotatedByRandom(MathF.PI * 2);
                int Proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), SpawnPoint, Vector2.Normalize(SpawnPoint - target.Center) * 15f, ProjectileID.Bone, 32, 0f, player.whoAmI);
                Main.projectile[Proj].friendly = true;
                Main.projectile[Proj].hostile = false;
                Main.projectile[Proj].active = true;
                Main.projectile[Proj].penetrate = 10;
            }
        }

        public override void AI()
        {
            if (Main.rand.Next(0, 9) == 1)
            {
                Dust.NewDust(Projectile.Center, 4, 4, DustID.Bone);
            }

            Projectile.velocity *= 0.9f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileInHBCenter(lightColor, origin: new Vector2(8, 23));
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i <= 30; i++)
            {

                Dust.NewDust(Projectile.Center, 4, 4, DustID.Bone);
            }
        }
    }
}