using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using System.Linq;
using Terraria.DataStructures;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class HellButcherProjectile : ModProjectile
    {
        public object dust { get; private set; }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell Butcher"); // Name of the projectile. It can be appear in chat
            Main.projFrames[Projectile.type] = 1; //number of frames in the animation;

            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 4;
        }

        // Setting the default parameters of the projectile
        // You can check most of Fields and Properties here https://github.com/tModLoader/tModLoader/wiki/Projectile-Class-Documentation
        public override void SetDefaults()
        {
            Projectile.width = 20; // The width of projectile hitbox
            Projectile.height = 20; // The height of projectile hitbox

            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Magic; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 0.4f; // How much light emit around the projectile
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
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
            int X = x.Next(-10, 10); //these 2 lines create a random number between -60 and 60
            Random y = new Random();
            int Y = y.Next(-10, 10);  //these 2 lines create another random number between -60 and 60

            if(Main.rand.Next(0, 2) == 1)
            { 
                Dust.NewDust(new Vector2(Projectile.position.X + 5 + X, Projectile.position.Y + 5 + Y), 8, 8, DustID.InfernoFork);  
            }
        }

        public override void Kill(int timeLeft) //this is caled whenever the projectile expires (only once);
        {
            /*
            for (int i = 0; i <= 30; i++) //repeats 50 times;
            {
                Random x = new Random();
                int X = x.Next(-60, 60); //these 2 lines create a random number between -60 and 60
                Random y = new Random();
                int Y = y.Next(-60, 60);  //these 2 lines create another random number between -60 and 60

                
                Dust.NewDust(new Vector2(Projectile.position.X + 5 + X, Projectile.position.Y + 5 + Y), 8, 8, 174);

                int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.FireflyHit, 0f, 0f, 0, default(Color), 174f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
                Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.013f;    
            }
            */

            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.InfernoFork, 10, speedFromCenter: 3, amount: 30);
            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.RedTorch, 10, speedFromCenter: 6, amount: 10, noGravity: true);
        }

        VertexStrip vertexStripMM = new VertexStrip();
        public override bool PreDraw(ref Color lightColor)
        {
            /*
            Main.spriteBatch.End();
            Main.spriteBatch.BeginWithShaderOptions();

            GameShaders.Misc["FlameLash"]
                .UseSaturation(-2f)
                .UseOpacity(6)
                .Apply(new DrawData?());

            //Enumerable.Repeat<float>(0, Projectile.oldRot.Count()).ToArray()

            vertexStripMM.PrepareStrip(Projectile.oldPos,
                Enumerable.Repeat<float>(0, Projectile.oldRot.Count()).ToArray(),
                prog => Color.Lerp(Color.Red, Color.Yellow * 0.5f, prog),
                prog => MathHelper.Lerp(30, 1, prog),
                Projectile.Hitbox.Size() * 0.5f - Main.screenPosition,
                null,
                true
                );

            vertexStripMM.PrepareStripWithProceduralPadding(Projectile.oldPos,
                Projectile.oldRot,
                prog => Color.Lerp(Color.Red, Color.Yellow * 0.5f, prog),
                prog => MathHelper.Lerp(30, 1, prog),
                Projectile.Hitbox.Size() * 0.5f - Main.screenPosition,
                true,
                true
                );

            vertexStripMM.DrawTrail();

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.End();
            Main.spriteBatch.BeginWithDefaultOptions();

            */
            Projectile.DrawProjectileInHBCenter(lightColor, centerOrigin: true);
            return false;
        }
    }
}