using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using Microsoft.Xna.Framework;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class UnholyGreatSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unholy Greatsword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("A Bone sword infected with hell's touch" +
				$"\nShatters into bones on hit");
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = 1;
			Item.knockBack = 7;
			Item.value = 13240;
			Item.rare = 3;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<UnholyGreatSwordProjectile>();
			Item.shootSpeed = 20f;
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			for (int i = 0; i < Main.rand.Next(4, 8); i++)
			{
				Vector2 SpawnPoint = target.Center + new Vector2(Main.rand.Next(30, 80) / 10, Main.rand.Next(30, 80)).RotatedByRandom(MathF.PI * 2);
				int Proj = Projectile.NewProjectile(Item.GetSource_OnHit(target), SpawnPoint, Vector2.Normalize(SpawnPoint - target.Center) * 15f, ProjectileID.Bone, 32, 0f, player.whoAmI);
				Main.projectile[Proj].friendly = true;
				Main.projectile[Proj].hostile = false;
				Main.projectile[Proj].active = true;
				Main.projectile[Proj].penetrate = 10;
			}
		}

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Bone, 30);
            recipe.AddIngredient(ItemID.HellstoneBar, 12);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}