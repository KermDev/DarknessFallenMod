using DarknessFallenMod.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class SwordOfNature : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword Of Light"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("A sword made by souls of nature");
		}

		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 1220;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			Item.GetGlobalItem<DarknessFallenItem>().WorldGlowMask = ModContent.Request<Texture2D>(Texture + "Glowmask").Value;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient<SoulOfNature>(5);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}