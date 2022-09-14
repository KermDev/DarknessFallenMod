using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod
{
    public static class DarknessFallenUtils
    {
        public static void BeginWithShaderOptions(this SpriteBatch spritebatch)
        {
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void BeginWithDefaultOptions(this SpriteBatch spritebatch)
        {
            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void DrawProjectileInHBCenter(this Projectile projectile, Color lightColor, bool animated = false, Vector2? offset = null, Vector2? origin = null)
        {
            if (projectile.ModProjectile is null) return;

            Texture2D texture = ModContent.Request<Texture2D>(projectile.ModProjectile.Texture).Value;

            Vector2 drawOrigin;
            Rectangle? sourceRectangle = null;
            if (animated)
            {
                int frameHeight = texture.Height / Main.projFrames[projectile.type];

                drawOrigin = origin ?? new Vector2(texture.Width, frameHeight / 2);

                sourceRectangle = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight);
            }
            else
            {
                drawOrigin = origin ?? new Vector2(texture.Width, texture.Height / 2);
            }

            Vector2 drawPos = projectile.Center - Main.screenPosition;
            if (offset.HasValue) drawPos += offset.Value.RotatedBy(projectile.rotation);

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                sourceRectangle,
                lightColor,
                projectile.rotation,
                drawOrigin,
                projectile.scale,
                SpriteEffects.None,
                0
                );
        }

        public static void OffsetShootPos(ref Vector2 position, Vector2 velocity, Vector2 offset)
        {
            Vector2 shootOffset = offset.RotatedBy(velocity.ToRotation());
            if (Collision.CanHit(position, 5, 5, position + shootOffset, 5, 5))
            {
                position += shootOffset;
            }
        }
    }
}
