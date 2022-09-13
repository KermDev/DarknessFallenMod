using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;
using System.Runtime.CompilerServices;

namespace DarknessFallenMod.Items.MagicWeapons
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class InfernoRodProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Inferno"); // Name of the projectile. It can be appear in chat
            Main.projFrames[Projectile.type] = 1; //number of frames in the animation;
        }

        // Setting the default parameters of the projectile
        // You can check most of Fields and Properties here https://github.com/tModLoader/tModLoader/wiki/Projectile-Class-Documentation
        public override void SetDefaults()
        {
            Projectile.width = 18; // The width of projectile hitbox
            Projectile.height = 18; // The height of projectile hitbox
            Projectile.light = 0.75f;
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
        public override void AI()
        {
            float num132 = (float)Math.Sqrt((double)(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y));
            float num133 = Projectile.localAI[0];
            if (num133 == 0f)
            {
                Projectile.localAI[0] = num132;
                num133 = num132;
            }
            float num134 = Projectile.position.X;
            float num135 = Projectile.position.Y;
            float num136 = 3000f;
            bool flag3 = false;
            int num137 = 0;
            if (Projectile.ai[1] == 0f)
            {
                for (int num138 = 0; num138 < 200; num138++)
                {
                    if (Main.npc[num138].CanBeChasedBy(this, false) && (Projectile.ai[1] == 0f || Projectile.ai[1] == (float)(num138 + 1)))
                    {
                        float num139 = Main.npc[num138].position.X + (float)(Main.npc[num138].width / 2);
                        float num140 = Main.npc[num138].position.Y + (float)(Main.npc[num138].height / 2);
                        float num141 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num139) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num140);
                        if (num141 < num136 && Collision.CanHit(new Vector2(Projectile.position.X + (float)(Projectile.width / 2), Projectile.position.Y + (float)(Projectile.height / 2)), 1, 1, Main.npc[num138].position, Main.npc[num138].width, Main.npc[num138].height))
                        {
                            num136 = num141;
                            num134 = num139;
                            num135 = num140;
                            flag3 = true;
                            num137 = num138;
                        }
                    }
                }
                if (flag3)
                {
                    Projectile.ai[1] = (float)(num137 + 1);
                }
                flag3 = false;
            }
            if (Projectile.ai[1] > 0f)
            {
                int num142 = (int)(Projectile.ai[1] - 1f);
                if (Main.npc[num142].active && Main.npc[num142].CanBeChasedBy(this, true) && !Main.npc[num142].dontTakeDamage)
                {
                    float num143 = Main.npc[num142].position.X + (float)(Main.npc[num142].width / 2);
                    float num144 = Main.npc[num142].position.Y + (float)(Main.npc[num142].height / 2);
                    if (Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num143) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num144) < 1000f)
                    {
                        flag3 = true;
                        num134 = Main.npc[num142].position.X + (float)(Main.npc[num142].width / 2);
                        num135 = Main.npc[num142].position.Y + (float)(Main.npc[num142].height / 2);
                    }
                }
                else
                {
                    Projectile.ai[1] = 0f;
                }
            }
            if (!Projectile.friendly)
            {
                flag3 = false;
            }
            if (flag3)
            {
                float num145 = num133;
                Vector2 vector10 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num146 = num134 - vector10.X;
                float num147 = num135 - vector10.Y;
                float num148 = (float)Math.Sqrt((double)(num146 * num146 + num147 * num147));
                num148 = num145 / num148;
                num146 *= num148;
                num147 *= num148;
                int num149 = 8;
                Projectile.velocity.X = (Projectile.velocity.X * (float)(num149 - 1) + num146) / (float)num149;
                Projectile.velocity.Y = (Projectile.velocity.Y * (float)(num149 - 1) + num147) / (float)num149;
            }
            Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 8, 8, DustID.Torch);

            Projectile.velocity *= 1.05f;

            Projectile.rotation += 0.4f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig);
            return true;
        }

        public override void Kill(int timeLeft) //this is caled whenever the projectile expires (only once);
        {
            for (int i = 0; i <= 49; i++) //repeats 50 times;
            {
                Random x = new Random();
                int X = x.Next(-60, 60); //these 2 lines create a random number between -60 and 60
                Random y = new Random();
                int Y = y.Next(-60, 60);  //these 2 lines create another random number between -60 and 60

                Dust.NewDust(new Vector2(Projectile.position.X + 5 + X, Projectile.position.Y + 5 + Y), 8, 8, DustID.Torch);
            }
        }
    }
}