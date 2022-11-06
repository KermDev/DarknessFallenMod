using DarknessFallenMod.Items.MagicWeapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.HellButcher
{
    public class HellButcher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell Butcher");
            Tooltip.SetDefault("The power of Hell");
        }

        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = -1;
            Item.knockBack = 8;
            Item.value = 17500;
            Item.rare = 8;
            Item.UseSound = SoundID.Item71;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<HellButcherProjectile>();
            Item.shootSpeed = 9f;
            Item.reuseDelay = 0;
            Item.channel = true;
            Item.useTurn = false;
        }
        /*
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			position.Y -= 20;
			velocity = Item.shootSpeed * position.DirectionTo(Main.MouseWorld);
        }
		*/
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