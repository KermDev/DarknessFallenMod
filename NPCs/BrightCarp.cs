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
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.NPCs
{
    public class BrightCarp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bright Carp");
            Main.npcFrameCount[NPC.type] = 4;
        }


        public override void SetDefaults()
        {
            NPC.width = 66;
            NPC.height = 20;
            NPC.damage = 40;
            NPC.defense = 22;
            NPC.lifeMax = 280;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 7400; // 74 silver
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = NPCAIStyleID.Piranha;
            NPC.noGravity = true;
            AIType = NPCID.Shark;

            Banner = Type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.BrightCarpBanner>();
        }

        public override void AI()
        {
            //NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if (player.wet)
            {
                NPC.spriteDirection = player.Center.X > NPC.Center.X ? 1 : -1;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.BasicAnimation(frameHeight, 7);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server) return;
            
            /*
            if (NPC.life <= 0)
            {
                DarknessFallenUtils.NewDustCircular(NPC.Center, DustID.ShadowbeamStaff, 20, speedFromCenter: 8, amount: 32).ForEach(dust => dust.noGravity = true);
            }
            */

            // Do gores or whatever
            //NPC.SpawnGoreOnDeath("CrimsonMawGore1", "CrimsonMawGore2", "CrimsonMawGore3");
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Tools.LightRod.LightRod>(), 50));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return base.SpawnChance(spawnInfo); // SpawnCondition.Crimson.Chance * 0.08f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement("With all things the Light touches, it modifies them to be better suited for their environment. Sharks are an example of this.")
            });
        }

    }
}