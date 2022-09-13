using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.RangeWeapons
{
	public class NatureBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Bow"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("A bow made by nature's materials");
		}

		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = 1025;
			Item.rare = 0;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = 1;
            Item.shoot = ProjectileID.Leaf;
            Item.shootSpeed = 6f;
		}

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, 0);
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Acorn, 15);
            recipe.AddIngredient(ItemID.Wood, 45);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	
	
	
	}
}