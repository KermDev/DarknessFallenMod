using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.MagmitePitchfork
{
    public class MagmitePitchforkProjectile : ModProjectile
    {
        Player Player => Main.player[Projectile.owner];

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

        Vector2 attackDirection;
        float rotOffset = MathHelper.PiOver4 + MathHelper.PiOver2;
        public override void OnSpawn(IEntitySource source)
        {
            attackDirection = Projectile.velocity.SafeNormalize(Vector2.Zero);
            Projectile.velocity = Vector2.Zero;

            Projectile.rotation = attackDirection.ToRotation();
        }

        float spearLength => 162f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 startCollision = Projectile.Center - rotationDirection * 0.5f * spearLength;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), startCollision, startCollision + rotationDirection * spearLength);
        }

        Vector2 rotationDirection;
        public override void AI()
        {
            Player.heldProj = Projectile.whoAmI;

            if (Player.ItemAnimationEndingOrEnded)
            {
                Projectile.Kill();
                return;
            }

            rotationDirection = Projectile.rotation.ToRotationVector2();

            Projectile.Center = Player.Center;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;

            float rotation = Projectile.rotation + rotOffset;
            Vector2 scale = Vector2.One + (rotation - attackDirection.ToRotation()).ToRotationVector2() * Math.Clamp((float)Player.itemAnimation / Player.itemAnimationMax * 1.5f, 0f, 1f);

            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor, rotation, tex.Size() * 0.5f, scale, SpriteEffects.None, 0);

            return false;
        }
    }
}
