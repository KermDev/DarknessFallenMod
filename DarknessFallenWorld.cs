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
					Player player = Main.player[i];

					if (player is not null)
                    {
						player.GetModPlayer<DarknessFallenPlayer>().HasRodGambled = false;
						player.GetModPlayer<DarknessFallenPlayer>().GambleRodBuff = -1;
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

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int FloatingIsland = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Island Houses"));
			if (FloatingIsland != -1)
			{
				tasks.Insert(FloatingIsland + 1, new PassLegacy(DarknessFallenUtils.FlyingCastleGenMessage, FlyingCastleGeneration));
			}
		}

		private void FlyingCastleGeneration(GenerationProgress progress, GameConfiguration configuration)
		{    
			progress.Message = DarknessFallenUtils.FlyingCastleGenMessage;

			int x = new Random().Next(60, Main.maxTilesX - 150);
			int y = 68;


			using (Stream fileStream = Mod.GetFileStream("Structures/FlyingCastleTiles.txt"))
			using (StreamReader reader = new StreamReader(fileStream))
			{
				FlyingCastleTiles = reader.ReadToEnd();
			}

			Systems.StructureGeneration.GenerateStructureTlies(x, y, 65, 36, FlyingCastleTiles);
		}
	}
}