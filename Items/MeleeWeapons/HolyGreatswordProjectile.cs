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
using System.IO;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    public class HolyGreatswordProjectile : ModProjectile
    {
        Player Player => Main.player[Projectile.owner];

        public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/HolyGreatsword";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
        }

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;

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

        float goBackAngle = MathHelper.PiOver2 * 1.75f;
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - (goBackAngle * Player.direction);
        }

        float swingSpeed;
        const float stopFramePercent = 0.2f;
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
                Projectile.rotation += swingSpeed * Player.direction * 0.11f;
            }

            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            /*
            Vector2 rotVector = Projectile.rotation.ToRotationVector2();
            Vector2 rotVector90 = rotVector.RotatedBy(-MathHelper.PiOver2);
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.Center + rotVector * Main.rand.NextFloat(40, swordResize * 100 + 100) + rotVector90 * 45 * Player.direction, 1, 1, DustID.TerraBlade, Scale: 0.4f);
            }
            */
        }
        public override bool? CanCutTiles()
        {
            return true;
        }

        float swordResize => swingSpeed * 0.6f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float normalBladeLenght = 68;
            Vector2 bladeDir = Projectile.rotation.ToRotationVector2();
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + bladeDir * (normalBladeLenght + normalBladeLenght * swordResize));
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(swingSpeed);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            swingSpeed = reader.ReadSingle();
        }

        VertexStrip vertexStripMM = new VertexStrip();
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D tex = TextureAssets.Projectile[Type].Value;

            Vector2 offset = Vector2.One * swordResize;
            Vector2 positionOffset = Projectile.rotation.ToRotationVector2() * 42 + new Vector2(-8, 0) * Player.direction;
            positionOffset += positionOffset * swordResize;
            /*
            Main.spriteBatch.End();
            Main.spriteBatch.BeginWithShaderOptions();

            GameShaders.Misc["EmpressBlade"]
                .UseShaderSpecificData(new Vector4(1f, 0.0f, 0.0f, 0.6f))
                .Apply(new DrawData?());

            vertexStripMM.PrepareStripWithProceduralPadding(
                Projectile.oldPos,
                Projectile.oldRot,
                prog => Color.Lerp(Color.Red, Color.Yellow * 0.5f, prog),
                prog => 100 + MathHelper.Lerp(100, 1, prog) * swordResize,
                50 * Projectile.rotation.ToRotationVector2() - Main.screenPosition,
                true,
                true
                );

            vertexStripMM.DrawTrail();

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

            Texture2D glowMaskTex = ModContent.Request<Texture2D>("DarknessFallenMod/Items/MeleeWeapons/HolyGreatSwordGlowmask").Value;

            Main.EntitySpriteDraw(
                glowMaskTex,
                Projectile.Center - Main.screenPosition + positionOffset,
                null,
                Color.White,
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
