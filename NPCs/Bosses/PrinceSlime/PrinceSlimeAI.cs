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
    // ???
    // Split it up cause the file was getting too long and too cluttered
    public partial class PrinceSlime
    {
        enum AIState
        {
            Falling,
            Phase1,
            Phase2
        }

        AIState aiState = AIState.Falling;
        Player Target { get; set; }

        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            else
            {
                Target = Main.player[NPC.target];

                switch (aiState)
                {
                    case AIState.Falling:
                        DoFalling();
                        break;
                    case AIState.Phase1:
                        DoPhase1();
                        break;
                    case AIState.Phase2:
                        DoPhase2();
                        break;
                }
            }

            //NPC.Hitbox.DrawRect(1);
        }

        void DoFalling()
        {
            if (NPC.velocity.Y == 0)
            {
                for (int i = 0; i < 25; i++)
                {
                    Dust.NewDust(NPC.Hitbox.BottomLeft(), NPC.width, 2, DustID.TintableDust, SpeedY: -5, SpeedX: Main.rand.Next(-5, 5), Scale: Main.rand.NextFloat(1f, 2.5f), newColor: new Color(0, 255, 80), Alpha: 170);
                }

                foreach (Player player in Main.player)
                {
                    if (player.DistanceSQ(NPC.Center) < 5760000)
                    {
                        player.GetModPlayer<DarknessFallenPlayer>().ShakeScreen(7, 0.94f);
                    }
                }

                SoundEngine.PlaySound(SoundID.Item167, NPC.Center);

                aiState = AIState.Phase1;
            }
        }

        void DoPhase1()
        {
            DoMoving();

            if (NPC.life <= NPC.lifeMax * 0.5f) aiState = AIState.Phase2;
        }

        void DoPhase2()
        {
            DoMoving();
            DoShooting();
        }

        Vector2 directionToTarget;
        int moveDirection;
        void DoMoving()
        {
            directionToTarget = NPC.Center.DirectionTo(Target.Center);
            moveDirection = MathF.Sign(directionToTarget.X);

            if (NPC.wet)
            {
                NPC.velocity.X += moveDirection * 0.3f;
                NPC.velocity.Y -= 0.3f;

                NPC.velocity.X = Math.Clamp(NPC.velocity.X, -1.5f, 1.5f);
                return;
            }

            DoJumping();

            if (Main.rand.NextBool(360)) NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)Target.Center.X + Main.rand.Next(-300, 300), (int)Target.Center.Y - 1200, ModContent.NPCType<PrinceSlimeMinion>()).netUpdate = true;

            if (NPC.collideX && NPC.velocity.Y != 0) NPC.velocity.X += moveDirection * 2;
            if (NPC.collideY) NPC.velocity.X *= 0.92f;
        }

        ref float JumpTimer => ref NPC.ai[0];
        void DoJumping()
        {
            if (NPC.velocity.Y == 0)
            {
                if (JumpTimer >= 90 && MortarTimer > 60)
                {
                    JumpTimer = 0;

                    float xVel = Math.Clamp((Target.Center.X - NPC.Center.X) * 0.05f, -6f, 6f);
                    NPC.velocity.X += xVel;

                    float maxJ = 14f;
                    //float yVel = Collision.SolidTiles(NPC.position + new Vector2(xVel, 2), NPC.width, (int)(NPC.height * 0.2f)) ? maxJ : Math.Clamp((NPC.Center.Y - Target.Center.Y) * 0.5f, 5f, maxJ);
                    float yVel = DarknessFallenUtils.SolidTerrain(NPC.Hitbox.MovedBy(NPC.velocity)) ? maxJ : Math.Clamp((NPC.Center.Y - Target.Center.Y) * 0.5f, 5f, maxJ);
                    NPC.velocity.Y -= yVel;

                }

                JumpTimer++;
            }
        }

        ref float MortarTimer => ref NPC.ai[1];
        Vector2 ShootPosition => NPC.Center + new Vector2(NPC.direction * -3, -11);
        void DoShooting()
        {
            if (MortarTimer <= 0)
            {
                MortarTimer = Main.rand.Next(80, 200);

                if (!Main.dedServ) Lighting.AddLight(ShootPosition, TorchID.Red);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {

                    Vector2 shootFrom = ShootPosition;

                    Vector2 shootAt = Target.Center - shootFrom;
                    shootAt.X = Math.Abs(shootAt.X);

                    float gravity = PrinceSlimeFireballProjectile.GRAVITY;

                    float speed = MathF.Sqrt(gravity * (shootAt.Y + MathF.Sqrt(MathF.Pow(shootAt.Y, 2) + MathF.Pow(shootAt.X, 2)))) + 3f;

                    float angle = MathF.Atan(
                        (MathF.Pow(speed, 2) + MathF.Sqrt(MathF.Pow(speed, 4) - (gravity * (gravity * MathF.Pow(shootAt.X, 2) + 2 * shootAt.Y * MathF.Pow(speed, 2))))) / (gravity * shootAt.X)
                        );

                    int shootDir = Target.Center.X - shootFrom.X < 0 ? -1 : 1;

                    Vector2 initialVel = new Vector2(shootDir, 0).RotatedBy(angle * -shootDir) * speed;

                    Main.NewText(initialVel);

                    Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        shootFrom,
                        initialVel,
                        ModContent.ProjectileType<PrinceSlimeFireballProjectile>(),
                        42,
                        1
                        );

                    SoundEngine.PlaySound(SoundID.Item117, shootFrom);
                }
            }
            else if (MortarTimer < 60)
            {
                foreach (Vector2 pos in ShootPosition.GetCircularPositions(MortarTimer * MortarTimer, 8, MortarTimer * 0.01f))
                {
                    // 21, 75, 304, 301 - useful dust ids
                    Dust.NewDust(pos, 0, 0, DustID.TreasureSparkle, newColor: Color.Red, Scale: 0.5f);
                }
            }

            MortarTimer--;
        }
    }
}
