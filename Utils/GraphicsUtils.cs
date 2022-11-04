using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI.Chat;

using static DarknessFallenMod.Systems.CoroutineSystem;

namespace DarknessFallenMod.Utils
{
    public static partial class DarknessFallenUtils
    {
        #region SpriteBatch
        public enum BeginType
        {
            /// <inheritdoc cref="BeginDefault(SpriteBatch)"/>
            Default,
            /// <inheritdoc cref="BeginShader(SpriteBatch)"/>
            Shader,
            /// <inheritdoc cref="BeginExperimental(SpriteBatch)"/>
            Experimental
        }

        /*
        public struct BeginData
        {
            SpriteSortMode sortMode;
            BlendState blendState;
            SamplerState samplerState;
            DepthStencilState depthStencil;
            RasterizerState rasterizerState;
            SpriteViewMatrix viewMatrix;
        }
        */

        /// <summary>
        /// Ends the current spritebatch, begins it with <paramref name="beginType"/>, invokes <paramref name="action"/>, ends the spritebatch and begins it with <paramref name="resetBeginType"/>
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="beginType"></param>
        /// <param name="resetBeginType"></param>
        /// <param name="action"></param>
        public static void BeginReset(this SpriteBatch spriteBatch, BeginType beginType, BeginType resetBeginType, Action<SpriteBatch> action)
        {
            spriteBatch.End();
            spriteBatch.Begin(beginType);

            action.Invoke(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(resetBeginType);
        }

        /// <summary>
        /// Begins spritebatch with the specified <paramref name="beginType"/>
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to begin</param>
        /// <param name="beginType"></param>
        public static void Begin(this SpriteBatch spriteBatch, BeginType beginType)
        {
            switch (beginType)
            {
                case BeginType.Default:
                    spriteBatch.BeginDefault();
                    break;
                case BeginType.Shader:
                    spriteBatch.BeginShader();
                    break;
                case BeginType.Experimental:
                    spriteBatch.BeginExperimental();
                    break;
            }
        }

        /// <summary>
        /// Begins the spritebatch with <see cref="SpriteSortMode.Immediate"/>, <see cref="BlendState.AlphaBlend"/>, <see cref="Main.DefaultSamplerState"/>, <see cref="Main.Rasterizer"/>, no effect and <see cref="Main.GameViewMatrix"/><c>.TransformationMatrix</c>
        /// </summary>
        /// <param name="spritebatch">The spritebatch to begin</param>
        public static void BeginShader(this SpriteBatch spritebatch, Effect fx = null)
        {
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, fx, Main.GameViewMatrix.TransformationMatrix);
        }

        /// <summary>
        /// Begins the spritebatch with <see cref="SpriteSortMode.Deferred"/>, <see cref="BlendState.AlphaBlend"/>, <see cref="Main.DefaultSamplerState"/>, <see cref="Main.Rasterizer"/>, no effect and <c>Main.GameViewMatrix.TransformationMatrix</c>
        /// </summary>
        /// <param name="spritebatch">The spritebatch to begin</param>
        public static void BeginDefault(this SpriteBatch spritebatch)
        {
            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        /// <summary>
        /// Begins the spritebatch with <see cref="SpriteSortMode.BackToFront"/> and <see cref="BlendState.AlphaBlend"/>
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to begin</param>
        public static void BeginExperimental(this SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
        }

        /// <summary>
        /// Begins the spritebatch with <see cref="SpriteSortMode.BackToFront"/> and <see cref="BlendState.Additive"/>
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to begin</param>
        public static void BeginAdditive(this SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Additive);
        }
        #endregion

        #region Entity Draw Code
        public static void DrawProjectileInHBCenter(this Projectile projectile, Color lightColor, bool animated = false, Vector2? offset = null, Vector2? origin = null, Texture2D altTex = null, bool centerOrigin = false, float rotOffset = 0)
        {
            Texture2D texture = altTex ?? TextureAssets.Projectile[projectile.type].Value;

            Vector2 drawOrigin;
            Rectangle? sourceRectangle = null;
            if (animated)
            {
                int frameHeight = texture.Height / Main.projFrames[projectile.type];

                drawOrigin = origin ?? (centerOrigin ? new Vector2(texture.Width / 2, frameHeight / 2) : new Vector2(texture.Width, frameHeight / 2));

                sourceRectangle = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight);
            }
            else
            {
                drawOrigin = origin ?? (centerOrigin ? texture.Size() * 0.5f : new Vector2(texture.Width, texture.Height / 2));
            }

