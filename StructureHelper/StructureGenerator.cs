using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DarknessFallenMod.StructureHelper
{
    public class StructureGenerator : ModPlayer
    {
        static Dictionary<string, TagCompound> loadedStructures = new();

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (!StructureSaver.active) return;

            if (PlayerInput.Triggers.JustReleased.QuickMount)
            {
                Point pos = Main.MouseWorld.ToTileCoordinates();
                GenerateStructure("CobbleStructure", pos.X, pos.Y);
            }
        }

        public static void GenerateStructure(string name, int x, int y)
        {
            TagCompound structureTag;
            if (loadedStructures.ContainsKey(name))
            {
                structureTag = loadedStructures[name];
            }
            else
            {
                Stream fileStream = DarknessFallenMod.Instance.GetFileStream("Structures/" + name);

                structureTag = TagIO.FromStream(fileStream);
                fileStream.Close();

                loadedStructures[name] = structureTag;
            }

            int width = structureTag.Get<int>("width");
            int height = structureTag.Get<int>("height");

            var tileSaveDataList = (List<TagCompound>)structureTag.GetList<TagCompound>("tileSaveData");

            int tileIndex = 0;
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    var tileData = tileSaveDataList[tileIndex];

                    PlaceTagTile(i, j, tileData);

                    tileIndex++;
                }
            }

            if (StructureSaver.active) Main.NewText("Structure Placed.");
        }

        static void PlaceTagTile(int i, int j, TagCompound tileTag)
        {
            ushort tileType = tileTag.Get<ushort>("tileType");
            int tileStyle = tileTag.Get<int>("tileStyle");
            short tileFrameX = tileTag.Get<short>("tileFrameX");
            short tileFrameY = tileTag.Get<short>("tileFrameY");

            ushort wallType = tileTag.Get<ushort>("wallType");
            int wallFrameX = tileTag.Get<int>("wallFrameX");
            int wallFrameY = tileTag.Get<int>("wallFrameY");

            Tile tile = Main.tile[i, j];

            //tile.Get<TileWallWireStateData>().HasTile = true;
            //tile.TileType = tileType;
            if (tileType != 0)
            {
                WorldGen.ReplaceTile(i, j, tileType, tileStyle);
            }
            
            tile.TileFrameX = tileFrameX;
            tile.TileFrameY = tileFrameY;
            tile.WallType = wallType;
            tile.WallFrameX = wallFrameX;
            tile.WallFrameY = wallFrameY;
        }
    }
}
