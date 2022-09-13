using DarknessFallenMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Potions
{
	public class NaturePotionBig : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Big Nature Potion"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("has infinite uses");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{


			Item.width = 40;
			Item.height = 40;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.value = 17234;
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item3;
			Item.autoReuse = true;
			Item.potion = true;
			Item.consumable = false;
			Item.healLife = 35;
            Item.maxStack = 1;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Bottle, 1);
            recipe.AddIngredient<SoulOfNature>(15);
            recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
}