using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using DarknessFallenMod.Core;

namespace DarknessFallenMod.Items.MeleeWeapons.MysticSoulSword
{
	public class MysticSouls : ModItem, IGlowmask
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
        }

        int charger;
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            charger++;
            if (charger >= 4)
            {
                SoundEngine.PlaySound(SoundID.Item14, target.position);
                Terraria.Projectile.NewProjectile(Item.GetSource_OnHit(target), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<SoulProj>(), damage, knockBack, player.whoAmI);
                charger = 0;
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
			if (Main.rand.NextBool(5)) Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.YellowStarDust);
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.SoulofFlight, 5);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
			recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}