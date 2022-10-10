using Microsoft.Xna.Framework;
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
    public class StructureSaver : ModPlayer
    {
        public static bool active = false;

        Point topLeft;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (!active) return;

            if (PlayerInput.Triggers.JustReleased.MouseRight)
            {
                if (topLeft != default)
                {
                    Point bottomRight = Main.MouseWorld.ToTileCoordinates();

                    Rectangle rect = new Rectangle(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X + 1, bottomRight.Y - topLeft.Y + 1);

                    Main.NewText($"Rectangle: {rect.X}, {rect.Y}, {rect.Width}, {rect.Height}");

                    SaveStructure(rect);

                    Main.NewText($"Structure saved.");
                }
                else
                {
                    Main.NewText("Top left selected.");
                    topLeft = Main.MouseWorld.ToTileCoordinates();
                }
            }
        }

        public static void SaveStructure(Rectangle tileRect)
        {
            TagCompound structureCompound = new TagCompound();

            structureCompound["width"] = tileRect.Width;
            structureCompound["height"] = tileRect.Height;

            List<TileSaveData> tileSaveDataList = new();
            tileRect.Foreach((i, j) =>
            {
                tileSaveDataList.Add(new TileSaveData(Main.tile[i, j]));
            });

            structureCompound.Add("tileSaveData", tileSaveDataList);

            string path = ModLoader.ModPath.Replace("Mods", "ModSources\\DarknessFallenMod\\Structures\\Saves\\NewStructure");

            File.Create(path).Close();
            TagIO.ToFile(structureCompound, path);
        }
    }
}
