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
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace DarknessFallenMod
{
	public class DarknessFallenWorld : ModSystem
	{
		// We can use PostWorldGen for world generation tasks that don't need to happen between vanilla world generation steps.
		public override void PostWorldGen()
		{
			// Place some items in Ice Chests
			int[] CavernChestItems = { ModContent.ItemType<Items.Accessories.HellFlame>() };
			int CavernChestChoice = 0;
			for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				// If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the 12th chest is the Ice Chest. Since we are counting from 0, this is where 11 comes from. 36 comes from the width of each tile including padding. 
				if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 1 * 36 && chest.y >= Main.maxTilesY - 400)
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (chest.item[inventoryIndex].type == ItemID.None)
						{
							if(Main.rand.Next(0, 21) == 2)
							{
								chest.item[inventoryIndex].SetDefaults(CavernChestItems[CavernChestChoice]);
								CavernChestChoice = (CavernChestChoice + 1) % CavernChestItems.Length;
								// Alternate approach: Random instead of cyclical: chest.item[inventoryIndex].SetDefaults(Main.rand.Next(itemsToPlaceInIceChests));
								break;
							}
						}
					}
				}
			}
		}
	}
}