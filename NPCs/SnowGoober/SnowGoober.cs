using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs.SnowGoober
{
    public class SnowGoober : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snow Goober");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 44;
            NPC.lifeMax = 400;
            NPC.defense = 6;
            NPC.damage = 0;
            NPC.HitSound = SoundID.NPCDeath15;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.buffImmune[BuffID.Frostburn] = true;
            NPC.value = 289f;
            NPC.knockBackResist = 0.15f;
            NPC.noGravity = false;
            NPC.netAlways = true;
            NPC.chaseable = true;
            NPC.lavaImmune = true;
        }

        public override bool PreAI()
        {
            bool expertMode = Main.expertMode;
            NPC.TargetClosest(true);
            NPC.velocity.X = NPC.velocity.X * 0.93f;
            if (NPC.velocity.X > -0.1F && NPC.velocity.X < 0.1F)
                NPC.velocity.X = 0;
            if (NPC.ai[0] == 0)
                NPC.ai[0] = 500f;

            if (NPC.ai[2] != 0 && NPC.ai[3] != 0)
            {
                // Teleport effects: away.
                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                for (int index1 = 0; index1 < 50; ++index1)
                {
                    int newDust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Flare_Blue, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[newDust].velocity *= 3f;
                    Main.dust[newDust].noGravity = true;
                }
                NPC.position.X = (NPC.ai[2] * 16 - (NPC.width / 2) + 8);
                NPC.position.Y = NPC.ai[3] * 16f - NPC.height;
                NPC.velocity.X = 0.0f;
                NPC.velocity.Y = 0.0f;
                NPC.ai[2] = 0.0f;
                NPC.ai[3] = 0.0f;
                // Teleport effects: arrived.
                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                for (int index1 = 0; index1 < 50; ++index1)
                {
                    int newDust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Flare_Blue, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[newDust].velocity *= 3f;
                    Main.dust[newDust].noGravity = true;
                }
            }

            ++NPC.ai[0];

            if (NPC.ai[0] == 100 || NPC.ai[0] == 300)
            {
                NPC.ai[1] = 30f;
                NPC.netUpdate = true;
            }

            bool teleport = false;
            if (NPC.ai[0] >= 500 && Main.netMode != NetmodeID.MultiplayerClient)
                teleport = true;

            if (teleport)
            {
                Teleport();
                NPC.ai[0] = 1;
                NPC.ai[1] = 0;
            }

            if (NPC.ai[1] > 0)
            {
                --NPC.ai[1];
                if (NPC.ai[1] == 15)
                {
                    SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int amountOfProjectiles = 1;
                        int flakenum = Main.rand.Next(3);
                       
                        for (int i = 0; i < amountOfProjectiles; ++i)
                        {
                            if (Main.rand.NextBool(2))
                            {
                                int somedamage = expertMode ? 15 : 30;
                                int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y - 300, 0, 0, ProjectileID.IceBolt, somedamage, 1, Main.myPlayer, 0, 0);
                                Main.projectile[p].hostile = true;
                                Main.projectile[p].friendly = false;
                                Main.projectile[p].tileCollide = false;
                            }
                            else
                            {
                                Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - (NPC.Center - new Vector2(0, 30))) * 4.9f;
                                int somedamage = expertMode ? 17 : 34;
                                int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y - 30, direction.X, direction.Y, ProjectileID.IceBolt, somedamage, 1, Main.myPlayer, 0, 0);
                                Main.projectile[p].hostile = true;
                                Main.projectile[p].friendly = false;
                                Main.projectile[p].tileCollide = false;
                            }
                        }
                    }
                }
            }

            if (Main.rand.NextBool(3))
                return false;
            Dust dust = Main.dust[Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + 2f), NPC.width, NPC.height, DustID.Flare_Blue, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, new Color(), 0.9f)];
            dust.noGravity = true;
            dust.velocity.X = dust.velocity.X * 0.3f;
            dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;

            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
            return false;
        }      

        public void Teleport()
        {
            NPC.ai[0] = 1f;
            int num1 = (int)Main.player[NPC.target].position.X / 16;
            int num2 = (int)Main.player[NPC.target].position.Y / 16;
            int num3 = (int)NPC.position.X / 16;
            int num4 = (int)NPC.position.Y / 16;
            int num5 = 20;
            int num6 = 0;
            bool flag1 = false;
            if (Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) + Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2000.0)
            {
                num6 = 100;
                flag1 = true;
            }
            while (!flag1 && num6 < 100)
            {
                ++num6;
                int index1 = Main.rand.Next(num1 - num5, num1 + num5);
                for (int index2 = Main.rand.Next(num2 - num5, num2 + num5); index2 < num2 + num5; ++index2)
                {
                    if ((index2 < num2 - 4 || index2 > num2 + 4 || (index1 < num1 - 4 || index1 > num1 + 4)) && (index2 < num4 - 1 || index2 > num4 + 1 || (index1 < num3 - 1 || index1 > num3 + 1)) && Main.tile[index1, index2].HasUnactuatedTile)
                    {
                        bool flag2 = true;
                        if ((Main.tile[index1, index2 - 1].LiquidType == LiquidID.Lava))
                            flag2 = false;
                        if (flag2 && Main.tileSolid[(int)Main.tile[index1, index2].TileType] && !Collision.SolidTiles(index1 - 1, index1 + 1, index2 - 4, index2 - 1))
                        {
                            NPC.ai[1] = 20f;
                            NPC.ai[2] = (float)index1;
                            NPC.ai[3] = (float)index2;
                            flag1 = true;
                            break;
                        }
                    }
                }
            }
            NPC.netUpdate = true;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = -NPC.direction;
            NPC.frameCounter++;

            if (NPC.frameCounter % 6f == 5f)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 6)
            {
                NPC.frame.Y = 0;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => NPC.downedBoss3 && (spawnInfo.SpawnTileY > Main.rockLayer && spawnInfo.Player.ZoneSnow) ? 0.035f : 0f;

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UnusedWhiteBluePurple, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Flare_Blue, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
            }          
        }
    }
}
