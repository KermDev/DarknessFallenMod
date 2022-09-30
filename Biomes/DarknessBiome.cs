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
            return player.Center.Y < Main.UnderworldLayer && ModContent.GetInstance<DarknessBiomeSystem>().darknessTileCount >= 40;
        }
    }

    public class DarknessBiomeSystem : ModSystem
    {
        public int darknessTileCount;
        public bool spawnedDarknessBiome;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            darknessTileCount = tileCounts[ModContent.TileType<Tiles.Terrain.DarknessRockTile>()];
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

        public static ushort darknessTile => (ushort)ModContent.TileType<Tiles.Terrain.DarknessRockTile>();
        public static void GenDarknessBiome(int x, int width, int height)
        {
            int i;
            int diff = 100;
            if (Main.rand.NextBool(2))
            {
                i = Main.rand.Next(x / 16 + diff, Main.maxTilesX - width);
            }
            else
            {
                i = Main.rand.Next(0, x / 16 - diff - width);
            }

            int j = Main.UnderworldLayer;

            Rectangle biomeRect = new Rectangle(i, j, width, height);

            GenLayer1(biomeRect);
        }

        public static void GenLayer1(Rectangle rect)
        {
            int maxI = rect.Y + rect.Width;

            int topHeight = (int)(rect.Height * 0.12f);
            int bottomHeight = (int)(rect.Height * 0.36f);

            int bottomJ = Main.maxTilesY - bottomHeight;

            int thresh = 5;

            rect.Foreach((i, j) =>
            {
                Tile tile = Framing.GetTileSafely(i, j);

                tile.Clear(Terraria.DataStructures.TileDataType.Wall);

                tile.LiquidAmount = 0;
                tile.SkipLiquid = true;

                if (j < rect.Y + topHeight || j > bottomJ)
                {
                    if ((j < rect.Y + rect.Height - 5) && (i == rect.X || i == rect.X + rect.Width - 1 || j == rect.Y || j == rect.Y + topHeight - 1 || j == bottomJ + 1))
                    {
                        if (Main.rand.NextBool(3)) return;

                        // thought this helped with tiling idk if it does
                        //if (!tile.HasTile) WorldGen.PlaceTile(i, j, darknessTile);

                        // this doesnt frame the blocks sometimes
                        WorldGen.TileRunner(i, j, Main.rand.Next(5, 7), Main.rand.Next(10, 32), darknessTile, addTile: true, ignoreTileType: darknessTile);

                        // uh yeah this doesnt work, left for later ungrades (maybe)
                        //DarknessFallenUtils.FramingTileRunner(i, j, darknessTile, Main.rand.Next(5, 7), Main.rand.Next(3, 5));
                    }
                    else if (tile.TileType != darknessTile)
                    {
                        tile.Get<TileWallWireStateData>().HasTile = true;
                        tile.TileType = darknessTile;
                        tile.BlockType = BlockType.Solid;
                    }
                }
                else
                {
                    if (!(i < rect.X + thresh || i > rect.X + rect.Width - thresh) || Main.rand.NextBool(4)) tile.ClearEverything();
                }

                // I have no idea if this works now or not
                DarknessFallenUtils.ResetTilesFrame(i, j);
            });
        }

        public static void GenLayer2(Rectangle rect)
        {
            rect.Foreach((i, j) =>
            {

            });
        }

        /*
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
        */
    }
}
