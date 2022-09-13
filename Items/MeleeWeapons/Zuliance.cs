using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class Zuliance : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zuliance"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("The glowing sword, forged with care and powered by souls");
		}

		public override void SetDefaults()
		{
			Item.damage = 286;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = 64300;
			Item.rare = 8;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.IceSickle;
			Item.shootSpeed = 20f;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 50);
            recipe.AddIngredient(ItemID.SoulofLight, 25);
            recipe.AddIngredient(ItemID.Sapphire, 5);
            recipe.AddIngredient(ItemID.IceBlock, 50);
            recipe.AddIngredient(ItemID.GoldBar, 10);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}