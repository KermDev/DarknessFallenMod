using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.HolyGreatsword
{
	public class HolyGreatsword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Greatsword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("A great sword powered by the souls of light");
		}

		public override void SetDefaults()
		{
			Item.damage = 140;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = -1;
			Item.knockBack = 8;
			Item.value = 17500;
			Item.rare = 8;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<HolyGreatswordProjectile>();
			Item.shootSpeed = 9f;
			Item.reuseDelay = 0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 35);
            recipe.AddIngredient(ItemID.SoulofLight, 35);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}