using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MagicWeapons
{
    public class SlimeCloud : ModItem
    {
        public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Creates a cloud that slows and poison enemies");
		}

        public override void SetDefaults()
        {
			Item.damage = 9;
			Item.mana = 6;
			Item.DamageType = DamageClass.Magic;
			Item.width = 45;
			Item.height = 45;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = 18764;
			Item.rare = 3;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SlimeCloudProjectileMoving>();
			Item.shootSpeed = 8f;
			Item.noMelee = true;
		}

        public override bool CanUseItem(Player player)
        {
			int type = ModContent.ProjectileType<SlimeCloudProjectile>();
			if (player.ownedProjectileCounts[type] >= 1) Main.projectile.FirstOrDefault(proj => proj.type == type && proj.owner == player.whoAmI)?.Kill();
			if (player.ownedProjectileCounts[Item.shoot] >= 1) Main.projectile.FirstOrDefault(proj => proj.type == Item.shoot && proj.owner == player.whoAmI)?.Kill();
			return base.CanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			velocity = Main.MouseWorld;
        }
    }

	public class SlimeCloudProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
			Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.RainCloudRaining);
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1800;
		}

		ref float timer => ref Projectile.ai[0];
        public override void AI()
        {
			timer++;
			if (timer > 2)
            {
				timer = 0;

				float boris = 5;
				Vector2 pos = new Vector2(Projectile.position.X + boris + Main.rand.NextFloat(Projectile.width - boris * 2), Projectile.Center.Y + 10 + Main.rand.NextFloat(20));

				int proj = Projectile.NewProjectile(
					Projectile.GetSource_FromAI(),
					pos,
					Vector2.UnitY * 7,
					ModContent.ProjectileType<SlimeCloudProjectileRain>(),
					Projectile.damage,
					9,
					Projectile.owner
					);

				Dust.NewDust(pos + Vector2.UnitY * -20, 1, 1, DustID.BlueFairy, SpeedY: 2, SpeedX: Main.rand.Next(-1, 1), Scale: Main.rand.NextFloat(0.4f, 1.5f), newColor: new Color(255, 0, 80), Alpha: 200);

				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].alpha = 120;
			}

			Projectile.BasicAnimation(10);
		}
    }

	public class SlimeCloudProjectileMoving : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.RainCloudMoving);
			Projectile.aiStyle = -1;
			Projectile.minion = true;
		}

		Vector2 goTo;
        public override void OnSpawn(IEntitySource source)
        {
			goTo = Projectile.velocity;
			Projectile.velocity = Vector2.Zero;
        }

        public override void AI()
        {
			Projectile.Center = Vector2.Lerp(Projectile.Center, goTo, 0.15f);
			if (Projectile.Center.DistanceSQ(goTo) < 1f)
            {
				Projectile.Kill();
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SlimeCloudProjectile>(), 20, 0, Projectile.owner);
            }

			Projectile.BasicAnimation(10);
        }
    }

	public class SlimeCloudProjectileRain : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RainNimbus;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RainNimbus);
			Projectile.aiStyle = 1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.velocity.X *= 0.96f;
		}
    }
}
