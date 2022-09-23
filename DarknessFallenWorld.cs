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

namespace DarknessFallenMod
{
	public class DarknessFallenWorld : ModSystem
	{
		
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
			return new int[] { itemType, chanceDenominator, chestStyle, itemStack};
        }

        int[] SetChestItem(int itemType, int chanceDenominator, int chestStyle, int itemStackMin, int itemStackMax)
        {
			return new int[] { itemType, chanceDenominator, chestStyle, itemStackMin, itemStackMax };
		}
	}
}