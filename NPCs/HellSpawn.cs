using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace DarknessFallenMod.NPCs
{
    public class HellSpawn : ModNPC
    {
        Player Target => Main.player[NPC.target];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire, 
                    BuffID.OnFire3, 
                    BuffID.Burning,
                    BuffID.Confused
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

            DisplayName.SetDefault("Hellwing");
        }


        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.damage = 12;
            NPC.defense = 3;
            NPC.lifeMax = 160;
            NPC.value = 22f;
            NPC.aiStyle = -1;
            NPC.noGravity = true;

            NPC.lavaImmune = true;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        ref float chargeTimer => ref NPC.ai[0];
        const float chargeTime = 120;
        const float acceleration = 0.1f;
        const float maxSpeed = 4;
        bool isCharging;
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Vector2 dirToTarget = NPC.DirectionTo(Target.Center);
            NPC.spriteDirection = Math.Sign(dirToTarget.X);

            if (isCharging)
            {
                chargeTimer++;
                NPC.velocity += dirToTarget.RotatedBy(-MathHelper.PiOver4 * NPC.spriteDirection) * acceleration * 0.7f;

                Vector2 mouthPos = NPC.Center + new Vector2(10 * NPC.spriteDirection, -7);

                int width = 12;
                if (Main.rand.NextBool(6)) Dust.NewDust(mouthPos - Vector2.UnitX * width * 0.5f, width, 2, DustID.InfernoFork);

                if (chargeTimer > 120)
                {
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), mouthPos, dirToTarget * 10, ProjectileID.Fireball, 18, 2).tileCollide = false;
                    DarknessFallenUtils.NewDustCircular(mouthPos, DustID.InfernoFork, 2, speedFromCenter: 2);
                    chargeTimer = 0;
                    isCharging = false;
                }
            }
            else
            {
                if (NPC.DistanceSQ(Target.Center) > 100000)
                {
                    Vector2 velAcc = dirToTarget * acceleration;
                    NPC.velocity += velAcc;
                    NPC.velocity.Y *= Math.Abs(NPC.Center.Y - Target.Center.Y) > 300 ? 1 : 0.97f;
                }
                else
                {
                    isCharging = true;
                }
            }

            
            if (NPC.frameCounter == 22) SoundEngine.PlaySound(SoundID.Item32, NPC.Center);

            NPC.velocity = Vector2.Clamp(NPC.velocity, Vector2.One * -maxSpeed, Vector2.One * maxSpeed);
            NPC.rotation = NPC.velocity.X * 0.07f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.Hellwings>(), 50));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            NPC.SpawnGoreOnDeath("HellSpawnGore1", "HellSpawnGore2", "HellSpawnGore3");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Underworld.Chance * 0.3f;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.BasicAnimation(frameHeight, 5);
        }
    }
}
