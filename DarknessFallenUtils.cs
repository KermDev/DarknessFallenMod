using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI.Chat;

namespace DarknessFallenMod
{
    public static class DarknessFallenUtils
    {
        public const string OreGenerationMessage = "Darkness Fallen Ore Generation";

        public static void BeginWithShaderOptions(this SpriteBatch spritebatch)
        {
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void BeginWithDefaultOptions(this SpriteBatch spritebatch)
        {
            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void DrawProjectileInHBCenter(this Projectile projectile, Color lightColor, bool animated = false, Vector2? offset = null, Vector2? origin = null, Texture2D altTex = null)
        {
            Texture2D texture = altTex ?? TextureAssets.Projectile[projectile.type].Value;

            Vector2 drawOrigin;
            Rectangle? sourceRectangle = null;
            if (animated)
            {
                int frameHeight = texture.Height / Main.projFrames[projectile.type];

                drawOrigin = origin ?? new Vector2(texture.Width, frameHeight / 2);

                sourceRectangle = new Rectangle(0, frameHeight * projectile.frame + 1, texture.Width, frameHeight);
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
                projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
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

        public static void ForeachNPCInRange(Vector2 center, float rangeSquared, Action<NPC> predicate)
        {
            Array.ForEach(Main.npc, npc =>
            {
                if (npc.DistanceSQ(center) <= rangeSquared)
                {
                    predicate.Invoke(npc);
                }
            });
        }

        public static void ForeachNPCInRectangle(Rectangle rectangle, Action<NPC> predicate)
        {
            Array.ForEach(Main.npc, npc =>
            {
                if (npc.Hitbox.Intersects(rectangle))
                {
                    predicate.Invoke(npc);
                }
            });
        }

        public static bool TryGetClosestEnemyNPC(Vector2 center, out NPC closest, float rangeSQ = float.MaxValue)
        {
            return TryGetClosestEnemyNPC(center, out closest, npc => true, rangeSQ);
        }

        public static bool TryGetClosestEnemyNPC(Vector2 center, out NPC closest, Func<NPC, bool> condition, float rangeSQ = float.MaxValue)
        {
            closest = null;
            NPC closestCondition = null;
            float minDist = rangeSQ;
            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy() && npc.DistanceSQ(center) < minDist)
                {
                    if (condition(npc))
                    {
                        closestCondition = npc;
                    }
                    else
                    {
                        closest = npc;
                    }
                }
            }

            closest = closestCondition ?? closest;

            if (closest is null) return false;
            return true;
        }

        public static Vector2[] GetCircularPositions(this Vector2 center, float radius, int amount = 8, float rotation = 0)
        {
            if (amount < 2) return Array.Empty<Vector2>();

            Vector2[] postitions = new Vector2[amount];

            float angle = MathHelper.Pi * 2f / amount;
            angle += rotation;

            for (int i = 0; i < amount; i++)
            {
                Vector2 position = new Vector2(MathF.Cos(angle * i), MathF.Sin(angle * i));
                position *= radius;
                position += center;

                postitions[i] = position;
            }


            return postitions;
        }

        public static void SetTrophy(this ModTile modTile)
        {
            Main.tileFrameImportant[modTile.Type] = true;
            Main.tileLavaDeath[modTile.Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.addTile(modTile.Type);

            TileID.Sets.DisableSmartCursor[modTile.Type] = true;
            TileID.Sets.FramesOnKillWall[modTile.Type] = true;
        }

        public static void DropCustomBannerKillCount(this NPC npc, int killCount, int bannerItem)
        {
            if (NPC.killCount[npc.type] % killCount == 0 && !(NPC.killCount[npc.type] % 50 == 0)) Item.NewItem(npc.GetSource_Death(), npc.Hitbox, bannerItem);
        }
    }
}
