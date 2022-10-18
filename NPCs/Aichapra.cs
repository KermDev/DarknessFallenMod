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
    public class Aichapra : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.width = 43;
            NPC.height = 50;
            NPC.damage = 11;
            NPC.defense = 17;
            NPC.lifeMax = 230;
            NPC.value = 67f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.aiStyle = 3;
            AIType = NPCID.GoblinScout;


            Banner = Type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.AichapraBanner>();

        }

        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            else
            {
                NPC.spriteDirection = -NPC.direction;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.DarknessBuff>(), 90);
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.X != 0 && NPC.collideY) NPC.BasicAnimation(frameHeight, 4);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement("A hideous creature covered in sharp spikes. Their nails are however very useful for crafting.")
            });

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.CapraNail>(), 10));
        }
    }
}
