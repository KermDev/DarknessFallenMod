using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;

namespace Trinitarian.Content.NPCs.Enemies.Ocean
{
    public class SeaSnake : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sea Snake");
            Main.npcFrameCount[base.NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.chaseable = false;
            NPC.damage = 34;
            NPC.width = 50;
            NPC.height = 28;
            NPC.defense = 12;
            NPC.lifeMax = 110;
            NPC.aiStyle = 16;
            AIType = NPCID.Goldfish;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit33;
            NPC.DeathSound = SoundID.NPCDeath28;
            NPC.knockBackResist = 0.3f;
        }

        public ref float DashTimer => ref NPC.ai[1];

        public override void AI()
        {
            Player target = Main.player[NPC.target];

            if (target.wet)
            {
                NPC.noGravity = false;
                NPC.spriteDirection = -NPC.direction;

                if (DashTimer++ >= 60 && Main.tile[(int)(NPC.position.X / 16), (int)(NPC.position.Y / 16 - 2)].LiquidAmount == 255)
                {
                    var direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center);
                    NPC.velocity = direction * Main.rand.Next(10, 20);
                    NPC.rotation = NPC.velocity.X * 0.2f;
                    DashTimer = 0;
                }
            }
            else
            {
                NPC.spriteDirection = -NPC.direction;
                NPC.aiStyle = 16;
                NPC.noGravity = true;
                AIType = NPCID.Goldfish;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode)
            {
                return SpawnCondition.Ocean.Chance * 0.50f;
            }
            else
            {
                return 0f;
            }
        }
    }
}