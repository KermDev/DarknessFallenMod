using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs.Goober
{
    public class Goober : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goober");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 60;
            NPC.damage = 50;
            NPC.defense = 31;
            NPC.lifeMax = 3200;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            NPC.knockBackResist = 0.03f;
            NPC.aiStyle = 44;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = -NPC.direction;
            NPC.frameCounter++;

            if (NPC.frameCounter % 6f == 5f)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 5) 
            {
                NPC.frame.Y = 0;
            }
        }

        int i;
        public override void AI()
        {
            Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.091f, 0.24f, .24f);

            i++;
            Player player = Main.player[NPC.target];




            Vector2 moveTo = player.Center;
            float speed = 10f;
            Vector2 move = moveTo - NPC.Center;
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            move *= speed / magnitude;
            if (i % 100 == 0)
            {
                NPC.velocity = move;
            }

            Vector2 distanceNorm = player.position - NPC.position;
            distanceNorm.Normalize();
            NPC.ai[0]++;
          
            if (NPC.ai[0] % 256 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, distanceNorm.X * 4, distanceNorm.Y * 4, ModContent.ProjectileType<GooberLaser>(), 30, 0f, Main.myPlayer, 0f, 0f);
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
                for (int i = 0; i < 3; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);

            for (int k = 0; k < 2; k++)
            {
                Vector2 vel = Vector2.Normalize(new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101))) * (Main.rand.Next(50, 100) * 0.04f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.GoldCoin);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = vel;
                Main.dust[dust].position = NPC.Center - (Vector2.Normalize(vel) * 34f);
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(6))
                target.AddBuff(BuffID.Cursed, 300);
        }
    }
}
