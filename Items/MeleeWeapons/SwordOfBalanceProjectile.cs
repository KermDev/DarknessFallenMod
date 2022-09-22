using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.GameContent.Shaders;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class SwordOfBalanceProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orb of Balance"); // Name of the projectile. It can be appear in chat
            Main.projFrames[Projectile.type] = 2; //number of frames in the animation;

            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 1;
        }

        // Setting the default parameters of the projectile
        // You can check most of Fields and Properties here https://github.com/tModLoader/tModLoader/wiki/Projectile-Class-Documentation
        public override void SetDefaults()
        {
            Projectile.width = 27; // The width of projectile hitbox
            Projectile.height = 27; // The height of projectile hitbox
            Projectile.penetrate = 10;
            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Melee; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 0.9f; // How much light emit around the projectile
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 180; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        // Custom AI
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 1) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 1; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }

        public override void AI()
        {
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.BlueCrystalShard);
            }

            Projectile.rotation += 0.6f;
        }

        public override void Kill(int timeLeft)
        {
            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.BlueCrystalShard, 5, speedFromCenter: 5, amount: 16);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.BeginWithShaderOptions();

            Projectile.DrawAfterImage(Color.White * 0.6f, animated: true);

            Main.spriteBatch.End();
            Main.spriteBatch.BeginWithDefaultOptions();

            Projectile.DrawProjectileInHBCenter(Color.White * 0.9f, true, centerOrigin: true);
            return false;
        }
    }
}