using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs
{
    public class MagmiteEye : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DemonEye);
            NPC.damage *= 2;
            NPC.defense *= 2;
            NPC.lifeMax *= 2;
            NPC.value *= 2;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.aiStyle = NPCAIStyleID.DemonEye;
            AIType = NPCID.DemonEye;

            AnimationType = NPCID.DemonEye;

            //Banner = Type;
            //BannerItem = ModContent.ItemType<Items.Placeable.Banners.>();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 90);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!(spawnInfo.Player.ZoneHallow || spawnInfo.Player.ZoneCrimson || spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneDungeon || spawnInfo.Player.ZoneSnow
                || spawnInfo.PlayerSafe || spawnInfo.Player.ZoneSnow || spawnInfo.Player.ZoneDesert || spawnInfo.Player.ZoneJungle) && (spawnInfo.Player.ZoneNormalUnderground
                || spawnInfo.Player.ZoneNormalCaverns))
            {
                return 0.05f;
            }
            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("Placeholder")
            });

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Ores.MagmiteOre>(), 1, 0, 3));
        }
    }
}
