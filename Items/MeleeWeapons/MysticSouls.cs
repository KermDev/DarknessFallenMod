using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class MysticSouls : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mystic Souls Sword");
			Tooltip.SetDefault("A sword fused with the souls of the fallen");
		}

		public override void SetDefaults()
		{
			Item.damage = 112;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.knockBack = 7;
			Item.value = 29440;
			Item.rare = 5;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.StarWrath;
			Item.shootSpeed = 6f;
			Item.GetGlobalItem<DarknessFallenItem>().WorldGlowMask = ModContent.Request<Texture2D>(Texture).Value;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
			if (Main.rand.NextBool(5)) Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.YellowStarDust);
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofNight, 25);
            recipe.AddIngredient(ItemID.SoulofLight, 25);
            recipe.AddIngredient(ItemID.SoulofMight, 20);
            recipe.AddIngredient(ItemID.SoulofFlight, 20);
            recipe.AddIngredient(ItemID.SoulofFright, 20);
            recipe.AddIngredient(ItemID.SoulofSight, 20);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}