﻿using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace DarknessFallenMod.NPCs
{
    public class BlueBeetle : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue Beetle");
            Main.npcFrameCount[NPC.type] = 6;

            NPCID.Sets.TrailCacheLength[Type] = 15;
            NPCID.Sets.TrailingMode[Type] = 3;
        }


        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 50;
            NPC.damage = 12;
            NPC.defense = 10;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit50;
            NPC.DeathSound = SoundID.NPCDeath53;
            NPC.value = 89f;
            NPC.knockBackResist = 0.5f;
            //NPC.aiStyle = 3;
            //AIType = NPCID.GoblinScout;
            NPC.aiStyle = -1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.BlueBeetleBanner>();
        }

        float normalSpeed = 1.5f;
        float inRangeSpeed = 2.5f;

        bool inRange;
        const int range = 50000;

        const float acceleration = 0.05f;
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest();
                player = Main.player[NPC.target];
            }

            Vector2 directionToPlayer = NPC.DirectionTo(player.Center);

            int xDirToPlayer = MathF.Sign(directionToPlayer.X);
            NPC.direction = xDirToPlayer;

            inRange = false;
            if (Vector2.DistanceSquared(player.Center, NPC.Center) < range)
            {
                inRange = true;

                if (Main.rand.NextBool(2)) Dust.NewDust(NPC.Hitbox.BottomLeft(), NPC.width, 2, DustID.Dirt);
            }

            if (((player.Center.Y < NPC.position.Y && inRange) || NPC.collideX) && NPC.velocity.Y == 0) NPC.velocity.Y -= 6f;

            float xSpeed = acceleration * xDirToPlayer;
            if (MathF.Sign(NPC.velocity.X) != xDirToPlayer)
            {
                xSpeed *= 2;
            }

            NPC.velocity.X += xSpeed;

            float maxSpeed = inRange ? inRangeSpeed : normalSpeed;
            NPC.velocity.X = Math.Clamp(NPC.velocity.X, -maxSpeed, maxSpeed);
        }

        /* Some effects that dont work rn
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (inRange)
            {
                Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;

                Main.spriteBatch.End();
                Main.spriteBatch.BeginWithShaderOptions();

                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    if (i % 3 == 0)
                    {
                        Vector2 pos = NPC.oldPos[i];

                        Main.EntitySpriteDraw(
                            tex,
                            pos - Main.screenPosition + NPC.Hitbox.Size() * 0.5f,
                            NPC.frame,
                            drawColor * 0.5f,
                            NPC.rotation,
                            NPC.frame.Size() * 0.5f,
                            NPC.scale,
                            NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                            0
                            );
                    }
                }

                Main.spriteBatch.End();
                Main.spriteBatch.BeginWithDefaultOptions();
            }

            return true;
        }
        */

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = -NPC.direction;
            NPC.frameCounter++;

            if (NPC.frameCounter % 6 == 5f) // Ticks per frame
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 6) // 6 is max # of frames
            {
                NPC.frame.Y = 0; // Reset back to default
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlueChitin>(), 2));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.SurfaceJungle.Chance * 0.07f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement("The blue specimen of a family of beetles, known enraging when around humans")
            });
        }
    }
}