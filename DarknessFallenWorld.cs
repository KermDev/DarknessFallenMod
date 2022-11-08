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
			SpawnItemsInChest(new ChestItem[]
			{
				new ChestItem(ModContent.ItemType<Items.Accessories.HellFlame>(), 12, 1, 1, 1),
				new ChestItem(ModContent.ItemType<Items.Throwables.Gearspark>(), 4, 1, 5, 18),
				new ChestItem(ModContent.ItemType<Items.Accessories.BrokenGlove>(), 12, 1, 1, 1),
				new ChestItem(ModContent.ItemType<Items.Materials.FungiteBar>(), 20, 32, 1, 5), //mushroom chest
				new ChestItem(ModContent.ItemType<Items.MagicWeapons.FungiteStaff>(), 100, 32, 1, 1), //mushroom chest
				new ChestItem(ModContent.ItemType<Items.Materials.SandstoneScales>(), 1, 10, 2, 6, TileID.Containers2) //sandstone chest
			});
		}

		void SpawnItemsInChest(ChestItem[] items)
		{
			foreach (Chest chest in Main.chest)
			{
				if (chest == null) continue;

				for (int i = 0; i < items.Length; i++)
				{
					ChestItem chestItem = items[i];

					Tile chestTile = Main.tile[chest.x, chest.y];

					if (chestTile.TileType == chestItem.chestType && TileObjectData.GetTileStyle(chestTile) == chestItem.chestStyle)
					{
						int itemStack = Main.rand.Next(chestItem.itemStackMin, chestItem.itemStackMax + 1);
						int chestIndex = Array.FindIndex(chest.item, item => item.type == ItemID.None);

						if (chestIndex >= 0 && Main.rand.NextBool(chestItem.itemChanceDenominator))
						{
							Item item = chest.item[chestIndex];

							item.SetDefaults(chestItem.itemType);
							item.stack = Math.Clamp(itemStack, 1, item.maxStack);
						}
					}
				}

			}
		}

		struct ChestItem
        {
			public int itemType;
			public int itemChanceDenominator;
			public int chestStyle;
			public ushort chestType;
			public int itemStackMin;
			public int itemStackMax;

			public ChestItem(int itemType, int itemChanceDenominator, int chestStyle, int itemStackMin, int itemStackMax, ushort chestType = TileID.Containers)
            {
				this.itemType = itemType;
				this.itemChanceDenominator = itemChanceDenominator;
				this.chestStyle = chestStyle;
				this.itemStackMin = itemStackMin;
				this.itemStackMax = itemStackMax;
				this.chestType = chestType;
            }
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