using DarknessFallenMod.Items;
using DarknessFallenMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.WorldBuilding;
using System.Linq;
using System;
using DarknessFallenMod.Utils;

namespace DarknessFallenMod
{
	public class DarknessFallenWorld : ModSystem
	{
		string FlyingCastleTiles = "";

		public override void PreUpdateWorld()
		{
			if (Main.time == 0 && Main.dayTime)
			{
				for (int i = 0; i < Main.player.Length; i++)
				{
					try
                    {
						Main.player[i].GetModPlayer<DarknessFallenPlayer>().HasRodGambled = false;
						Main.player[i].GetModPlayer<DarknessFallenPlayer>().GambleRodBuff = -1;
					}
					catch
                    {
						return;
                    }
				}
			}
		}

		public override void PostWorldGen()
		{
			// chest style 1 is gold chest
			SpawnItemsInChest(new int[][]
			{
				SetChestItem(ModContent.ItemType<Items.Accessories.HellFlame>(), 12, 1, 1),
				SetChestItem(ModContent.ItemType<Items.Throwables.Gearspark>(), 4, 1, 5, 18),
				SetChestItem(ModContent.ItemType<Items.Accessories.BrokenGlove>(), 12, 1, 1),
				SetChestItem(ModContent.ItemType<Items.Materials.FungiteBar>(), 20, 32, 1, 5), //mushroom chest
				SetChestItem(ModContent.ItemType<Items.MagicWeapons.FungiteStaff>(), 100, 32, 1) //mushroom chest
			});
		}

		void SpawnItemsInChest(int[][] items)
		{
			

			foreach (Chest chest in Main.chest)
			{
				if (chest == null) continue;

				for (int i = 0; i < items.Length; i++)
				{
					int chestStyle = items[i][2];

					if (TileObjectData.GetTileStyle(Main.tile[chest.x, chest.y]) == chestStyle)
					{
						int itemType = items[i][0];
						int itemChanceDenominator = items[i][1];
						int itemStack = items[i].Length > 4 ? Main.rand.Next(items[i][3], items[i][4]) : items[i][3];

						int chestIndex = Array.FindIndex(chest.item, item => item.type == ItemID.None);

						if (chestIndex >= 0 && Main.rand.NextBool(itemChanceDenominator))
						{
							Item item = chest.item[chestIndex];

							item.SetDefaults(itemType);
							item.stack = Math.Clamp(itemStack, 1, item.maxStack);
						}
					}
				}

			}
		}

		int[] SetChestItem(int itemType, int chanceDenominator, int chestStyle, int itemStack)
		{
			return new int[] { itemType, chanceDenominator, chestStyle, itemStack };
		}

		int[] SetChestItem(int itemType, int chanceDenominator, int chestStyle, int itemStackMin, int itemStackMax)
		{
			return new int[] { itemType, chanceDenominator, chestStyle, itemStackMin, itemStackMax };
		}

		// 3. We use the ModifyWorldGenTasks method to tell the game the order that our world generation code should run
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			// 4. We use FindIndex to locate the index of the vanilla world generation task called "Shinies". This ensures our code runs at the correct step.
			int FloatingIsland = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Island Houses"));
			if (FloatingIsland != -1)
			{
				// 5. We register our world generation pass by passing in a name and the method that will execute our world generation code.	
				tasks.Insert(FloatingIsland + 1, new PassLegacy(DarknessFallenUtils.FlyingCastleGenMessage, FlyingCastleGeneration));
			}
		}

		// 6. This is the actual world generation code.
		private void FlyingCastleGeneration(GenerationProgress progress, GameConfiguration configuration)
		{
			// 7. Setting a progress message is always a good idea. This is the message the user sees during world generation and can be useful for identifying infinite loops.      
			progress.Message = DarknessFallenUtils.FlyingCastleGenMessage;


			// 8. Here we use a for loop to run the code inside the loop many times. This for loop scales to the product of Main.maxTilesX, Main.maxTilesY, and 2E-05. 2E-05 is scientific notation and equal to 0.00002. Sometimes scientific notation is easier to read when dealing with a lot of zeros.
			// 9. In a small world, this math results in 4200 * 1200 * 0.00002, which is about 100. This means that we'll run the code inside the for loop 100 times. This is the amount Crimtane or Demonite will spawn. Since we are scaling by both dimensions of the world size, the ammount spawned will adjust automatically to different world sizes for a consistent distribution of ores.
			//for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 6E-05); k++)
			//{
			// 10. We randomly choose an x and y coordinate. The x coordinate is choosen from the far left to the far right coordinates. The y coordinate, however, is choosen from between WorldGen.worldSurfaceLow and the bottom of the map. We can use this technique to determine the depth that our ore should spawn at.
			int x = new Random().Next(60, Main.maxTilesX - 150);
			int y = 68;

			// 11. Finally, we do the actual world generation code. In this example, we use the WorldGen.TileRunner method. This method spawns splotches of the Tile type we provide to the method. The behavior of TileRunner is detailed in the Useful Methods section below.
			//	WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), TileID.CobaltBrick);
			//}		

			using (Stream fileStream = Mod.GetFileStream("Structures/FlyingCastleTiles.txt"))
			using (StreamReader reader = new StreamReader(fileStream))
			{
				FlyingCastleTiles = reader.ReadToEnd();
			}

			Systems.StructureGeneration.GenerateStructureTlies(x, y, 65, 36, FlyingCastleTiles);
		}
	}
}