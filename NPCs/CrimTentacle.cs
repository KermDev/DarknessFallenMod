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
    public class CrimTentacle : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Tentacle");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[4];
        }

        public override void SetDefaults()
        {
            NPC.width = 28;
            NPC.height = 70;
            NPC.damage = 20;
            NPC.defense = 4;
            NPC.lifeMax = 94;
            NPC.value = 67f;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AIType = NPCID.None;
            NPC.knockBackResist = 0;
            NPC.scale = 0.5f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.CrimsonTentacleBanner>();
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Crimson.Chance * 0.07f;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 20)
            {
                NPC.frameCounter = 0;
            }
            NPC.frame.Y = (int)NPC.frameCounter / 10 * frameHeight;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.CrimFlesh>(), chanceDenominator: 5));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                new FlavorTextBestiaryInfoElement("A manifestation of the crimson's will to spread and attack, these tentacle like objects with act as a danger to those who dare venture inside.")
            });
        }
    }
}