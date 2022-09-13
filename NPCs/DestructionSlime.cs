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
    public class DestructionSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Destruction Slime");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[2];
        }

        public override void SetDefaults()
        {
            NPC.width = 28;
            NPC.height = 44;
            NPC.damage = 13;
            NPC.defense = 5;
            NPC.lifeMax = 78;
            NPC.value = 72f;
            NPC.aiStyle = 1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AIType = NPCID.GreenSlime;
            AnimationType = NPCID.GreenSlime;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.DestructionSlimeBanner>();
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNightMonster.Chance * 0.02f;
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
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ItemID.Gel, Main.rand.Next(0, 8));
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ItemID.DemoniteOre, Main.rand.Next(0, 4));
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ModContent.ItemType<SoulOfDestruction>(), (int)MathF.Floor(Main.rand.Next(0, 3) / 2f));
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,
                new FlavorTextBestiaryInfoElement("These dangerous slimes lurk around the darkness and pose a significant threat to adventurers however the ones who lived to tell the tale say they hold bountiful treasure")
            });
        }
    }
}