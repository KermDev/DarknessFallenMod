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
    public class NatureSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nature Slime");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[2];
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 15;
            NPC.damage = 10;
            NPC.defense = 3;
            NPC.lifeMax = 42;
            NPC.value = 16f;
            NPC.aiStyle = 1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AIType = NPCID.GreenSlime;
            AnimationType = NPCID.GreenSlime;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.NatureSlimeBanner>();
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.SurfaceJungle.Chance * 0.08f;
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
            npcLoot.Add(ItemDropRule.Common(ItemID.Acorn, minimumDropped: 2, maximumDropped: 4));
            npcLoot.Add(ItemDropRule.Common(ItemID.Wood, minimumDropped: 2, maximumDropped: 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, minimumDropped: 1, maximumDropped: 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfNature>(), 5, minimumDropped: 1, maximumDropped: 3));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Sun,
                new FlavorTextBestiaryInfoElement("Slimes infested and absorb elements of the jungle, boosting their power and allowing them to search for prey elsewhere")
            });
        }
    }
}