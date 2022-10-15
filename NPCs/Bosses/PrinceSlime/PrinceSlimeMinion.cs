using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs.Bosses.PrinceSlime
{
    public class PrinceSlimeMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;

            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 39;
            NPC.height = 31;

            NPC.damage = 7;
            NPC.defense = 2;
            NPC.lifeMax = 20;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.alpha = 70;

            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(silver: 23);

            Banner = Type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.PrinceSlimeMinionBanner>();

            NPC.aiStyle = NPCAIStyleID.Slime;
            AIType = NPCAIStyleID.Slime;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);

            if (Main.rand.NextBool(60)) Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, SpeedX: 0, SpeedY: 0.3f, Alpha: 150, newColor: Color.Green * 0.8f);
        }

        const int animationSpeed = 10;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (NPC.frameCounter > animationSpeed)
            {
                NPC.frameCounter = 0;

                NPC.frame.Y += frameHeight;
            }

            if (NPC.velocity.Y != 0)
            {
                NPC.frame.Y = frameHeight;
            }

            if (NPC.frame.Y > frameHeight) NPC.frame.Y = 0;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, minimumDropped: 1, maximumDropped: 3));
        }
    }
}
