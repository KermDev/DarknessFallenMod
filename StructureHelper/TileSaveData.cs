using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace DarknessFallenMod.StructureHelper
{
    public struct TileSaveData : TagSerializable
    {
        public ushort wallType;
        public int wallFrameX;
        public int wallFrameY;

        public ushort tileType;
        public int tileStyle;
        public short tileFrameX;
        public short tileFrameY;

        public TileSaveData(Tile tile)
        {
            wallType = tile.WallType;
            tileType = tile.TileType;
            tileStyle = TileObjectData.GetTileStyle(tile);

            tileFrameX = tile.TileFrameX;
            tileFrameY = tile.TileFrameY;

            wallFrameX = tile.WallFrameX;
            wallFrameY = tile.WallFrameY;
        }

        public TagCompound SerializeData()
        {
            return new TagCompound()
            {
                ["wallType"] = wallType,
                ["tileType"] = tileType,
                ["wallFrameY"] = wallFrameY,
                ["wallFrameX"] = wallFrameX,
                ["tileStyle"] = tileStyle,
                ["tileFrameX"] = tileFrameX,
                ["tileFrameY"] = tileFrameY
            };
        }
    }
}
