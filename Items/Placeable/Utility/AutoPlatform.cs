using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static DarknessFallenMod.Systems.CoroutineSystem;

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
			StartCoroutine(PlacePlatform(player));
        }

        /*public override void OnConsumeItem(Player player)
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

				Vector2 effectPos = new Vector2(tileX * 16, tileY * 16);
				Dust.NewDust(effectPos, 16, 16, DustID.Smoke);
				Gore.NewGore(null, Main.rand.NextVector2FromRectangle(new Rectangle((int)effectPos.X, (int)effectPos.Y, 16, 16)), Main.rand.NextVector2Unit(), GoreID.Smoke1 + Main.rand.Next(3), Scale: Main.rand.NextFloat(0.5f, 1.3f));
			}
		}*/

        static IEnumerator PlacePlatform(Player player)
        {
			if (Main.netMode == NetmodeID.Server) yield return false;

			Vector2 mousePos = Main.MouseWorld;

			int tileX = (int)(mousePos.X / 16);
			int tileY = (int)(mousePos.Y / 16);

			int dir = player.direction;
			Tile dirTile = Main.tile[tileX + dir, tileY];
			if (dirTile.HasTile) dir *= -1;

			for (int i = 2; i < 81; i++)
			{
				yield return WaitFor.Frames(3);

				tileX += dir;

				if (!WorldGen.PlaceTile(tileX, tileY, TileID.Platforms)) break;

				if (i % 10 == 0)
				{
					int tileYUp = tileY - 1;
					Tile tileUp = Main.tile[tileX, tileYUp];

					if (!tileUp.HasTile)
					{
						WorldGen.PlaceTile(tileX, tileYUp, TileID.Torches);
					}
				}

				Vector2 effectPos = new Vector2(tileX * 16 - 16 + dir * 8, tileY * 16 - 16);
				Dust.NewDust(effectPos, 16, 16, DustID.Smoke);
				Gore.NewGore(null, Main.rand.NextVector2FromRectangle(new Rectangle((int)effectPos.X, (int)effectPos.Y, 16, 16)), Main.rand.NextVector2Unit(), GoreID.Smoke1 + Main.rand.Next(3), Scale: Main.rand.NextFloat(0.5f, 1.3f));
			}
		}

        public override void AddRecipes()
        {
			CreateRecipe()
				.AddRecipeGroup("Platforms", 80)
				.AddRecipeGroup("Torches", 8)
				.AddTile(TileID.WorkBenches)
				.Register();
        }
    }
}
