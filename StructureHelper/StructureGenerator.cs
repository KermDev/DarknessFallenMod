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
            Stream fileStream = DarknessFallenMod.Instance.GetFileStream("Structures/" + name);
            TagCompound structureTag = TagIO.FromStream(fileStream);

            int width = structureTag.Get<int>("width");
            int height = structureTag.Get<int>("height");

            TileSaveData[,] tileSaveDataList = structureTag.Get<TileSaveData[,]>("TileSaveData");

            for (int i = x; i < width; i++)
            {
                for (int j = y; j < height; j++)
                {
                    TileSaveData tileData = tileSaveDataList[i, j];

                    WorldGen.PlaceTile(i, j, tileData.tileType);
                }
            }
        }
    }
}
