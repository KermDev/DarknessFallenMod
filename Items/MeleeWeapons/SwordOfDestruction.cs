using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Items.Materials;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class SwordOfDestruction : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword of Destruction"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("The sword powered by destruction souls");
		}

		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.DamageType = DamageClass.Melee;
			Item.width = 2;
			Item.height = 2;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 4300;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient <SoulOfDestruction> (5); 
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}