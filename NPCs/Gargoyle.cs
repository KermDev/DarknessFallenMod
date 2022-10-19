using DarknessFallenMod.Utils;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs
{
    public class Gargoyle : ModNPC
    {
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


            Banner = Type;
            //BannerItem = ModContent.ItemType<Items.Placeable.Banners.GargoyleBanner>();

        }

        public override void AI()
        {
            // Do AI
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            //target.AddBuff(ModContent.BuffType<Buffs.DarknessBuff>(), 90);
        }

        public override void FindFrame(int frameHeight)
        {
            //if (NPC.velocity.X != 0 && NPC.collideY) NPC.BasicAnimation(frameHeight, 4);
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