            Vector2 drawPos = projectile.Center - Main.screenPosition;
            if (offset.HasValue) drawPos += offset.Value.RotatedBy(projectile.rotation);

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                sourceRectangle,
                lightColor * Math.Clamp((255 - projectile.alpha) / 255f, 0f, 1f),
                projectile.rotation + rotOffset,
                drawOrigin,
                projectile.scale,
                projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0
                );
        }

        public static void DrawNPCInHBCenter(this NPC npc, Color color, Vector2? origin = null, Texture2D altTex = null)
        {
            Texture2D texture = altTex is null ? TextureAssets.Npc[npc.type].Value : altTex;

            Vector2 drawPos = npc.Center - Main.screenPosition;
            Vector2 drawOrigin = origin ?? npc.frame.Size() * 0.5f;

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                npc.frame,
                color * Math.Clamp((255 - npc.alpha) / 255f, 0f, 1f),
                npc.rotation,
                drawOrigin,
                npc.scale,
                npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0
                );
        }

        public static void DrawAfterImage(this Projectile projectile, Func<float, Color> color, bool transitioning = true, bool animated = false, bool centerOrigin = true, Vector2? origin = null, Func<int, Vector2> posOffset = null, Func<int, float> rotOffset = null, Vector2 scaleOffset = default, bool oldRot = true, bool oldPos = true, Texture2D altTex = null)
        {
            Texture2D tex = altTex ?? TextureAssets.Projectile[projectile.type].Value;

            int frameHeight = tex.Height / Main.projFrames[projectile.type];
            Rectangle? source = animated ? new Rectangle(0, frameHeight * projectile.frame + 1, tex.Width, frameHeight) : null;

            Vector2 drawOrigin = origin ?? (centerOrigin ? new Vector2(tex.Width * 0.5f, frameHeight * 0.5f) : tex.Size() * 0.5f);

            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 pos = oldPos ? projectile.oldPos[i] : projectile.position;

                pos += posOffset?.Invoke(i) ?? Vector2.Zero;

                Main.EntitySpriteDraw(
                    tex,
                    pos + new Vector2(projectile.width, projectile.height) * 0.5f - Main.screenPosition,
                    source,
                    transitioning ? color.Invoke((float)i / projectile.oldPos.Length) * ((float)(projectile.oldPos.Length - i) / projectile.oldPos.Length) : color.Invoke((float)i / projectile.oldPos.Length),
                    (oldRot ? projectile.oldRot[i] : projectile.rotation) + (rotOffset?.Invoke(i) ?? 0),
                    drawOrigin,
                    projectile.scale * Vector2.One + scaleOffset,
                    projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    0
                    );
            }
        }

        public static void DrawAfterImageNPC(this NPC npc, Func<float, Color> color, bool transitioning = true, bool centerOrigin = true, Vector2? origin = null, Vector2 posOffset = default, float rotOffset = 0, Vector2 scaleOffset = default, bool oldRot = true, bool oldPos = true, Texture2D altTex = null)
        {
            Texture2D tex = altTex ?? TextureAssets.Npc[npc.type].Value;

            int frameHeight = tex.Height / Main.npcFrameCount[npc.type];

            Vector2 drawOrigin = origin ?? (centerOrigin ? new Vector2(tex.Width * 0.5f, frameHeight * 0.5f) : tex.Size() * 0.5f);

            for (int i = 0; i < npc.oldPos.Length; i++)
            {
                Vector2 pos = oldPos ? npc.oldPos[i] : npc.position;

                pos += posOffset;

                Main.EntitySpriteDraw(
                    tex,
                    pos + new Vector2(npc.width, npc.height) * 0.5f - Main.screenPosition,
                    npc.frame,
                    transitioning ? color.Invoke((float)i / npc.oldPos.Length) * ((float)(npc.oldPos.Length - i) / npc.oldPos.Length) : color.Invoke((float)i / npc.oldPos.Length),
                    (oldRot ? npc.oldRot[i] : npc.rotation) + rotOffset,
                    drawOrigin,
                    npc.scale * Vector2.One + scaleOffset,
                    npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    0
                    );
            }
        }

        #endregion

        #region Tooltip Draw Code
        public enum TooltipLineEffectStyle
        {
            Epileptic
        }

        public static void DrawTooltipLineEffect(DrawableTooltipLine line, int x, int y, TooltipLineEffectStyle effectStyle)
        {
            switch (effectStyle)
            {
                case TooltipLineEffectStyle.Epileptic:
                    EpilepticEffect(line, new Vector2(x, y));
                    break;
            }
        }

        static void EpilepticEffect(DrawableTooltipLine line, Vector2 position)
        {
            float ind = 0.1f;
            for (int i = 0; i < 10; i++)
            {
                //float val = MathF.Abs(MathF.Sin(Main.GameUpdateCount * 0.05f + ind));
                float val = ind;
                ChatManager.DrawColorCodedStringWithShadow(
                    Main.spriteBatch,
                    line.Font,
                    line.Text,
                    position,
                    new Color(Main.rand.NextFloat(), Main.rand.NextFloat(), Main.rand.NextFloat()) * 0.5f * val,
                    0,
                    line.Origin,
                    Vector2.UnitX * val + Vector2.One
                    );
                ind += 0.1f;
            }

        }

        #endregion

        public static void DrawPoint(this Point point, int timeInFrames)
        {
            StartCoroutine(EDrawPoint(point, timeInFrames), CoroutineType.PostDrawTiles);
        }

        public static void DrawRect(this Rectangle rect, int frameTime)
        {
            DrawPoint(rect.TopLeft().ToPoint(), frameTime);
            DrawPoint(rect.TopRight().ToPoint(), frameTime);
            DrawPoint(rect.BottomLeft().ToPoint(), frameTime);
            DrawPoint(rect.BottomRight().ToPoint(), frameTime);
        }
    }
}
