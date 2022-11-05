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
        public const string OreGenerationMessage = "Darkness Fallen Ore Generation";
        public const string FlyingCastleGenMessage = "Building things in the sky";
        public const string SoundsPath = "DarknessFallenMod/Sounds/";


        public static void OffsetShootPos(ref Vector2 position, Vector2 velocity, Vector2 offset, bool noHitCheck = false)
        {
            Vector2 shootOffset = offset.RotatedBy(velocity.ToRotation());
            if (noHitCheck || Collision.CanHit(position, 5, 5, position + shootOffset, 5, 5))
            {
                position += shootOffset;
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

        public static void ForeachPlayerInRange(Vector2 center, float rangeSquared, Action<Player> predicate)
        {
            Array.ForEach(Main.player, player =>
            {
                if (player.DistanceSQ(center) <= rangeSquared)
                {
                    predicate.Invoke(player);
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
            proj.BasicAnimation(speed, 0, 0);
        }

        public static void BasicAnimation(this Projectile proj, int speed, int delay, int delayFrame)
        {
            proj.frameCounter++;
            if (proj.frameCounter >= Main.projFrames[proj.type] * speed + delay)
            {
                proj.frameCounter = 0;
            }

            if (proj.frameCounter > delay) proj.frame = (proj.frameCounter - delay) / speed;
            else proj.frame = delayFrame;
        }

        /// <inheritdoc cref="BasicAnimation(NPC, int, int, int, int)"/>
        public static void BasicAnimation(this NPC npc, int frameHeight, int speed)
        {
            npc.BasicAnimation(frameHeight, speed, 0, 0);
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
                npc.frame.Y = (int)(npc.frameCounter - delay) / speed * frameHeight;
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
            foreach (Vector2 pos in center.GetCircularPositions(radius, amount, rotation))
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
            foreach (Vector2 pos in center.GetCircularPositions(radius, amount, rotation))
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
                    if (ii == i && jj == j || ii > Main.maxTilesX || ii < 0 || jj > Main.maxTilesY || jj < 0) continue;
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

        /// <summary>
        /// Checks if there is solid terrain at specified world position is
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool SolidTerrain(Vector2 position)
        {
            Point tileCoord = position.ToTileCoordinates();
            Tile tile = Main.tile[tileCoord.X, tileCoord.Y];

            return tile.HasTile && Main.tileSolid[tile.TileType] && (tile.BlockType == BlockType.Solid || (tile.IsHalfBlock && (position.Y % (tileCoord.Y * 16) > 8) && tile.BlockType == BlockType.HalfBlock));
        }

        public static bool SolidTerrain(Rectangle rect)
        {
            for (int i = rect.X; i < rect.X + rect.Width; i += 4)
            {
                for (int j = rect.Y; j < rect.Y + rect.Height; j += 4)
                {
                    //Point tileCoords = new Vector2(i, j).ToTileCoordinates();

                    Tile tile = Main.tile[i / 16, j / 16];
                    if (tile.HasTile && Main.tileSolid[tile.TileType] && !tile.IsHalfBlock || ((tile.IsHalfBlock && (j * 16 + 8 < rect.Bottom) && tile.BlockType == BlockType.HalfBlock)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static Rectangle MovedBy(this Rectangle rect, Vector2 offset)
        {
            return new Rectangle(rect.X + (int)offset.X, rect.Y + (int)offset.Y, rect.Width, rect.Height);
        }

        public static void RemoveMana(this Player player, int mana)
        {
            player.manaRegenDelay = (int)(0.7f * ((1f - (player.statMana / player.statManaMax)) * 240 + 45));
            player.statMana -= mana;
        }
    }
}
