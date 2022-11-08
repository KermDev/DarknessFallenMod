using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DarknessFallenMod.Items.Materials
{
    public class MagmiteBar : ModItem
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Magmite Crystal");
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
			Item.createTile = ModContent.TileType<MagmiteBarTile>();
			Item.width = 15;
			Item.height = 15;
			Item.value = Item.buyPrice(silver: 32);
			Item.rare = ItemRarityID.LightRed;
		}

		public override bool? CanBurnInLava() => false;

        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Placeable.Ores.MagmiteOre>(), 4)
				.AddTile(TileID.Hellforge)
				.Register();
        }
    }

	public class MagmiteBarTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileShine[Type] = 1100;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);

			ItemDrop = ModContent.ItemType<MagmiteBar>();

			var name = CreateMapEntryName();
			name.SetDefault("Magmite Bar");

			AddMapEntry(new Color(200, 40, 40), name);
		}
    }
}
