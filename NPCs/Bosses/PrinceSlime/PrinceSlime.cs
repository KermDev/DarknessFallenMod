using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace DarknessFallenMod.NPCs.Bosses.PrinceSlime
{
    [AutoloadBossHead]
    public partial class PrinceSlime : ModNPC
    {
        #region KING SLIME SPAWN PREVENTION IL EDIT, DONT TOUCH IT WILL BREAK
        public override void Load()
        {
            IL.Terraria.NPC.DoDeathEvents_AdvanceSlimeRain += ILPreventCountAdvance;
        }

        void ILPreventCountAdvance(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchAdd())) return;

            c.EmitDelegate<Func<int, int>>((x) =>
            {
                if ((NPC.AnyNPCs(ModContent.NPCType<PrinceSlime>()) || !Systems.DownedBossSystem.downedPrinceSlime) && Main.slimeRainKillCount + x == (NPC.downedSlimeKing ? 75 : 150))
                {
                    return 0;
                }
                return 1;
            });
        }
        #endregion

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 8;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetDefaults()
        {
            NPC.width = 95;
            NPC.height = 95;
            NPC.damage = 36;
            NPC.defense = 9;
            NPC.lifeMax = 3500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = -200f;
            NPC.value = Item.buyPrice(gold: 4, silver: 46, copper: 12);
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 20;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC.stairFall = true;

            BannerItem = ModContent.ItemType<Items.Placeable.Banners.PrinceSlimeBanner>();
            Banner = Type;

            NPC.BossBar = ModContent.GetInstance<PrinceSlimeBossBar>();
        }

        public override bool? CanFallThroughPlatforms()
        {
            return Target is not null && Target.Top.Y > NPC.Bottom.Y;
        }

        const int animationSpeed = 10;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            int maxFrame = Main.npcFrameCount[Type] - 1;

            if (NPC.frameCounter > animationSpeed)
            {
                NPC.frameCounter = 0;

                NPC.frame.Y += 108;
            }

            if (NPC.velocity.Y != 0)
            {
                NPC.frame.Y = (aiState == AIState.Phase1 ? 2 : 6) * 109;
            }

            if (NPC.frame.Y > (aiState == AIState.Phase1 ? 3 : maxFrame) * 108) NPC.frame.Y = (aiState == AIState.Phase1 ? 0 : 4) * 108;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;

            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server) return;

            if (NPC.life <= 0)
            {
                int crown = Mod.Find<ModGore>("PrinceSlime_CrownGore").Type;
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vector2.UnitY * -3 + Vector2.UnitX * (Main.rand.NextBool() ? -1 : 1) * 1.5f, crown);
            }
        }

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref Systems.DownedBossSystem.downedPrinceSlime, -1);

            NPC.DropCustomBannerKillCount(50, ModContent.ItemType<Items.Placeable.Banners.PrinceSlimeBanner>());
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.MeleeWeapons.Slimescaliber>(), 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.SummonWeapons.CultSlime>(), 4));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.BottleOSlime>(), 3));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.MagicWeapons.SlimyRain>(), 100));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.SlimeGuardHelmet>(), 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.SlimeGuardChestplate>(), 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.SlimeGuardLeggings>(), 3));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.PrinceSlimeTrophy>(), 10));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.Furniture.PrinceSlimeRelic>()));

            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Consumables.PrinceSlimeBossBag>()));

            npcLoot.Add(ItemDropRule.Common(ItemID.LesserHealingPotion, 1, 1, 5));

            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, minimumDropped: 2, maximumDropped: 12));
        }

        public override void OnSpawn(IEntitySource source)
        {
            PrinceSlimeOnePerSlimeRain.PrinceSlimeSpawned = true;
            ChatHelper.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("His slimy excellency has arrived"), Color.Green);

            NPC.TargetClosest();
            NPC.Center = Main.player[NPC.target].Center;
            NPC.position.Y -= 1200;
            NPC.velocity.Y = 12;

            // Timers
            MortarTimer = 90;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.slimeRain && !PrinceSlimeOnePerSlimeRain.PrinceSlimeSpawned && !NPC.AnyNPCs(NPC.type) && !NPC.AnyNPCs(NPCID.KingSlime))
            {
                return 0.3f;
            }
            return 0;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.SlimeRain,
                new FlavorTextBestiaryInfoElement("The prince of all that is slime, it has somehow acquired a crown with magic powers.")
            });
        }
    }

    public class PrinceSlimeBossBar : ModBossBar
    {
        public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame)
        {
            return ModContent.Request<Texture2D>("DarknessFallenMod/NPCs/Bosses/PrinceSlime/PrinceSlime_Head_Boss");
        }
    }

    public class PrinceSlimeOnePerSlimeRain : ModSystem
    {
        public static bool PrinceSlimeSpawned { get; set; } = false;
        public override void PostUpdateEverything()
        {
            if (!Main.slimeRain) PrinceSlimeSpawned = false;
        }
    }
}
