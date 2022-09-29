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
                    SpawnDarknessBiome((int)npc.Center.X, 400, 170);
                }
            }
        }

        public static void SpawnDarknessBiome(int x, int width, int height)
        {
            int diff = 100;
            int rectX;

            if (Main.rand.NextBool(2))
            {
                rectX = Main.rand.Next(x / 16 + diff, Main.maxTilesX - width);
            }
            else
            {
                rectX = Main.rand.Next(0, x / 16 - diff - width);
            }

            //int rectY = Main.maxTilesY - height;
            int rectY = Main.UnderworldLayer;

            int darknessTile = ModContent.TileType<Tiles.Terrain.DarknessAshTile>();

            for (int i = rectX; i < rectX + width; i++)
            {
                for (int j = rectY; j < Main.maxTilesY; j++)
                {
                    /*
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
                    */

                    Tile tile = Framing.GetTileSafely(i, j);

                    tile.ClearEverything();
                }
            }
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
            for (int i = rectX; i < rectX + width; i += Main.rand.Next(4, 32))
            {
                bool top = Main.rand.NextBool(2);
                WorldGen.TileRunner(i, top ? rectY : Main.maxTilesY, Main.rand.Next(13, 40), Main.rand.Next(8, 15), darknessTile, speedY: top ? 100 : -100, addTile: true);
            }
        }
    }
}
