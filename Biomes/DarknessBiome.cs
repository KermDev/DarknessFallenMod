using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            darknessTileCount = tileCounts[ModContent.TileType<Tiles.Terrain.ShadowGrassTile>()];
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            
        }
    }
}
