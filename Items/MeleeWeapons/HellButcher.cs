using DarknessFallenMod.Items.MagicWeapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class HellButcher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hell Butcher"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("The power of Hell");
		}

		public override void SetDefaults()
		{
			Item.damage = 41;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 17764;
			Item.rare = 8;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HellButcherProjectile>();
            Item.shootSpeed = 20f;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MeteoriteBar, 35);
            recipe.AddIngredient(ItemID.HellstoneBar, 35);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}