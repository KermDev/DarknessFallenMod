using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;
using System;
using Terraria.GameContent.Bestiary;
using MonoMod.Cil;

namespace DarknessFallenMod.NPCs
{
    public class RedBeetle : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Beetle");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 35;
            NPC.height = 53;
            NPC.damage = 13;
            NPC.defense = 10;
            NPC.lifeMax = 250;
            NPC.HitSound = Sounds.SoundManager.BeetleHit;
            NPC.DeathSound = Sounds.SoundManager.BeetleDeath;
            NPC.value = 90f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 3;
            AIType = NPCID.GoblinScout;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.RedBeetleBanner>();
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

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if(!target.noKnockback)
            {
                target.velocity += Vector2.Normalize(target.Center - NPC.Center) * 10f;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RedChitin>(), 2));
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
                new FlavorTextBestiaryInfoElement("The red specimen of a family of beetles, known for its pushing power. If only there was some kind of shield to block its effects.")
            });
        }
    }
}