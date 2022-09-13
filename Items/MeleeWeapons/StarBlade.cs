using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class StarBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Blade"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("A sword which drains its power from the stars");
		}

		public override void SetDefaults()
		{
			Item.damage = 27;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 1;
			Item.knockBack = 7;
			Item.value = 13340;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.FallingStar;
			Item.shootSpeed = 10f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 SpawnPos = player.Center + new Vector2(Main.rand.Next(-300, 300), -500);
			int Proj = Projectile.NewProjectile(source, SpawnPos, Vector2.Normalize(Main.MouseWorld - SpawnPos) * 10f, type, damage, knockback, player.whoAmI);

			return false;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FallenStar, 30);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}