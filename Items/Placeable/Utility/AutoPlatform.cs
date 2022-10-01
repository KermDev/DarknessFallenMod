using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Placeable.Utility
{
    public class AutoPlatform : ModItem
    {
        public override void SetDefaults()
        {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = TileID.Platforms;
			Item.width = 15;
			Item.height = 15;
			Item.value = Item.buyPrice(silver: 19);
			Item.rare = ItemRarityID.Lime;
		}

        public override void OnConsumeItem(Player player)
        {
			if (Main.netMode == NetmodeID.Server) return;

			Vector2 mousePos = Main.MouseWorld;

			int tileX = (int)(mousePos.X / 16);
			int tileY = (int)(mousePos.Y / 16);

			int dir = player.direction;
			Tile dirTile = Main.tile[tileX + dir, tileY];
			if (dirTile.HasTile) dir *= -1;

			for (int i = 2; i < 101; i++)
			{
				tileX += dir;

				Tile tile = Main.tile[tileX, tileY];

				if (!tile.HasTile)
				{
					WorldGen.PlaceTile(tileX, tileY, TileID.Platforms);

					if (i % 10 == 0)
                    {
						int tileYUp = tileY - 1;
						Tile tileUp = Main.tile[tileX, tileYUp];

						if (!tileUp.HasTile)
                        {
							WorldGen.PlaceTile(tileX, tileYUp, TileID.Torches);
                        }
                    }
				}
                else
                {
					break;
                }
			}
		}
    }
}
