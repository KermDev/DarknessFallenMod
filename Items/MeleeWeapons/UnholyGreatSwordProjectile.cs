using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class UnholyGreatSwordProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unholy Greatsword"); // Name of the projectile. It can be appear in chat
            Main.projFrames[Projectile.type] = 1; //number of frames in the animation;
        }

        // Setting the default parameters of the projectile
        // You can check most of Fields and Properties here https://github.com/tModLoader/tModLoader/wiki/Projectile-Class-Documentation
        public override void SetDefaults()
        {
            Projectile.width = 16; // The width of projectile hitbox
            Projectile.height = 32; // The height of projectile hitbox

            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Melee; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 0.4f; // How much light emit around the projectile
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
            Random x = new Random();
            int X = x.Next(-4, 4); //these 2 lines create a random number between -60 and 60
            Random y = new Random();
            int Y = y.Next(-20, 20);  //these 2 lines create another random number between -60 and 60

            if (Main.rand.Next(0, 9) == 1)
            {
                Dust.NewDust(Projectile.Center, 4, 4, DustID.Bone);
            }

            Projectile.velocity *= 0.9f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft) //this is caled whenever the projectile expires (only once);
        {
            for (int i = 0; i <= 30; i++) //repeats 50 times;
            {
                Random x = new Random();
                int X = x.Next(-4, 4); //these 2 lines create a random number between -60 and 60
                Random y = new Random();
                int Y = y.Next(-20, 20);  //these 2 lines create another random number between -60 and 60

                Dust.NewDust(Projectile.Center, 4, 4, DustID.Bone);
            }
        }
    }
}