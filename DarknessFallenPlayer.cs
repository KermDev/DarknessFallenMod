using DarknessFallenMod.Items;
using DarknessFallenMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Generation;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace DarknessFallenMod
{
	public class DarknessFallenPlayer : ModPlayer
	{


		public override void clientClone(ModPlayer clientClone)
		{
			DarknessFallenPlayer clone = clientClone as DarknessFallenPlayer;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)Player.whoAmI);
			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			DarknessFallenPlayer clone = clientPlayer as DarknessFallenPlayer;
		}


        float screenShakeStrenght;
        float screenShakeDesolve;
        public void ShakeScreen(float strenght, float desolve = 0.95f)
        {
            screenShakeStrenght = strenght;
            screenShakeDesolve = Math.Clamp(desolve, 0, 0.9999f);
        }

        public override void ModifyScreenPosition()
        {
            if (screenShakeStrenght > 0.001f)
            {
                Main.screenPosition += screenShakeStrenght * Main.rand.NextVector2Unit();
                screenShakeStrenght *= screenShakeDesolve;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (PlayerInput.Triggers.JustPressed.MouseRight && PlayerInput.Triggers.Current.Down)
            {
                Main.NewText("cccc");
                PlaceOresAtLocation();
            }
        }

        private void PlaceOresAtLocation()
        {
            /*
            for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 0.001f); i++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);

                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile && tile.TileType == TileID.Mud && WorldGen.mushroomBiomesPosition.Any(vector => vector.DistanceSQ(new Vector2(x, y)) < 10000))
                {
                    WorldGen.TileRunner(x, y, 15, 2, ModContent.TileType<Tiles.Ores.FungiteOreTile>());
                }
            }
            */
            /*
            foreach (Vector2 mushroomPos in WorldGen.mushroomBiomesPosition)
            {
                for (int i = 0; i < 1000; i++)
                {
                    int x = (int)mushroomPos.X;
                    int y = (int)mushroomPos.Y;

                    int spread = 150;
                    x += (int)(spread * Main.rand.NextFloatDirection());
                    y += (int)(spread * Main.rand.NextFloatDirection());

                    Tile tile = Framing.GetTileSafely(x, y);
                    if (tile.HasTile && tile.TileType == TileID.Mud)
                    {
                        WorldGen.TileRunner(x, y, 10, 2, ModContent.TileType<Tiles.Ores.FungiteOreTile>(), true);
                    }
                }
            }
            */

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = (int)WorldGen.worldSurfaceLow; j < Main.maxTilesY; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);

                    if (tile.HasTile && tile.TileType == TileID.MushroomTrees)
                    {
                        for (int k = 0; k < 10; k++)
                        {
                            int spread = 100;

                            int x = i + Main.rand.Next(-spread, spread);
                            int y = j + Main.rand.Next(-spread, spread);

                            Tile spawnTile = Framing.GetTileSafely(x, y);
                            if (spawnTile.HasTile && spawnTile.TileType == TileID.Mud) WorldGen.TileRunner(x, y, 4, 7, ModContent.TileType<Tiles.Ores.FungiteOreTile>(), true);
                        }
                    }
                }
            }
        }
    }
}
