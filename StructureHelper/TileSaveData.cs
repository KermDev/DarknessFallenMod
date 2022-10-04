using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.ModLoader.IO;

namespace DarknessFallenMod.StructureHelper
{
    public struct TileSaveData : TagSerializable
    {
        public ushort wallType;
        public ushort tileType;

        public TileSaveData(Tile tile)
        {
            wallType = tile.WallType;
            tileType = tile.TileType;
        }

        public TagCompound SerializeData()
        {
            return new TagCompound()
            {
                ["wallType"] = wallType,
                ["tileType"] = tileType
            };
        }

        public static TileSaveData DeserializeData(TagCompound tag)
        {
            return new TileSaveData()
            {
                wallType = tag.Get<ushort>("wallType"),
                tileType = tag.Get<ushort>("tileType")
            };
        }
    }
}
