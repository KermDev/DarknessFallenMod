using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
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
        }

        public override void SetDefaults()
        {
            NPC.width = 43;
            NPC.height = 50;
            NPC.damage = 20;
            NPC.defense = 4;
            NPC.lifeMax = 94;
            NPC.value = 67f;
            NPC.netAlways = true;
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
        const float distSQToCircle = 20000;
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
        }

        ref float curRot => ref NPC.ai[0];
        const float distFromPlayer = 400;
        void Circle()
        {
            Vector2 lerpPos = (curRot + additionalRot).ToRotationVector2() * distFromPlayer + Target.Center;
            NPC.Center = Vector2.Lerp(NPC.Center,  lerpPos, 0.2f);

            NPC.rotation = MathHelper.Lerp(NPC.rotation, NPC.Center.DirectionTo(Target.Center).ToRotation(), 0.2f);

            //NPC.velocity *= 0.98f;

            curRot += 0.02f;
            if (curRot > MathHelper.TwoPi + additionalRot)
            {
                curRot = 0;
                aiPhase = AIPhase.CloseIn;
            }
        }

        const float inertia = 12;
        const int circleCd = 600;
        ref float additionalRot => ref NPC.ai[1];
        ref float circleTimer => ref NPC.ai[2];
        void CloseIn()
        {
            Vector2 velToTarg = NPC.DirectionTo(Target.Center) * 13;
            NPC.velocity = (NPC.velocity * (inertia - 1) + velToTarg) / inertia;
            NPC.rotation = NPC.velocity.Y;
            NPC.spriteDirection = Math.Sign(NPC.velocity.X);

            if (circleTimer >= circleCd && NPC.Center.DistanceSQ(Target.Center) < distSQToCircle)
            {
                additionalRot = Target.Center.DirectionTo(NPC.Center).ToRotation();
                NPC.velocity *= 0;
                aiPhase = AIPhase.Circle;
            }

            circleTimer++;
        }
    }
}
