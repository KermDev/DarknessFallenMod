using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    public class HolyGreatswordProjectile : ModProjectile
    {
        Player Player => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
        }

        public override void SetDefaults()
        {
            Projectile.width = 73;
            Projectile.height = 73;

            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.light = 0.4f;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 66;
            Projectile.ownerHitCheck = true;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 3;
        }

        float goBackAngle = MathHelper.PiOver2 * 1.5f;
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - (goBackAngle * Player.direction);
        }

        float swingSpeed;
        const float stopFramePercent = 0.5f;
        int stopFrames => (int)(Player.itemAnimationMax * stopFramePercent);
        int swingFrames => Player.itemAnimationMax - stopFrames;
        public override void AI()
        {
            Player.heldProj = Projectile.whoAmI;

            if (Player.ItemAnimationEndingOrEnded)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter, true);
            if (Player.itemAnimation > stopFrames)
            {
                swingSpeed = MathF.Pow(MathF.Sin(MathHelper.Pi * (Player.itemAnimation - stopFrames) / swingFrames), 2);
                Projectile.rotation += swingSpeed * Player.direction * 0.14f;
            }

            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            /*
            Vector2 rotVector = Projectile.rotation.ToRotationVector2();
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Projectile.Center + rotVector * Main.rand.NextFloat(40, swordResize * 100 + 100), 1, 1, DustID.TreasureSparkle, Scale: 0.4f);
            }
            */
        }

        float swordResize => swingSpeed * 0.6f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float normalBladeLenght = 100;
            Vector2 bladeDir = Projectile.rotation.ToRotationVector2();
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + bladeDir * 30, Projectile.Center + bladeDir * (normalBladeLenght + normalBladeLenght * swordResize));
        }

        VertexStrip vtx = new VertexStrip();
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;

            Vector2 offset = Vector2.One * swordResize;
            Vector2 positionOffset = Projectile.rotation.ToRotationVector2() * 53 + new Vector2(-10, 0) * Player.direction;
            positionOffset += positionOffset * swordResize;

            /*
            Main.spriteBatch.End();
            Main.spriteBatch.BeginWithShaderOptions();

            GameShaders.Misc["MagicMissile"]
                .UseSaturation(-3f)
                .UseOpacity(2f)
                .Apply();

            vtx.PrepareStrip(Projectile.oldPos,
                Projectile.oldRot,
                prog => Color.Lerp(Color.Red, Color.LightYellow, Utils.GetLerpValue(0, 1, prog)),
                prog => 60f,
                positionOffset - Main.screenPosition,
                Projectile.oldPos.Length,
                true
                );

            vtx.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.End();
            Main.spriteBatch.BeginWithDefaultOptions();
            */

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition + positionOffset,
                null,
                lightColor,
                Projectile.rotation + MathHelper.PiOver4,
                tex.Size() * 0.5f,
                Vector2.One + offset,
                SpriteEffects.None,
                0
                );


            return false;
        }
    }
}
