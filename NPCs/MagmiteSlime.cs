using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs
{
    public class MagmiteSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 28;
            NPC.height = 18;
            NPC.damage = 20;
            NPC.defense = 16;
            NPC.lifeMax = 80;
            NPC.value = 6800f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.aiStyle = NPCAIStyleID.Slime;
            AIType = NPCID.BlueSlime;

            AnimationType = NPCID.BlueSlime;

            //Banner = Type;
            //BannerItem = ModContent.ItemType<Items.Placeable.Banners.>();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 90);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!(spawnInfo.Player.ZoneHallow || spawnInfo.Player.ZoneCrimson || spawnInfo.Player.ZoneCorrupt || Main.eclipse || spawnInfo.Player.ZoneTowerNebula || spawnInfo.Player.ZoneTowerVortex
                || spawnInfo.Player.ZoneTowerSolar || spawnInfo.Player.ZoneTowerStardust || Main.pumpkinMoon || Main.snowMoon || spawnInfo.PlayerSafe || spawnInfo.Player.ZoneSnow
                || spawnInfo.Player.ZoneDesert) && spawnInfo.Player.ZoneOverworldHeight && Main.dayTime)
            {
                return 0.05f;
            }
            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("placeholder")
            });

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Ores.MagmiteOre>(), 1, 0, 3));
        }
    }
}
