using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Items.Materials;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class BeetleSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beetle Sword");
			Tooltip.SetDefault("Poor Beetles");
		}

		public override void SetDefaults()
		{
			Item.damage = 17;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = 195300;
			Item.rare = 8;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient <RedChitin> (5);
            recipe.AddIngredient<BlueChitin>(5);
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}