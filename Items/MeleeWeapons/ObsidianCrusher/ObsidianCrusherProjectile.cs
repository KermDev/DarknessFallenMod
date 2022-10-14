using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static DarknessFallenMod.Systems.CoroutineSystem;

namespace DarknessFallenMod.Items.MeleeWeapons.ObsidianCrusher
{
    public class ObsidianCrusherProjectile : ModProjectile
    {
        Player Player => Main.player[Projectile.owner];

        public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/ObsidianCrusher/ObsidianCrusher";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 120;
        }

        public override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;

            Projectile.knockBack = 8;

            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 9999;
            Projectile.ownerHitCheck = true;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 3;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * Player.itemAnimationMax - 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - (MathHelper.Pi * Player.direction);
        }

        Vector2 rotatedDirection;
        float rotVel;

        bool shouldSlowDown;
        public override void AI()
        {
            if (Player.ItemAnimationEndingOrEnded)
            {
                Projectile.Kill();
                return;
            }

            Player.heldProj = Projectile.whoAmI;
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter);

            if (!shouldSlowDown) rotVel = (MathF.Cos(MathHelper.PiOver2 * ((float)Player.itemAnimation / Player.itemAnimationMax) + MathHelper.PiOver2) + 1) * 0.3f * (ObsidianCrusher.speedMult != 1 ? 0.42f : 1f);
            if (Player.itemAnimation < Player.itemAnimationMax * 0.4f)
            {
                if (ObsidianCrusher.speedMult != 1f)
                {
                    Vector2 hitPoint = Player.MountedCenter + rotatedDirection * bladeLenght + new Vector2(-8, 10 * Player.direction).RotatedBy(Projectile.rotation);
                    Vector2 hitPoint2 = Player.MountedCenter + rotatedDirection * bladeLenght + new Vector2(-23, 10 * Player.direction).RotatedBy(Projectile.rotation);
                    Vector2 hitPoint3 = Player.MountedCenter + rotatedDirection * bladeLenght + new Vector2(-38, 10 * Player.direction).RotatedBy(Projectile.rotation);

                    //hitPoint.ToPoint().DrawPoint(2);
                    //hitPoint2.ToPoint().DrawPoint(2);
                    //hitPoint3.ToPoint().DrawPoint(2);

                    bool hitTile = Collision.SolidTiles(hitPoint, 1, 1) || Collision.SolidTiles(hitPoint2, 1, 1) || Collision.SolidTiles(hitPoint3, 1, 1);
                    if (hitTile && !shouldSlowDown)
                    {
                        rotVel *= -0.3f;

                        shouldSlowDown = true;
                        StartCoroutine(EShootBoulders(hitPoint, Player.direction));
                    }
                }
                else if (Player.itemAnimation < 10) 
                {
                    shouldSlowDown = true;
                    rotVel *= 0.8f;
                }

            }

            Projectile.rotation += rotVel * Player.direction;

            rotatedDirection = Projectile.rotation.ToRotationVector2();

            Projectile.direction = Player.direction;
            Projectile.spriteDirection = Projectile.direction;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (ObsidianCrusher.speedMult != 1) damage = (int)(damage * 1.4f);
        }

        float bladeLenght => TextureAssets.Projectile[Type].Value.Width * 1.7f;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Player.MountedCenter, Player.MountedCenter + rotatedDirection * bladeLenght);
        }

        VertexStrip vrtx = new();

        Vector2[] drawPos = new Vector2[120]; 
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 texSize = tex.Size();

            Vector2 drawPosition = Player.MountedCenter - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + rotatedDirection * bladeLenght * 0.5f;
            for (int i = 0; i < drawPos.Length - 1; i++)
            {
                drawPos[i + 1] = drawPos[i];
            }
            drawPos[0] = drawPosition;

            /*
            Main.spriteBatch.BeginReset(DarknessFallenUtils.BeginType.Shader, DarknessFallenUtils.BeginType.Default, s =>
            {
                GameShaders.Misc["EmpressBlade"]
                .UseShaderSpecificData(new Vector4(1f, 0.0f, 0.0f, 0.6f))
                .Apply();

                vrtx.PrepareStrip(
                    drawPos,
                    new float[drawPos.Length],
                    prog => Color.Purple * 0.7f,
                    prog => 36f, rotatedDirection * bladeLenght * 0.25f,
                    drawPos.Length,
                    true
                    );
                vrtx.DrawTrail();

                Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            });
            */

            Main.EntitySpriteDraw(
                tex,
                drawPosition,
                null,
                lightColor,
                Projectile.rotation + (Player.direction == -1 ? -MathHelper.Pi - MathHelper.PiOver4 : MathHelper.PiOver4),
                texSize * 0.5f,
                1,
                Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0
                );

            return false;
        }

        IEnumerator EShootBoulders(Vector2 startPos, int direction)
        {
            Vector2 curPoint = startPos;
            for (int i = 1; i < 5; i++)
            {
                if (Collision.SolidTiles(curPoint, 1, 1))
                {
                    while (Collision.SolidTiles(curPoint, 1, 1))
                    {
                        curPoint.Y--;
                    }
                    curPoint.Y += 3;
                }
                else
                {
                    while (!Collision.SolidTiles(curPoint, 1, 1))
                    {
                        curPoint.Y++;
                    }
                    curPoint.Y += 2;
                }

                IEntitySource src = new EntitySource_ItemUse(Player, Player.HeldItem, i.ToString());
                int type = ModContent.ProjectileType<ObsidianCrusherBoulder>();

                if (Player.ownedProjectileCounts[type] > 3)
                {
                    Player.GetOldestProjectile(type).timeLeft = 1;
                }

                Projectile.NewProjectileDirect(src, curPoint, Vector2.UnitY * -10, type, 20, 1, Player.whoAmI);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    DarknessFallenUtils.ShakeScreenInRange(1.25f * i, curPoint, 2560000f, 0.7f);
                }

                SoundEngine.PlaySound(SoundID.Item62, curPoint);

                curPoint.X += 50 * direction;

                yield return WaitFor.Frames(15);
            }
        }
    }
}
