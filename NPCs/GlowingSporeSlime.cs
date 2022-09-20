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
using System.Linq;
using Terraria.Audio;

namespace DarknessFallenMod.NPCs
{
    public class GlowingSporeSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glowing Spore Slime");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 15;
            NPC.damage = 11;
            NPC.defense = 3;
            NPC.lifeMax = 55;
            NPC.value = 22f;
            NPC.aiStyle = 1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AIType = NPCID.GreenSlime;
            AnimationType = NPCID.GreenSlime;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.SporeSlimeBanner1>();
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneGlowshroom)
            {
                return 0.4f;
            }
            return 0;
        }

        public override void AI()
        {
            if (!Main.dedServ) Lighting.AddLight(NPC.Center, 1f, 0.6f, 1.4f);
        }

        const int effectRadius = 140;
        public override void OnKill()
        {
            Array.ForEach(Main.player, player =>
            {
                if (player.DistanceSQ(NPC.Center) < effectRadius * effectRadius)
                {
                    player.AddBuff(Main.rand.NextFromList(new int[] { BuffID.Electrified, BuffID.Poisoned, BuffID.OnFire }), 320);
                }
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 17; i++)
                {
                    Dust.NewDust(NPC.Center - effectRadius * Vector2.One,
                        effectRadius * 2,
                        effectRadius * 2,
                        DustID.TintableDustLighted,
                        0,
                        0,
                        160,
                        Color.BlueViolet,
                        Main.rand.NextFloat(1.2f, 3)
                        );

                    Dust.NewDust(NPC.Center - effectRadius * Vector2.One,
                        effectRadius * 2,
                        effectRadius * 2,
                        DustID.BlueTorch
                        );
                }

                for (int i = 0; i < 8; i++)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Unit() * 2, 375, Main.rand.NextFloat(0.3f, 1.5f));
                }

                if (!Main.dedServ) Lighting.AddLight(NPC.Center, 1.6f, 0.9f, 2f);

                SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, NPC.Center);
            }
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

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.GlowingMushroom, minimumDropped: 1, maximumDropped: 4));
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, minimumDropped: 1, maximumDropped: 2));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                new FlavorTextBestiaryInfoElement("A slime that has wandered too far and has evolved to fit with the mushrooms")
            });
        }
    }
}