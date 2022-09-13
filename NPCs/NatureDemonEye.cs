using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria;
using System;
using System.Collections.Generic;

namespace DarknessFallenMod.NPCs
{
    public class NatureDemonEye : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nature DemonEye");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[2];
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 15;
            NPC.damage = 21;
            NPC.defense = 5;
            NPC.lifeMax = 82;
            NPC.value = 52f;
            NPC.aiStyle = 2;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AIType = NPCID.DemonEye;
            AnimationType = NPCID.DemonEye;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.NatureDemonEyeBanner>();
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.SurfaceJungle.Chance * 0.04f;
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

        public override void OnKill()
        {
            int LegGore = Mod.Find<ModGore>("NatureDemonEyeGore1").Type;
            int HeadGore = Mod.Find<ModGore>("NatureDemonEyeGore0").Type;

            for (int i = 0; i < 3; i++)
            {
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), LegGore);
            }
            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), HeadGore);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Acorn, 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.Wood, minimumDropped: 0, maximumDropped: 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.Lens, minimumDropped: 0, maximumDropped: 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.BlackLens, 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfNature>(), 5, minimumDropped: 1, maximumDropped: 3));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Sun,
                new FlavorTextBestiaryInfoElement("Demon eyes whos corpses are taken back into nature are reborn with will to protect the environment from danger")
            });
        }
    }
}