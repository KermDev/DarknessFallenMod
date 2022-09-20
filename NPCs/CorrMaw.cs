﻿using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using DarknessFallenMod.Items.Accessories;
using Terraria.GameContent.Bestiary;
using System;
using System.Collections.Generic;
namespace DarknessFallenMod.NPCs
{
    public class CorrMaw : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corruption Maw");
            Main.npcFrameCount[NPC.type] = 9;
        }


        public override void SetDefaults()
        {
            NPC.width = 35;
            NPC.height = 53;
            NPC.damage = 17;
            NPC.defense = 3;
            NPC.lifeMax = 120;
            NPC.HitSound = SoundID.NPCHit50;
            NPC.DeathSound = SoundID.NPCDeath53;
            NPC.value = 73f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 3;
            AIType = NPCID.GoblinScout;
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
                NPC.velocity.Y += 20f;
            }

            if (NPC.collideX && NPC.velocity.Y == 0)
            {
                NPC.velocity.Y += 6f;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server) return;

            if (NPC.life <= 0)
            {
                int gore1 = Mod.Find<ModGore>("CorruptionMawGore1").Type;
                int gore2 = Mod.Find<ModGore>("CorruptionMawGore2").Type;
                int gore3 = Mod.Find<ModGore>("CorruptionMawGore3").Type;

                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Unit() * 2, gore1);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Unit() * 2, gore2);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Unit() * 2, gore3);
            }
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

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CorruptedAntenna>(), 10));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Corruption.Chance * 0.08f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                new FlavorTextBestiaryInfoElement("A manifestation of the corruption's violent nature, these monsters will attack anything that moves")
            });
        }
    }
}