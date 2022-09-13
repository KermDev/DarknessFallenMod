using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace DarknessFallenMod.Items.RangeWeapons
{
	public class HolyBow : ModItem

	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Bow"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("A bow powered by the souls of light");
		}

		public override void SetDefaults()
		{
			Item.damage = 56;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = 34320;
			Item.rare = 8;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<HolyBowProjectile>();
			Item.shootSpeed = 48f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2f, 0);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofLight, 30);
            recipe.AddIngredient(ItemID.HallowedBar, 30);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	
	
	
	}
}