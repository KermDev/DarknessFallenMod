using Terraria.ModLoader;
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
    public class Wisp : ModNPC
    {
        float shootTimer = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wisp");
            Main.npcFrameCount[NPC.type] = 4;
        }


        public override void SetDefaults()
        {
            NPC.width = 14;
            NPC.height = 28;
            NPC.damage = 11;
            NPC.defense = 5;
            NPC.lifeMax = 90;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.value = 73f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 22;
            NPC.noGravity = true;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            // Fire a Chaos Ball every 5 seconds
            if (shootTimer++ % 300 == 0)
            {
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.ChaosBall);
                // Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Normalize(player.Center - NPC.Center) * 16, ProjectileID., 5, 0, Main.myPlayer);
                // shootTimer = 0;
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
            if (NPC.frame.Y >= frameHeight * 4) // 6 is max # of frames
            {
                NPC.frame.Y = 0; // Reset back to default
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server) return;

            //NPC.SpawnGoreOnDeath("CrimsonMawGore1", "CrimsonMawGore2", "CrimsonMawGore3");
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodyJaw>(), 10));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return base.SpawnChance(spawnInfo);// SpawnCondition.Crimson.Chance * 0.08f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                // BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                // new FlavorTextBestiaryInfoElement("A manifestation of the crimson's violent nature, these monsters will attack anything that moves")
            });
        }

    }
}