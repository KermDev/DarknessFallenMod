using DarknessFallenMod.Items.Placeable.Banners;
using DarknessFallenMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DarknessFallenMod.Tiles.Banners
{
	public class AridBeastBannerTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.Banners, 0));
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(255, 0, 0), CreateMapEntryName());
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				int type = ModContent.NPCType<AridBeast>();
				Main.SceneMetrics.hasBanner = true;
				Main.SceneMetrics.NPCBannerBuff[type] = true;
			}
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<AridBeastBanner>());
		}
	}
}