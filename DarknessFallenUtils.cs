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

namespace DarknessFallenMod
{
    public static class DarknessFallenUtils
    {
        public const string OreGenerationMessage = "Darkness Fallen Ore Generation";
        public const string FlyingCastleGenMessage = "Building things in the sky";
        public const string SoundsPath = "DarknessFallenMod/Sounds/";

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
                    BeginDefault(spriteBatch);
                    break;
                case BeginType.Shader:
                    BeginShader(spriteBatch);
                    break;
                case BeginType.Experimental:
                    BeginExperimental(spriteBatch);
                    break;
            }
        }

        /// <summary>
        /// Begins the spritebatch with <see cref="SpriteSortMode.Immediate"/>, <see cref="BlendState.AlphaBlend"/>, <see cref="Main.DefaultSamplerState"/>, <see cref="Main.Rasterizer"/>, no effect and <see cref="Main.GameViewMatrix"/><c>.TransformationMatrix</c>
        /// </summary>
        /// <param name="spritebatch">The spritebatch to begin</param>
        public static void BeginShader(this SpriteBatch spritebatch)
        {
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
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

        public static void DrawAfterImage(this Projectile projectile, Func<float, Color> color, bool transitioning = true, bool animated = false, bool centerOrigin = true, Vector2? origin = null, Func<int ,Vector2> posOffset = null, Func<int, float> rotOffset = null, Vector2 scaleOffset = default, bool oldRot = true, bool oldPos = true, Texture2D altTex = null)
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

        /// <summary>
        /// Invokes <paramref name="predicate"/> on the npcs inside <paramref name="rectangle"/>
        /// </summary>
        /// <param name="rectangle">The rectangle area to find NPCs on</param>
        /// <param name="predicate">The action to invoke on each npc</param>
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

        public static bool TryGetClosestEnemyNPC(Vector2 center, out NPC closest, float rangeSQ = float.MaxValue, bool checkChase = true)
        {
            return TryGetClosestEnemyNPC(center, out closest, npc => false, out _, rangeSQ, checkChase);
        }

        public static bool TryGetClosestEnemyNPC(Vector2 center, out NPC closest, Func<NPC, bool> condition, float rangeSQ = float.MaxValue, bool checkChase = true)
        {
            return TryGetClosestEnemyNPC(center, out closest, condition, out _, rangeSQ, checkChase);
        }

        public static bool TryGetClosestEnemyNPC(Vector2 center, out NPC closest, out float distSQ, float rangeSQ = float.MaxValue, bool checkChase = true)
        {
            return TryGetClosestEnemyNPC(center, out closest, npc => false, out distSQ, rangeSQ, checkChase);
        }

        public static bool TryGetClosestEnemyNPC(Vector2 center, out NPC closest, Func<NPC, bool> condition, out float distSQ, float rangeSQ = float.MaxValue, bool checkChase = true)
        {
            closest = null;
            NPC closestCondition = null;
            float minDistCondition = rangeSQ;
            float minDist = rangeSQ;
            foreach (NPC npc in Main.npc)
            {
                if (checkChase)
                {
                    if (!npc.CanBeChasedBy()) 
                        continue;
                }
                else if (npc.life <= 0 || !npc.active || npc.friendly) 
                    continue;

                float dist = npc.DistanceSQ(center);

                if (condition(npc) && dist < minDistCondition)
                {
                    closestCondition = npc;
                    minDistCondition = dist;
                }
                else if (dist < minDist)
                {
                    closest = npc;
                    minDist = dist;
                }
            }
            if (closestCondition is not null)
            {
                closest = closestCondition;
                distSQ = minDistCondition;
            }
            else
            {
                distSQ = minDist;
            }

            return closest is not null;
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
            if (NPC.killCount[npc.type] % killCount == 0 && !(NPC.killCount[npc.type] % 50 == 0)) 
                Item.NewItem(npc.GetSource_Death(), npc.Hitbox, bannerItem);
        }

        public static void ShakeScreenInRange(float strenght, Vector2 center, float rangeSQ, float desolve = 0.95f)
        {
            foreach (Player player in Main.player)
            {
                if (player.DistanceSQ(center) < rangeSQ)
                {
                    player.GetModPlayer<DarknessFallenPlayer>().ShakeScreen(strenght, desolve);
                }
            }
        }

        public static void BasicAnimation(this Projectile proj, int speed)
        {
            BasicAnimation(proj, speed, 0, 0);
        }

        public static void BasicAnimation(this Projectile proj, int speed, int delay, int delayFrame)
        {
            proj.frameCounter++;
            if (proj.frameCounter >= Main.projFrames[proj.type] * speed + delay)
            {
                proj.frameCounter = 0;
            }

            if (proj.frameCounter > delay) proj.frame = (int)(proj.frameCounter - delay) / speed;
            else proj.frame = delayFrame;
        }

        /// <inheritdoc cref="BasicAnimation(NPC, int, int, int, int)"/>
        public static void BasicAnimation(this NPC npc, int frameHeight, int speed)
        {
            BasicAnimation(npc, frameHeight, speed, 0, 0);
        }

        /// <summary>
        /// Does a basic vertical animation through a spritesheet on the npc
        /// </summary>
        /// <param name="npc">The NPC to animate</param>
        /// <param name="frameHeight">Height of each frame on the spritesheet</param>
        /// <param name="speed">Time per frame</param>
        /// <param name="delay">Delay until the npc animation starts</param>
        /// <param name="delayFrame">The frame to use while the animation is starting</param>
        public static void BasicAnimation(this NPC npc, int frameHeight, int speed, int delay, int delayFrame)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= Main.npcFrameCount[npc.type] * speed + delay)
            {
                npc.frameCounter = 0;
            }

            if (npc.frameCounter > delay) 
                npc.frame.Y = ((int)(npc.frameCounter - delay) / speed) * frameHeight;
            else
                npc.frame.Y = delayFrame * frameHeight;
        }

        public static Dust[] NewDustCircular(
            Vector2 center,
            int dustType,
            float radius,
            Vector2 dustVelocity = default,
            int alpha = 0,
            Color color = default,
            float scale = 1, int amount = 8,
            float rotation = 0,
            float speedFromCenter = 0
            )
        {
            Dust[] dusts = new Dust[amount];
            int i = 0;
            foreach(Vector2 pos in GetCircularPositions(center, radius, amount, rotation))
            {

                Vector2 velocity = dustVelocity;
                velocity += center.DirectionTo(pos) * speedFromCenter;
                dusts[i] = Dust.NewDustDirect(pos, 0, 0, dustType, velocity.X, velocity.Y, alpha, color, scale);
                i++;
            }

            return dusts;
        }

        public static void NewGoreCircular(Vector2 center, int goreType, float radius, Vector2 goreVelocity = default, float scale = 1, int amount = 4, float rotation = 0, float speedFromCenter = 0, IEntitySource source = null)
        {
            foreach (Vector2 pos in GetCircularPositions(center, radius, amount, rotation))
            {
                Vector2 velocity = goreVelocity;
                velocity += center.DirectionTo(pos) * speedFromCenter;
                Gore.NewGore(source, pos, velocity, goreType, scale);
            }
        }

        public static void SpawnGoreOnDeath(this NPC npc, float speed = 2.5f, params string[] goreNames)
        {
            if (npc.life <= 0)
            {
                foreach (string name in goreNames)
                {
                    int gore = ModContent.Find<ModGore>(name).Type;
                    Gore.NewGore(npc.GetSource_Death(), npc.position, Main.rand.NextVector2Unit() * speed + npc.velocity, gore);
                }
            }
        }

        public static void SpawnGoreOnDeath(this NPC npc, params string[] names)
        {
            if (Main.netMode == NetmodeID.Server) return;

            if (npc.life <= 0)
            {
                foreach (string name in names)
                {
                    int gore = npc.ModNPC.Mod.Find<ModGore>(name).Type;
                    Gore.NewGore(npc.GetSource_Death(), npc.position, Main.rand.NextVector2Unit() * 2.5f, gore);
                }
            }
        }

        public static void ManualFriendlyLocalCollision(this Projectile projectile)
        {
            if (projectile.friendly)
            {
                ForeachNPCInRectangle(projectile.Hitbox, npc =>
                {
                    if (!npc.friendly && npc.active && npc.life > 0 && projectile.localNPCImmunity[npc.whoAmI] <= 0)
                    {
                        npc.StrikeNPC(projectile.damage, projectile.knockBack * 0.03f, (int)(npc.Center.X - projectile.Center.X));
                        projectile.localNPCImmunity[npc.whoAmI] = projectile.localNPCHitCooldown;
                        projectile.penetrate--;
                    }
                });
            }
        }

        /// <summary>
        /// Formats the specified string with the color chat tag as "[c/<paramref name="color"/>.Hex3():<paramref name="text"/>]"
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetColored(this string text, Color color)
        {
            return $"[c/{color.Hex3()}:{text}]";
        }

        /// <summary>
        /// Gets the oldest projectile owned by <paramref name="player"/>
        /// </summary>
        /// <param name="player"></param>
        /// <param name="projType"></param>
        /// <returns></returns>
        public static Projectile GetOldestProjectile(this Player player, int projType)
        {
            Projectile[] playerProjs = Main.projectile.Where(proj => proj.owner == player.whoAmI && proj.type == projType && proj.active).ToArray();
            if (playerProjs.Length < 1) return null;

            Projectile oldest = playerProjs[0];
            foreach (Projectile proj in playerProjs)
            {
                if (proj.timeLeft < oldest.timeLeft)
                {
                    oldest = proj;
                }
            }
            
            return oldest;
        }

        /// <summary>
        /// Extension for <see cref="Array.ForEach{T}(T[], Action{T})"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="predicate"></param>
        public static void ForEach<T>(this T[] array, Action<T> predicate)
        {
            T[] other = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                predicate.Invoke(array[i]);
            }
        }

        public static T[] ForEach<T>(this T[] array, Func<T, T> predicate)
        {
            T[] other = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                other[i] = predicate.Invoke(array[i]);
            }
            return other;
        }

        public static float InverseLerp(float raw, float min, float max)
        {
            return (raw - min) / (max - min);
        }

        public static float Map(float value, float min, float max, float newMin, float newMax)
        {
            return (value - min) / (max - min) * (newMax - newMin) + newMin;
        }

        /// <summary>
        /// Invokes <paramref name="predicate"/> on each point inside <paramref name="rect"/> with the specified <paramref name="xStep"/>, <paramref name="yStep"/>
        /// </summary>
        /// <param name="rect">The rectangle</param>
        /// <param name="predicate">The action to invoke on each point inside <paramref name="rect"/></param>
        /// <param name="xStep"></param>
        /// <param name="yStep"></param>
        /// <returns>The input rectangle</returns>
        public static Rectangle Foreach(this Rectangle rect, Action<int, int> predicate, int xStep = 1, int yStep = 1)
        {
            int iEnd = rect.X + rect.Width;
            int jEnd = rect.Y + rect.Height;

            for (int i = rect.X; i < iEnd; i += xStep)
            {
                for (int j = rect.Y; j < jEnd; j += yStep)
                {
                    predicate.Invoke(i, j);
                }
            }

            return rect;
        }

        public static void ResetTilesFrame(int i, int j)
        {
            for (int ii = i - 1; ii < i + 2; ii++)
            {
                for (int jj = j - 1; jj < j + 2; jj++)
                {
                    if ((ii == i && jj == j) || ii > Main.maxTilesX || ii < 0 || jj > Main.maxTilesY || jj < 0) continue;
                    WorldGen.TileFrame(ii, jj);
                }
            }
            WorldGen.TileFrame(i, j, true);
        }

        // unfinished
        public static void FramingTileRunner(int i, int j, ushort type, int strenght, int steps, params int[] ignoreTiles)
        {
            int curI = i;
            int curJ = j;
            for (int k = 0; k < steps; k++)
            {
                for (int l = curI - strenght; l < curI + strenght; l++)
                {
                    for (int z = curJ - strenght; z < curJ + strenght; z++)
                    {
                        Tile tileLZ = Framing.GetTileSafely(l, z);
                        float dist = new Vector2(l, z).DistanceSQ(new Vector2(curI, curJ));
                        float chance = Math.Clamp(10 * InverseLerp(dist, 0, strenght * strenght), 1, 10); 
                        if (Main.rand.NextBool((int)chance) && !ignoreTiles.Contains(tileLZ.TileType))
                        {
                            tileLZ.Get<TileWallWireStateData>().HasTile = true;
                            tileLZ.TileType = type;
                            ResetTilesFrame(l, z);
                        }
                    }

                    curI += (int)Main.rand.NextFloatDirection();
                    curJ += (int)Main.rand.NextFloatDirection();
                }
            }
        }

        public static int HitDirection(this Projectile projectile, Vector2 other)
        {
            return MathF.Sign(other.X - projectile.Center.X); //Math.Sign(projectile.Center.DirectionTo(other).X);
        }

        public static IEnumerator DrawCustomAnimation(
            Texture2D texture,
            Func<int, Vector2> positionOnScreen,
            int frames,
            int frequency,
            Func<int, Color> color = null,
            Vector2? origin = null,
            Func<int, float> rotation = null,
            float scale = 1f,
            SpriteEffects spriteEffects = SpriteEffects.None,
            Action<int> onFrame = null
            )
        {
            Vector2 texSize = texture.Size();
            int sourceHeight = (int)texSize.Y / frames;
            Vector2 drawOrigin = origin ?? texSize * 0.5f;

            int currFrame = 0;
            while (currFrame < frames)
            {
                for (int i = 0; i < frequency; i++)
                {
                    Main.spriteBatch.Begin(BeginType.Default);
                    Main.EntitySpriteDraw(
                        texture,
                        positionOnScreen.Invoke(currFrame),
                        new Rectangle(0, currFrame * sourceHeight, (int)texSize.X, sourceHeight),
                        color?.Invoke(currFrame) ?? Color.White,
                        rotation?.Invoke(currFrame) ?? 0,
                        drawOrigin,
                        scale,
                        spriteEffects,
                        0
                        );
                    Main.spriteBatch.End();
                    yield return null;
                }

                onFrame?.Invoke(currFrame);
                currFrame++;
            }
        }
        
        public static void DrawPoint(this Point point, int timeInFrames)
        {
            StartCoroutine(EDrawPoint(point, timeInFrames), CoroutineType.PostDrawTiles);
        }

        public static IEnumerator EDrawPoint(Point point, int timeInFrames)
        {
            Texture2D lineTex = new Texture2D(Main.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

            lineTex.SetData(new Color[] { Color.White });

            int width = 1;

            point -= Main.screenPosition.ToPoint();
            int pointX = point.X - (int)(Main.screenWidth * 0.5f);
            int pointY = point.Y - (int)(Main.screenWidth * 0.5f);

            for (int i = 0; i < timeInFrames; i++)
            {
                Main.spriteBatch.BeginDefault();

                Main.spriteBatch.Draw(lineTex, new Rectangle(pointX, point.Y, Main.screenWidth, width), Color.Red);
                Main.spriteBatch.Draw(lineTex, new Rectangle(point.X, pointY, width, Main.screenWidth), Color.Red);

                Main.spriteBatch.End();

                yield return null;
            }
        }

        /// <summary>
        /// Creates an array of different type from another array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="array"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T2[] CreateFrom<T, T2>(this T[] array, Func<T, T2> predicate)
        {
            T2[] other = new T2[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                other[i] = predicate(array[i]);
            }

            return other;
        }
    }
}
