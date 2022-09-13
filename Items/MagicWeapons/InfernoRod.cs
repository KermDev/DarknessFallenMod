using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MagicWeapons
{
	public class InfernoRod : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Inferno Rod"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("shoots 3 fire balls that track enemies");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 19;
			Item.mana = 8;
			Item.DamageType = DamageClass.Magic;
			Item.width = 30;
			Item.height = 46;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = 5;
			Item.knockBack = 3;
			Item.value = 18764;
			Item.rare = 3;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<InfernoRodProjectile>();
			Item.shootSpeed = 8f;
			Item.noMelee = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity.RotatedBy(0.58f), type, damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity.RotatedBy(-0.58f), type, damage, knockback, player.whoAmI);
			return true;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 45);
            recipe.AddIngredient(ItemID.MeteoriteBar, 45);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}