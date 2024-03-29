﻿using DarknessFallenMod.Utils;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs
{
    public class Nyctoid : ModNPC
    {
        Player Target => Main.player[NPC.target];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;

            NPCID.Sets.TrailCacheLength[Type] = 58;
            NPCID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 20;
            NPC.defense = 10;
            NPC.lifeMax = 240;
            NPC.value = 67f;
            NPC.netAlways = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        const float moveSpeed = 4f;
        const float inertia = 7f;
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            else
            {
                Vector2 dirToTarg = NPC.DirectionTo(Target.Center);

                Vector2 velToTarg = dirToTarg * moveSpeed;
                NPC.velocity = (NPC.velocity * (inertia - 1) + velToTarg) / inertia;

                NPC.rotation += NPC.velocity.X * 0.04f;
                
                
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.BasicAnimation(frameHeight, 7);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.DrawAfterImageNPC(proh => Color.Black * 0.2f);
            NPC.DrawNPCInHBCenter(drawColor);
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement("A swirling void of Darkness primordial. Its many eyes watch for nearby prey")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.Nyctoplasm>(), 10));
        }
    }
}
