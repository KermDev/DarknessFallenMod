using System;
using System.Text;
using System.Collections.Generic;
using Terraria.WorldBuilding;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using System.IO;
using Terraria.ObjectData;

namespace DarknessFallenMod.Systems
{
    public class StructureGeneration
    {
        public static void GetStructureTileData(int x1, int x2, int y1, int y2, string path)
        {
            //gets structure data in a big list, y first then coloums i.e.
            //1,2,3,4,5,6,7,8,9
            //in
            //1 4 7
            //2 5 8
            //3 6 9

            FileStream fs = File.OpenWrite(path);

            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    string TileTypeString = Main.tile[i, j].TileType.ToString();
                    if (TileTypeString == "0")
                    {
                        if (Main.tile[i, j].LiquidType == LiquidID.Water && Main.tile[i, j].LiquidAmount >= 0.2)
                        {
                            TileTypeString = "w";
                        }
                        else if (!Main.tile[i, j].HasTile)
                        {
                            TileTypeString = "a";
                        }
                    }

                    if (TileTypeString == TileID.RubyGemspark.ToString())
                    {
                        TileTypeString = "i";
                    }

                    fs.Write(Encoding.UTF8.GetBytes(TileTypeString));

                    fs.Write(Encoding.UTF8.GetBytes("-"));

                    string TileStyle = "";
                    TileStyle = TileObjectData.GetTileStyle(Main.tile[i, j]).ToString();

                    if (TileStyle == "-1")
                    {
                        TileStyle = "0";
                    }

                    fs.Write(Encoding.UTF8.GetBytes(TileStyle));

                    fs.Write(Encoding.UTF8.GetBytes(","));
                }
            }

            fs.Close();
            //59 is the i size
            //33 is the j size
        }

        public static void GenerateStructureTlies(int posX, int posY, int iSize, int jSize, string Structure)
        {
            char[] structureChars = Structure.ToCharArray();
            //creates a strcutres with the left top point at posX, posY
            //iSize and jSize should be the amount of columns and rows repextavly;

            int iterationPosition = 0; //position of iteration so like the index of the character so where the tile is idk how to explain ask me in dicord;
            int numberLength = 0;
            int styleLength = 0;
            for (int i = 0; i < (iSize * jSize); i++)
            {
                numberLength = 0;
                styleLength = 0;

                int protextion = 0;

                while (structureChars[iterationPosition + numberLength] != '-' || protextion >= 40)
                {
                    numberLength++;
                    protextion++;
                }

                while (structureChars[iterationPosition + numberLength + 1 + styleLength] != ',' || protextion >= 400)
                {
                    styleLength++;
                    protextion++;
                }
                //number legnth is one for a one space number;

                string TileIDS = "";
                string StyleIDS = "";

                for (int NoPos = 0; NoPos < numberLength; NoPos++)
                {
                    TileIDS += structureChars[iterationPosition + NoPos];
                }

                for (int NoPos = 0; NoPos < styleLength; NoPos++)
                {
                    StyleIDS += structureChars[iterationPosition + NoPos + 1 + numberLength];
                }

                int XPos = posX + (int)Math.Floor((float)i / jSize);
                int YPos = posY + (i % jSize);

                if (TileIDS != "i" && TileIDS != "w" && TileIDS != "a")
                {
                    int TileToPlaceID = int.Parse(TileIDS.ToString());

                    if (TileToPlaceID == 2)
                    {
                        TileToPlaceID = 0;
                    }

                    switch (TileIDS)
                    {
                        default:
                            {
                                WorldGen.PlaceTile(XPos, YPos, TileToPlaceID, style: int.Parse(StyleIDS), forced: true);
                                break;
                            }
                        case "79": //bed;
                            {
                                if (Main.tile[XPos, YPos + 2].TileType != TileID.Stone)
                                {
                                    for (int TempFloor = 0; TempFloor < 4; TempFloor++)
                                    {
                                        WorldGen.PlaceTile(XPos + TempFloor, YPos + 2, TileID.Stone);
                                    }
                                }
                                WorldGen.PlaceTile(XPos, YPos, TileToPlaceID, style: int.Parse(StyleIDS));
                                
                                break;
                            }
                        case "21": //chest;
                            {
                                if (Main.tile[XPos, YPos + 2].TileType != TileID.Stone)
                                {
                                    for (int TempFloor = 0; TempFloor < 2; TempFloor++)
                                    {
                                        WorldGen.PlaceTile(XPos + TempFloor, YPos + 2, TileID.Stone);
                                    }
                                }
                                WorldGen.PlaceTile(XPos, YPos, TileToPlaceID, style: int.Parse(StyleIDS));
                                break;
                            }
                        case "104": //grandfatherclock;
                            {
                                if (Main.tile[XPos, YPos + 5].TileType != TileID.Stone)
                                {
                                    for (int TempFloor = 0; TempFloor < 2; TempFloor++)
                                    {
                                        WorldGen.PlaceTile(XPos + TempFloor, YPos + 5, TileID.Stone);
                                    }
                                }
                                WorldGen.PlaceTile(XPos, YPos, TileToPlaceID, style: int.Parse(StyleIDS));
                                break;
                            }
                    }
                }
                else
                {
                    if (TileIDS == "w")
                    {
                        WorldGen.PlaceLiquid(XPos, YPos, LiquidID.Water, 255);
                    }

                    if(TileIDS == "a")
                    {
                        WorldGen.KillTile(XPos, YPos);
                    }
                }

                iterationPosition += numberLength + 2 + styleLength;
            }
        }

        /*Notes:
         * for the flying castle, use 
         *  //GennerateStructureTile(x, y, 65, 36, path)
         * 
         * */
    }
}
