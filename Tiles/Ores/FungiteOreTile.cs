using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace DarknessFallenMod.Tiles.Ores
{
	public class FungiteOreTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileOreFinderPriority[Type] = 410;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 975;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileLighted[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Fungite Ore");
			AddMapEntry(new Color(120, 130, 180), name);

			DustType = 25;
			ItemDrop = ModContent.ItemType<Items.Placeable.Ores.MagmiteOre>();
			HitSound = SoundID.Tink;
			MineResist = 3f;
			MinPick = 100;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.7f;
			g = 0.8f;
			b = 1.2f;
		}
	}

	public class FungiteOreSystem : ModSystem
	{
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Mushrooms"));

			if (ShiniesIndex != -1)
			{
				tasks.Insert(ShiniesIndex + 1, new FungiteOrePass(DarknessFallenUtils.OreGenerationMessage, 237.4298f));
			}
		}
	}

	public class FungiteOrePass : GenPass
	{
		public FungiteOrePass(string name, float loadWeight) : base(name, loadWeight)
		{
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = DarknessFallenUtils.OreGenerationMessage;

			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile tile = Framing.GetTileSafely(i, j);

					if (tile.HasTile && tile.TileType == TileID.MushroomTrees)
					{
						for (int k = 0; k < 7; k++)
						{
							int spread = 150;

							int x = i + Main.rand.Next(-spread, spread);
							int y = j + Main.rand.Next(-spread, spread);

							Tile spawnTile = Framing.GetTileSafely(x, y);
							if (spawnTile.HasTile && spawnTile.TileType == TileID.Mud) WorldGen.TileRunner(x, y, Main.rand.Next(3, 9), 4, ModContent.TileType<Tiles.Ores.FungiteOreTile>(), true);
						}
					}
				}
			}
		}
	}
}
