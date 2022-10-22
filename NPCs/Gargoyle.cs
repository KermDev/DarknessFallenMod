using DarknessFallenMod.Utils;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs
{
    public class Gargoyle : ModNPC
    {
        bool awake = false;
        Vector2 target = Vector2.Zero;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 43;
            NPC.height = 50;

            NPC.damage = 17;
            NPC.defense = 7;
            NPC.lifeMax = 80;
            NPC.value = 200f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.aiStyle = 0;
            NPC.noGravity = true;

            //Banner = Type;
            //BannerItem = ModContent.ItemType<Items.Placeable.Banners.GargoyleBanner>();

        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            if (Vector2.DistanceSquared(NPC.Center, player.Center) <= 20000)
            {
                awake = true;
            }

            if(!awake)
            {
                NPC.velocity = Vector2.Zero;
                NPC.frame = new Rectangle(0, 126, 52, 42);
                NPC.velocity.Y += 5f;
                return;
            }
            else
            {
                NPC.ai[0] -= 1;
                if (NPC.ai[0] <= 0)
                {
                    target = ((player.Center - NPC.Center) + player.Center);
                    NPC.ai[0] = 60;
                }
                NPC.Center = Vector2.Lerp(NPC.Center, target, (60 - NPC.ai[0]) / 60);
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            //target.AddBuff(ModContent.BuffType<Buffs.DarknessBuff>(), 90);
        }

        public override void FindFrame(int frameHeight)
        {
            if (awake)
            {
                base.FindFrame(frameHeight);
                NPC.BasicAnimation(frameHeight, 10);

            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("Placeholder.")
            });

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Do Loot
        }
    }
}
