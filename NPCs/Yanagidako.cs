using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using System;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;
using DarknessFallenMod.Tiles.Banners;
using DarknessFallenMod.Items.Placeable.Banners;
using Microsoft.Xna.Framework.Graphics;
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.NPCs
{
    public class Yanagidako : ModNPC
    {
        Vector2 Velocity;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yanagidako"); //yakuza squid;
            Main.npcFrameCount[NPC.type] = 4;

            NPCID.Sets.TrailCacheLength[Type] = 16;
            NPCID.Sets.TrailingMode[Type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.width = 28;
            NPC.height = 28;
            NPC.damage = 14;
            NPC.defense = 5;
            NPC.lifeMax = 84;
            NPC.value = 12f;
            NPC.aiStyle = 0;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AnimationType = NPCID.GreenSlime;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.YanagidakoBanner>();
        }

        public override void AI()
        {
            Vector2 Target;

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];

            Target = player.Center;

            if(Vector2.DistanceSquared(Target, NPC.Center) < 10000)
            {
                Target = player.Center + new Vector2(Main.rand.Next(-25, 25), Main.rand.Next(-25, 25));
            }

            NPC.ai[1] -= 0.01f;
            float Clamped = Math.Clamp(NPC.ai[1], 0f, 0.3f);
            if (Clamped != NPC.ai[1])
            {
                Velocity = Vector2.Normalize(Target - NPC.Center) * 15;
                DarknessFallenUtils.NewDustCircular(NPC.Center, DustID.AncientLight, 6, speedFromCenter: 5, color: Color.OrangeRed).ForEach(dust => dust.noGravity = true);
                NPC.rotation = Velocity.ToRotation() + MathHelper.PiOver2;
                NPC.ai[1] = 0.3f;
            }

            NPC.Center += Velocity;
            Velocity *= 0.9f;
            NPC.velocity = Vector2.Zero;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Underworld.Chance * 0.08f;
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

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 origin = new Vector2(19, 11);

            spriteBatch.BeginReset(DarknessFallenUtils.BeginType.Shader, DarknessFallenUtils.BeginType.Default, sb => NPC.DrawAfterImageNPC(prog => Color.Red * 0.35f, oldRot: false, origin: origin));
            NPC.DrawNPCInHBCenter(drawColor, origin: origin);
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server) return;

            NPC.SpawnGoreOnDeath("YanagidakoGore0", "YanagidakoGore1");
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfDestruction>(), 5, minimumDropped: 1, maximumDropped: 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Pets.BloodySquid.BloodyTentacle>(), 33));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("A harmless squid that has been infected by Hell's touch, making it a uncontrollable force of danger")
            });
        }
    }
}