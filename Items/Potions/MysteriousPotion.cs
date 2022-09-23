using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Potions
{
    public class MysteriousPotion : ModItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("[c/666666:Hmm I do wonder what it does...]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

			ItemID.Sets.DrinkParticleColors[Type] = new Color[3] {
				new Color(240, 240, 240),
				new Color(200, 200, 200),
				new Color(140, 140, 140)
			};
		}

		public override void SetDefaults()
		{
			buffs = Enumerable.Range(1, Main.maxBuffTypes - 1).Where(type => !blackListedBuffs.Contains(type) && !Main.buffNoTimeDisplay[type]).ToArray();

			Item.width = 23;
			Item.height = 31;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.value = Item.buyPrice(copper: 67);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.maxStack = 30;
			Item.useTurn = true;
			Item.buffTime = 1800;
			RandomizeBuff();
		}

		int[] blackListedBuffs = new int[] { 19, 27, 28, 34, 40, 41, 42, 45, 49, 50, 51, 52, 53, 54, 55, 56, 57, 64, 65, 66, 81, 82, 83, 84, 85, 90, 91, 92, 101, 102, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 138, 139, 140, 142, 143, 154, 161, 162, 166, 167, 168, 182, 184, 185, 187, 188, 189, 190, 191, 193, 199, 200, 201, 202, 208, 209, 210, 211, 212, 213, 214, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 333, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 258, 259, 260, 261, 262, 263, 264, 265, 266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 302, 303, 304, 305, 317, 318, 325, 327, 328, 329, 330, 331, 335 };
		int[] buffs;

        public override void OnConsumeItem(Player player)
        {
			RandomizeBuff();
		}

		void RandomizeBuff()
        {
			Item.buffType = Main.rand.NextFromList(buffs);
		}

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup("Consumables", 2)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}
}
