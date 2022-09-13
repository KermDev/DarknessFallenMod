using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Items.Materials;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class SwordOfBalance : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword of Balance"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Nature and Destruction combined");
		}

		public override void SetDefaults()
		{
			Item.damage = 54;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 15470;
			Item.rare = 8;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SwordOfBalanceProjectile>();
			Item.shootSpeed = 20f;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient <SoulOfNature> (20);
            recipe.AddIngredient<SoulOfDestruction>(20);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}