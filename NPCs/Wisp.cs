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
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.NPCs
{
    public class Wisp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wisp");
            Main.npcFrameCount[NPC.type] = 4;
        }


        public override void SetDefaults()
        {
            NPC.width = 14;
            NPC.height = 28;
            NPC.damage = 18;
            NPC.defense = 15;
            NPC.lifeMax = 180;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.value = 73f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 22;
            NPC.noGravity = true;
        }

        float shootTimer;
        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            if (shootTimer++ > 150)
            {
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.ChaosBall);
                //Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Normalize(player.Center - NPC.Center) * 16, ProjectileID., 5, 0, Main.myPlayer);
                shootTimer = 0;
            }

            NPC.spriteDirection = -NPC.direction;
            NPC.rotation = NPC.velocity.X * -0.08f;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.BasicAnimation(frameHeight, 7);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server) return;

            if (NPC.life <= 0)
            {
                DarknessFallenUtils.NewDustCircular(NPC.Center, DustID.ShadowbeamStaff, 20, speedFromCenter: 8, amount: 32).ForEach(dust => dust.noGravity = true);
            }

            //NPC.SpawnGoreOnDeath("CrimsonMawGore1", "CrimsonMawGore2", "CrimsonMawGore3");
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.MeleeWeapons.TwilightShards>(), 50));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return base.SpawnChance(spawnInfo);// SpawnCondition.Crimson.Chance * 0.08f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement("Scorched remains of slain humans coalesced into this flaming omen. Their flames are surprisingly helpful for cooking.")
            });
        }
    }
}