using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Items.Materials;

namespace DarknessFallenMod.Items.MagicWeapons
{
	public class FungiteStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fungite Staff");
			Tooltip.SetDefault("");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 17;
			Item.mana = 5;
			Item.DamageType = DamageClass.Magic;
			Item.width = 30;
			Item.height = 46;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = 5;
			Item.knockBack = 3;
			Item.value = 18764;
			Item.rare = 3;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<FungiteStaffProjectile>();
			Item.shootSpeed = 13f;
			Item.noMelee = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			DarknessFallenUtils.OffsetShootPos(ref position, velocity, Vector2.UnitX * 70);
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient<FungiteBar>(15);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}