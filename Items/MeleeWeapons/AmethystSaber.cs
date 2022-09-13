using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class AmethystSaber : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Saber"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("The power of gemstones forged together");
		}

		public override void SetDefaults()
		{
			Item.damage = 31;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 19;
			Item.useAnimation = 19;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = 13764;
			Item.rare = 8;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AmethystSaberProjectile>();
            Item.shootSpeed = 6f;
			Item.mana = 2;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MeteoriteBar, 15);
            recipe.AddIngredient(ItemID.Sapphire, 15);
            recipe.AddIngredient(ItemID.Amethyst, 25);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}