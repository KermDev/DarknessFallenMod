using DarknessFallenMod.Tiles.Banners;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Placeable.Banners
{
	public class AridBeastBanner : ModItem
	{
		public override void SetStaticDefaults()
		{
			SacrificeTotal = 1;
		}

		public override void SetDefaults()
		{
			Item.Size = new(10, 24);
			Item.DefaultToPlaceableTile(ModContent.TileType<AridBeastBannerTile>());
			Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice(silver: 2));
		}
	}
}