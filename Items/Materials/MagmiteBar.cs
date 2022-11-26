using DarknessFallenMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Materials
{
    public class MagmiteBar : ModItem, IGlowmask
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magmite Crystal");
        }

        public override void SetDefaults()
        {
            // Removed until a proper crystal tile is sprited. - Snek
            //Item.useStyle = ItemUseStyleID.Swing;
            //Item.useTurn = true;
            //Item.useAnimation = 15;
            //Item.useTime = 10;
            //Item.autoReuse = true;
            //Item.consumable = true;
            //Item.createTile = ModContent.TileType<MagmiteBarTile>();

            Item.maxStack = 999;
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

    // Removed until a proper crystal tile is sprited. - Snek
    //public class MagmiteBarTile : ModTile
    //   {
    //       public override void SetStaticDefaults()
    //       {
    //		Main.tileShine[Type] = 1100;
    //		Main.tileSolid[Type] = true;
    //		Main.tileSolidTop[Type] = true;
    //		Main.tileFrameImportant[Type] = true;

    //		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
    //		TileObjectData.newTile.StyleHorizontal = true;
    //		TileObjectData.newTile.LavaDeath = false;
    //		TileObjectData.addTile(Type);

    //		ItemDrop = ModContent.ItemType<MagmiteBar>();

    //		var name = CreateMapEntryName();
    //		name.SetDefault("Magmite Bar");

    //		AddMapEntry(new Color(200, 40, 40), name);
    //	}
    //   }
}
