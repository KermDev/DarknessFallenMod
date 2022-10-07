using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs
{
    public class Dargon : ModNPC
    {
        Player Target => Main.player[NPC.target];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3;

            NPCID.Sets.TrailCacheLength[Type] = 10;
            NPCID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 25;
            NPC.defense = 20;
            NPC.lifeMax = 450;
            NPC.value = 67f;
            NPC.netAlways = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        enum AIPhase
        {
            CloseIn,
            Circle
        }

        AIPhase aiPhase;
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            else
            {
                switch (aiPhase)
                {
                    case AIPhase.CloseIn:
                        CloseIn();
                        break;
                    case AIPhase.Circle:
                        Circle();
                        break;
                }
            }

            if (Main.rand.NextBool(7)) Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Shadowflame).noGravity = true;
        }

        ref float curRot => ref NPC.ai[0];
        int projectileTimer;
        const float distFromPlayer = 350;
        const int projectileCd = 15;
        void Circle()
        {
            Vector2 lerpPos = (curRot + additionalRot).ToRotationVector2() * distFromPlayer + Target.Center;
            NPC.Center = Vector2.Lerp(NPC.Center,  lerpPos, 0.15f);

            Vector2 dirToTarget = NPC.Center.DirectionTo(Target.Center);
            NPC.rotation = dirToTarget.ToRotation();

            if (projectileTimer > projectileCd)
            {
                projectileTimer = 0;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + dirToTarget * 5, dirToTarget * 12, ModContent.ProjectileType<DargonProjectile>(), 14, 1);
                }
            }

            projectileTimer++;

            curRot += 0.04f;
            if (curRot > MathHelper.TwoPi + additionalRot)
            {
                curRot = 0;
                aiPhase = AIPhase.CloseIn;
            }
        }

        const float moveSpeed = 7;
        const float inertia = 8.5f;

        const int circleCd = 250;
        const float distSQToCircle = 100000f;
        const int dashCd = 40;
        ref float additionalRot => ref NPC.ai[1];
        ref float circleTimer => ref NPC.ai[2];
        int dashTimer;
        bool openMouth;
        void CloseIn()
        {
            float distSQtoTarg = NPC.Center.DistanceSQ(Target.Center);
            Vector2 dirToTarg = NPC.DirectionTo(Target.Center);

            Vector2 velToTarg = dirToTarg * moveSpeed;
            NPC.velocity = (NPC.velocity * (inertia - 1) + velToTarg) / inertia;
            NPC.spriteDirection = Math.Sign(NPC.velocity.X);

            NPC.rotation = NPC.spriteDirection == 1 ? NPC.velocity.ToRotation() : NPC.velocity.ToRotation() - MathHelper.Pi;

            if (distSQtoTarg < 35000f)
            {
                openMouth = true;

                if (distSQtoTarg < 15000f && dashTimer >= dashCd)
                {
                    dashTimer = 0;

                    NPC.velocity += dirToTarg * 26;
                    SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, NPC.Center);
                }

                dashTimer++;
            }
            else openMouth = false;

            if (distSQtoTarg < distSQToCircle && circleTimer >= circleCd)
            {
                openMouth = true;

                circleTimer = 0;

                NPC.spriteDirection = 1;
                NPC.velocity *= 0;

                additionalRot = Target.Center.DirectionTo(NPC.Center).ToRotation();
                aiPhase = AIPhase.Circle;
                
            }

            circleTimer++;
        }

        const int animSpeed = 6;
        public override void FindFrame(int frameHeight)
        {
            if (openMouth)
            {
                NPC.frameCounter = 2 * animSpeed;
            }
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 2 * animSpeed) NPC.frameCounter = 0;
            }

            NPC.frame.Y = (int)NPC.frameCounter / animSpeed * frameHeight;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.MeleeWeapons.UmbralEdge>(), 100));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 origin = new Vector2(32, 26);

            if (openMouth) NPC.DrawAfterImageNPC(prog => (prog * NPC.oldPos.Length % 2 == 0 ? Color.WhiteSmoke : Color.BlueViolet) * 0.5f, origin: origin);

            DarknessFallenUtils.DrawNPCInHBCenter(NPC, drawColor, origin: origin);

            return false;
        }
    }

    public class DargonProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;

            ProjectileID.Sets.TrailCacheLength[Type] = 12;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.alpha = 34;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.InfernoFork);

            Projectile.BasicAnimation(5);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileInHBCenter(Color.White, true, origin: new Vector2(15, 3));

            return false;
        }
    }
}
