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
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.Items.MeleeWeapons.HolyGreatsword
{
    public class HolyGreatswordProjectile : ModProjectile
    {
        Player Player => Main.player[Projectile.owner];

        public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/HolyGreatsword/HolyGreatsword";

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
            
            Vector2 rotVector = Projectile.rotation.ToRotationVector2();
            Vector2 rotVector90 = rotVector.RotatedBy(-MathHelper.PiOver2);

            Dust.NewDust(Projectile.Center + rotVector * Main.rand.NextFloat(40, SwordResize * 100 + 100) + rotVector90 * 20 * Player.direction, 1, 1, DustID.ShadowbeamStaff, Scale: 0.4f);
        }
        public override bool? CanCutTiles()
        {
            return true;
        }

        float SwordResize => swingSpeed * 0.6f;
        const int bladeLenght = 68;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 bladeDir = Projectile.rotation.ToRotationVector2();
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + bladeDir * (bladeLenght + bladeLenght * SwordResize));
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(swingSpeed);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            swingSpeed = reader.ReadSingle();
        }

        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D tex = TextureAssets.Projectile[Type].Value;

            Vector2 scale = Vector2.One *  (1 + SwordResize);
            Vector2 origin = new Vector2(-18, 67);
            Vector2 posOffset = -Projectile.rotation.ToRotationVector2() * bladeLenght * SwordResize;

            Main.spriteBatch.End();
            Main.spriteBatch.BeginShader();

            //Projectile.DrawAfterImage(prog => Color.Lerp(Color.LightGoldenrodYellow, Color.Black, prog) * 0.1f, origin: origin, posOffset: posOffset, scaleOffset: scale - Vector2.One, rotOffset: MathHelper.PiOver4, oldPos: false);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float prog = (float)i / Projectile.oldPos.Length;
                Color color = Color.Lerp(Color.Gray, Color.White, prog) * 0.1f * ((float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length);

                float oldRot = Projectile.oldRot[i];
                if (oldRot == 0) continue;
                Vector2 posOffset2 = -oldRot.ToRotationVector2() * bladeLenght * SwordResize;

                Main.EntitySpriteDraw(
                    tex,
                    Projectile.Center - Main.screenPosition + posOffset2,
                    null,
                    color,
                    oldRot + MathHelper.PiOver4,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                    );
            }

            Main.spriteBatch.End();
            Main.spriteBatch.BeginDefault();
            

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition + posOffset,
                null,
                lightColor,
                Projectile.rotation + MathHelper.PiOver4,
                origin,
                scale,
                SpriteEffects.None,
                0
                );

            Texture2D glowMaskTex = ModContent.Request<Texture2D>("DarknessFallenMod/Items/MeleeWeapons/HolyGreatsword/HolyGreatswordGlowmask").Value;

            Main.EntitySpriteDraw(
                glowMaskTex,
                Projectile.Center - Main.screenPosition + posOffset,
                null,
                Color.White,
                Projectile.rotation + MathHelper.PiOver4,
                origin,
                scale,
                SpriteEffects.None,
                0
                );

            return false;
        }
    }
}