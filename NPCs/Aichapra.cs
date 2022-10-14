using DarknessFallenMod.Utils;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs
{
    public class Aichapra : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 9;
        }

        public override void SetDefaults()
        {
            NPC.width = 43;
            NPC.height = 50;
            NPC.damage = 11;
            NPC.defense = 17;
            NPC.lifeMax = 230;
            NPC.value = 67f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.aiStyle = 3;
            AIType = NPCID.GoblinScout;
        }

        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            else
            {
                NPC.spriteDirection = -NPC.direction;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.DarknessBuff>(), 90);
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.X != 0 && NPC.collideY) NPC.BasicAnimation(frameHeight, 4);
        }
    }
}
