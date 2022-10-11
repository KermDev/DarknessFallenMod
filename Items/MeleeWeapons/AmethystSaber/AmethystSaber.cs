using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            Item.damage = 31;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = 1;
            Item.knockBack = 8;
            Item.value = 13764;
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AmethystSaberProjectile>();
            Item.shootSpeed = 8f;
            Item.mana = 2;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.PiOver4 * 0.25f);
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