using DarknessFallenMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Potions
{
	public class NaturePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Potion"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{


			Item.width = 40;
			Item.height = 40;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.value = 1692;
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item3;
			Item.autoReuse = true;
			Item.potion = true;
			Item.consumable = true;
			Item.healLife = 35;
            Item.maxStack = 30;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Bottle, 1);
            recipe.AddIngredient<SoulOfNature>(1);
            recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
}