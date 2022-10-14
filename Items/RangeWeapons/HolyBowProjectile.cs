using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.Items.RangeWeapons
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class HolyBowProjectile : ModProjectile
    {
        public object dust { get; private set; }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Wave"); // Name of the projectile. It can be appear in chat
            Main.projFrames[Projectile.type] = 2; //number of frames in the animation;
        }

        // Setting the default parameters of the projectile
        // You can check most of Fields and Properties here https://github.com/tModLoader/tModLoader/wiki/Projectile-Class-Documentation
        public override void SetDefaults()
        {
            Projectile.width = 20; // The width of projectile hitbox
            Projectile.height = 20; // The height of projectile hitbox

            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Ranged; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 0.4f; // How much light emit around the projectile
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.extraUpdates = 4;

        }
        
        public override void AI()
        {
            AnimateProjectile();

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 2; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileInHBCenter(lightColor, true, Vector2.UnitX * 5);
            return false;
        }

        public override void Kill(int timeLeft) //this is caled whenever the projectile expires (only once);
        {
            for (int i = 0; i <= 5; i++) //repeats 50 times;
            {
                Random x = new Random();
                int X = x.Next(-20, 20); //these 2 lines create a random number between -60 and 60
                Random y = new Random();
                int Y = y.Next(-5, 5);  //these 2 lines create another random number between -60 and 60

                Dust.NewDust(new Vector2(Projectile.position.X + X, Projectile.position.Y + Y), 8, 8, DustID.FireworkFountain_Yellow);
            }
        }
    }
}