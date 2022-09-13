using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.Bestiary;

namespace DarknessFallenMod.NPCs
{
    public class ZombieFinger : ModNPC
    {
        Vector2 Velocity;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zombie Finger");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.velocity = NPC.velocity * (0.01f);
            NPC.width = 20;
            NPC.height = 11;
            NPC.damage = 9;
            NPC.defense = 2;
            NPC.lifeMax = 47;
            NPC.value = 33f;
            NPC.aiStyle = 3;
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit10;
            NPC.DeathSound = SoundID.NPCDeath1;
            AnimationType = NPCID.GreenSlime;
            NPC.scale = 2f;
            NPC.knockBackResist = 0.5f;
            AIType = NPCID.GoblinScout;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.ZombieFingerBanner>();
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

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNight.Chance * 0.09f;
        }

        public override void FindFrame(int frameHeight)
        {

            NPC.spriteDirection = -NPC.direction;
            NPC.frameCounter++;

            if (NPC.frameCounter >= 40)
            {
                NPC.frameCounter = 0;
            }
            NPC.frame.Y = (int)NPC.frameCounter / 10 * frameHeight;
        }

        public override void OnKill()
        {
            int LegGore = GoreID.MaggotZombieMaggotPieces;

            for (int i = 0; i < 3; i++)
            {
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), LegGore);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("A finger from a zombie, at least thats what it looks like. I wonder where its from")
            });
        }
    }
}