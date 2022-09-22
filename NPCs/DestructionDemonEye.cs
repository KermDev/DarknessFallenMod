using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace DarknessFallenMod.NPCs
{
    public class DestructionDemonEye : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Destruction Demon Eye");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.damage = 19;
            NPC.defense = 5;
            NPC.lifeMax = 82;
            NPC.value = 52f;
            NPC.aiStyle = 2;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AIType = NPCID.DemonEye;
            AnimationType = NPCID.DemonEye;
            //Banner = NPC.type;
            //BannerItem = ModContent.ItemType<Items.Placeable.Banners.>();
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNightMonster.Chance * 0.2f;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = (int)NPC.frameCounter / 4 * frameHeight;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server) return;

            NPC.SpawnGoreOnDeath("DestructionEyeGore1", "DestructionEyeGore2", "DestructionEyeGore3", "DestructionEyeGore4");
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SoulOfDestruction>(), 5, minimumDropped: 1, maximumDropped: 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.Lens, minimumDropped: 0, maximumDropped: 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.BlackLens, 100));
            npcLoot.Add(ItemDropRule.Common(ItemID.DemoniteOre, minimumDropped: 0, maximumDropped: 4));
        }
    }
}
