using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using System;
using System.Collections.Generic;

namespace DarknessFallenMod.NPCs
{
    public class BlueBeetle : ModNPC
    {
        int speed;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue Beetle");
            Main.npcFrameCount[NPC.type] = 6;
        }


        public override void SetDefaults()
        {
            NPC.width = 35;
            NPC.height = 53;
            NPC.damage = 12;
            NPC.defense = 10;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit50;
            NPC.DeathSound = SoundID.NPCDeath53;
            NPC.value = 89f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 3;
            AIType = NPCID.GoblinScout;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.BlueBeetleBanner>();
        }

        

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                NPC.velocity.Y = 20;
            }

            if (!player.active)
            {
                NPC.velocity.Y += 20f ;
            }

            if (NPC.collideX && NPC.velocity.Y == 0)
            {
                NPC.velocity.Y += 6f;
            }

            speed = Vector2.DistanceSquared(player.Center, NPC.Center) < 100000 ? 3 : 1;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = -NPC.direction;
            NPC.frameCounter++;

            if (NPC.frameCounter % 6 == 5f) // Ticks per frame
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 6) // 6 is max # of frames
            {
                NPC.frame.Y = 0; // Reset back to default
            }
        }


        public override void OnKill()
        {
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ModContent.ItemType<BlueChitin>(), (int)MathF.Floor(Main.rand.Next(0, 3) / 2f));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.SurfaceJungle.Chance * 0.07f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement("The blue specimen of a family of beetles, known enraging when around humans")
            });
        }
    }
}