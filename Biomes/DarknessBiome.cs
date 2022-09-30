using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace DarknessFallenMod.Biomes
{
    public class DarknessBiome : ModBiome
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkness Biome");
        }

        public override bool IsBiomeActive(Player player)
        {
            return player.ZoneUnderworldHeight && ModContent.GetInstance<DarknessBiomeSystem>().darknessTileCount >= 40;
        }
    }

    public class DarknessBiomeSystem : ModSystem
    {
        public int darknessTileCount;
        public bool spawnedDarknessBiome;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            darknessTileCount = tileCounts[ModContent.TileType<Tiles.Terrain.DarknessAshTile>()];
        }

        public override void OnWorldLoad()
        {
            Main.NewText(nameof(spawnedDarknessBiome));
            spawnedDarknessBiome = false;
        }

        public override void OnWorldUnload()
        {
            spawnedDarknessBiome = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag[nameof(spawnedDarknessBiome)] = spawnedDarknessBiome;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey(nameof(spawnedDarknessBiome))) spawnedDarknessBiome = tag.GetBool(nameof(spawnedDarknessBiome));
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = spawnedDarknessBiome;

            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();

            spawnedDarknessBiome = flags[0];
        }
    }

    public class DarknessBiomeSpawn : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.WallofFlesh)
            {
                DarknessBiomeSystem system = ModContent.GetInstance<DarknessBiomeSystem>();
                if (!system.spawnedDarknessBiome)
                {
                    system.spawnedDarknessBiome = true;
                    GenDarknessBiome((int)npc.Center.X, 600, 320);
                }
            }
        }

        public static void GenDarknessBiome(int x, int width, int height)
        {
            int diff = 100;
            int rectI;

            if (Main.rand.NextBool(2))
            {
                rectI = Main.rand.Next(x / 16 + diff, Main.maxTilesX - width);
            }
            else
            {
                rectI = Main.rand.Next(0, x / 16 - diff - width);
            }

            //int rectY = Main.maxTilesY - height;
            int rectY = Main.UnderworldLayer;

            int maxI = rectI + width;

            ushort darknessTile = (ushort)ModContent.TileType<Tiles.Terrain.DarknessAshTile>();

            int topHeight = (int)(height * 0.12f);
            int bottomHeight = (int)(height * 0.40f);

            int bottomJ = Main.maxTilesY - bottomHeight;

            Rectangle biomeRect = new Rectangle(rectI, rectY, width, height).Foreach((i, j) =>
            {
                Tile tile = Framing.GetTileSafely(i, j);

                tile.Clear(Terraria.DataStructures.TileDataType.Wall);
                tile.Get<LiquidData>().Amount = 0;

                if (j < rectY + topHeight || j > bottomJ)
                {
                    if ((j < Main.maxTilesY - 5) && (i == rectI || i == rectI + width - 1 || j == rectY || j == rectY + topHeight - 1 || j == bottomJ + 1))
                    {
                        if (Main.rand.NextBool(3)) return;

                        tile.TileType = darknessTile;
                        WorldGen.TileRunner(i, j, Main.rand.Next(2, 7), Main.rand.Next(10, 32), darknessTile, addTile: true, ignoreTileType: darknessTile);
                    }
                    else if (tile.TileType != darknessTile)
                    {
                        tile.Get<TileWallWireStateData>().HasTile = true;
                        tile.TileType = darknessTile;
                        
                        //DarknessFallenUtils.ResetTilesFrame(i, j);
                        //WorldGen.Place1x1(i, j, darknessTile);
                    }
                }
                else
                {
                    tile.ClearEverything();
                }
            });

            /*
            for (int i = rectX; i < maxX; i++)
            {
                for (int j = rectY; j < Main.maxTilesY; j++)
                {
                    
                    Tile tile = Framing.GetTileSafely(i, j);

                    if (tile.TileType == TileID.Ash || tile.TileType == TileID.Hellstone || tile.TileType == TileID.Lavafall)
                    {
                        WorldGen.ReplaceTile(i, j, (ushort)darknessTile, 0);
                    }
                    else if (tile.LiquidType == LiquidID.Lava)
                    {
                        WorldGen.PlaceTile(i, j, darknessTile, true);
                    }
                    else if (tile.HasTile && tile.TileType != darknessTile)
                    {
                        WorldGen.TileRunner(i, j, 7, 3, darknessTile, addTile: true);
                    }

                    if (tile.WallType > 0) WorldGen.KillWall(i, j);
                    if (tile.LiquidType > 0) WorldGen.EmptyLiquid(i, j);
                    

                    
                }
            }*/

            /*
            float minH = rectY + height * 0.4f;
            float maxVal = minH / rectY;
            for (int i = rectX; i < rectX + width; i += 4)
            {
                for (int j = rectY; j < minH; j++)
                {
                    WorldGen.TileRunner(i, j, MathHelper.Lerp(2, 4, DarknessFallenUtils.InverseLerp(minH / j, 1, maxVal)) + Main.rand.Next(5), Main.rand.Next(1, 3), darknessTile, addTile: true);
                }
            }

            int maxH = (int)(rectY + height * 0.6f);
            int maxVal2 = maxH / Main.maxTilesY;
            for (int i = rectX; i < rectX + width; i += 4)
            {
                for (int j = maxH; j < Main.maxTilesY; j++)
                {
                    WorldGen.TileRunner(i, j, MathHelper.Lerp(2, 4, DarknessFallenUtils.InverseLerp(j / Main.maxTilesY, 1, maxVal2)) + Main.rand.Next(5), Main.rand.Next(1, 3), darknessTile, addTile: true);
                }
            }*/
            /*
            new Rectangle(rectX, rectY, width, topHeight).Foreach((i, j) => 
            {
                GenTerrain(i, j, rectX, rectY, topHeight, width, darknessTile);
            });

            int minY = Main.maxTilesY - topHeight * 2;
            new Rectangle(rectX, minY, width, topHeight * 2).Foreach((i, j) =>
            {
                GenTerrain(i, j, rectX, minY, topHeight, width, darknessTile);
            });
            */
            //GenBigSpikes(rectX, rectY, maxX, darknessTile);
        }

        public static void GenTerrain(int i, int j, int rectX, int rectY, int topHeight, int width, int darknessTile)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            if (tile.HasTile && tile.TileType != darknessTile) tile.ClearEverything();
            WorldGen.PlaceTile(i, j, darknessTile);
            if ((j < Main.maxTilesY - 2) && (i <= rectX || j == rectY || j >= rectY + topHeight - 1 || i >= rectX + width - 1))
            {
                WorldGen.TileRunner(i, j, Main.rand.Next(2, 7), Main.rand.Next(5, 32), darknessTile, addTile: true, ignoreTileType: darknessTile);
            }
        }

        public static void GenBigSpikes(int rectX, int rectY, int maxX, int darknessTile)
        {
            for (int i = rectX; i < maxX; i += Main.rand.Next(14, 22))
            {
                bool top = Main.rand.NextBool(2);
                int startPosition = top ? rectY : Main.maxTilesY - 40;

                int dir = Main.rand.NextBool(2) ? 1 : -1;

                float multiplierX = Main.rand.NextFloat(0f, 3f);
                int multiplierY = Main.rand.Next(4, 6);

                int times = Main.rand.Next(7, 34);

                int currPos = startPosition;
                for (int j = 1; j < times; j++)
                {
                    float diffusion = DarknessFallenUtils.InverseLerp((float)times / j, (float)times / (times - 1) , times);

                    WorldGen.TileRunner((int)(i + j * multiplierX * dir), currPos, Main.rand.Next(7, 17), (int)MathHelper.Lerp(6, 32, diffusion) + Main.rand.Next(3), darknessTile, addTile: true, ignoreTileType: darknessTile);

                    currPos += (int)MathHelper.Lerp(1, multiplierY, diffusion) * (top ? 1 : -1);
                }
            }
        }
    }
}
