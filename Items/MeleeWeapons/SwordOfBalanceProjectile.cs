using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

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
        }

        // Setting the default parameters of the projectile
        // You can check most of Fields and Properties here https://github.com/tModLoader/tModLoader/wiki/Projectile-Class-Documentation
        public override void SetDefaults()
        {
            Projectile.width = 48; // The width of projectile hitbox
            Projectile.height = 48; // The height of projectile hitbox
            Projectile.penetrate = 10;
            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Melee; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 0.9f; // How much light emit around the projectile
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 180; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
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
            Random x = new Random();
            int X = x.Next(-4, 4); //these 2 lines create a random number between -60 and 60
            Random y = new Random();
            int Y = y.Next(-20, 20);  //these 2 lines create another random number between -60 and 60

            if (Main.rand.Next(0, 9) == 1)
            {
                Dust.NewDust(Projectile.Center, 4, 4, DustID.BlueCrystalShard);
            }

            Projectile.rotation += 0.6f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(Projectile.penetrate <= 1)
            {
                Projectile.Kill();
                return true;
            }
            
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

            // If the projectile hits the left or right side of the tile, reverse the X velocity
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }

            // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }


            return false;
        }

        public override void Kill(int timeLeft) //this is caled whenever the projectile expires (only once);
        {
            for (int i = 0; i <= 30; i++) //repeats 50 times;
            {
                Random x = new Random();
                int X = x.Next(-4, 4); //these 2 lines create a random number between -60 and 60
                Random y = new Random();
                int Y = y.Next(-20, 20);  //these 2 lines create another random number between -60 and 60

                Dust.NewDust(Projectile.Center, 4, 4, DustID.BlueCrystalShard);
            }
        }
    }
}