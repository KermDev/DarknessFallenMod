using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using System;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;
using DarknessFallenMod.Tiles.Banners;
using DarknessFallenMod.Items.Placeable.Banners;

namespace DarknessFallenMod.NPCs
{
    public class Yanagidako : ModNPC
    {
        Vector2 Velocity;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yanagidako");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 46;
            NPC.damage = 14;
            NPC.defense = 5;
            NPC.lifeMax = 84;
            NPC.value = 12f;
            NPC.aiStyle = 0;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AnimationType = NPCID.GreenSlime;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.YanagidakoBanner>();
        }

        public override void AI()
        {
            Vector2 Target;

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];

            Target = player.Center;

           

            NPC.ai[1] -= 0.01f;
            float Clamped = Math.Clamp(NPC.ai[1], 0f, 4f);
            if (Clamped != NPC.ai[1])
            {
                Velocity = Vector2.Normalize(Target - NPC.Center) * 15;
                NPC.ai[1] = 2f;
            }

            NPC.Center += Velocity;
            Velocity *= 0.9f;
            NPC.velocity = Vector2.Zero;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Underworld.Chance * 0.08f;
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
            int LegGore = Mod.Find<ModGore>("YanagidakoGore0").Type;
            int HeadGore = Mod.Find<ModGore>("YanagidakoGore1").Type;

            for (int i = 0; i < 3; i++)
            {
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), LegGore);
            }
            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), HeadGore);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfDestruction>(), 5, minimumDropped: 1, maximumDropped: 3));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("A harmless squid that has been infected by Hell's touch, making it a uncontrollable force of destruction")
            });
        }
    }
    
}