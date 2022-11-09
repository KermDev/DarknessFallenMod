using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace DarknessFallenMod.Items.MeleeWeapons.AmethystSaber
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
			Item.damage = 18;
			Item.crit = 8;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 17;
			Item.useAnimation = 17;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.rare = 8;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.shootSpeed = 8f;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (crit)
            {
                SoundEngine.PlaySound(SoundID.Item101, player.Center);
                for (int i = 0; i < Main.rand.Next(4, 7); i++)
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-9, -5)), ModContent.ProjectileType<AmethystSpike>(), 5, knockBack / 2, player.whoAmI);
            }
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