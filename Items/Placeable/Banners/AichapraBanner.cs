using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using DarknessFallenMod.NPCs;

namespace DarknessFallenMod.Items.Placeable.Banners
{
	class AichapraBanner : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Banners.AichapraBannerTile>();
			Item.width = 10;
			Item.height = 24;
			Item.value = 500;
			Item.rare = ItemRarityID.Blue;


		}
	}
}
