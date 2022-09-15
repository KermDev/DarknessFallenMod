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
    public class MagmiteOreTile : ModTile
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

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Magmite Ore");
			AddMapEntry(new Color(152, 171, 198), name);

			DustType = 25;
			ItemDrop = ModContent.ItemType<Items.Placeable.Ores.MagmiteOre>();
			HitSound = SoundID.Tink;
			MineResist = 3f;
			MinPick = 100;
		}

        public override void FloorVisuals(Player player)
        {
			player.AddBuff(BuffID.Burning, 10);
        }
    }

	public class MagmiteOreSystem : ModSystem
	{
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int UnderWorldIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Underworld"));

			if (UnderWorldIndex != -1)
			{
				tasks.Insert(UnderWorldIndex + 1, new MagmiteOrePass(DarknessFallenUtils.OreGenerationMessage, 237.4298f));
			}
		}
	}

	public class MagmiteOrePass : GenPass
	{
        public MagmiteOrePass(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
			progress.Message = DarknessFallenUtils.OreGenerationMessage;

			for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 0.00002f); i++)
            {
				int x = WorldGen.genRand.Next(0, Main.maxTilesX);
				int y = WorldGen.genRand.Next(Main.UnderworldLayer, Main.maxTilesY);

				Tile tile = Framing.GetTileSafely(x, y);
				if (tile.HasTile && tile.TileType == TileID.Ash)
                {
					WorldGen.TileRunner(x, y, WorldGen.genRand.Next(6, 7), WorldGen.genRand.Next(1, 2), ModContent.TileType<MagmiteOreTile>());
				}
            }
		}
    }
}
