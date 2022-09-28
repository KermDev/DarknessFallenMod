using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Items.Materials;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class UmbralEdge : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("The sword powered by Darkness ".GetColored(Color.DimGray) + "\n" + "10% chance to one shot a non boss enemy".GetColored(Color.DarkGray));
		}

		public override void SetDefaults()
		{
			Item.damage = 94;
			Item.DamageType = DamageClass.Melee;
			Item.width = 2;
			Item.height = 2;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = 1;
			Item.knockBack = 5;
			Item.value = 4300;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<UmbralEdgeProjectile>();
			Item.shootSpeed = 20;
        }

        public override bool CanUseItem(Player player)
        {
			return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			float speed = Math.Clamp(Main.MouseWorld.DistanceSQ(player.Center) * 0.0004f, 6f, Item.shootSpeed);

			velocity.Normalize();
			velocity *= speed;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int fartType = ModContent.ProjectileType<UmbralEdgeFart>();

			Projectile.NewProjectile(source, position, velocity, fartType, 10, 0, player.whoAmI);
			return true;
        }

        /*
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient <SoulOfDestruction> (5); 
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}*/
    }

	public class UmbralEdgeProjectile : ModProjectile
    {
		Player Player => Main.player[Projectile.owner];

		public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/UmbralEdge";

		public override void SetStaticDefaults()
		{
			/*ProjectileID.Sets.TrailingMode[Type] = 2;
			ProjectileID.Sets.TrailCacheLength[Type] = 20;*/
		}

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;

			Projectile.knockBack = 5;

			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 9999;
			//Projectile.ownerHitCheck = true;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 4;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = Projectile.extraUpdates * Player.itemAnimationMax - 1;
		}

		ref float startAngle => ref Projectile.ai[0];
		const float swingAngle = MathHelper.PiOver2 + MathHelper.PiOver4;
		static int swingDir = 1;
        public override void OnSpawn(IEntitySource source)
        {
			startAngle = Projectile.velocity.ToRotation() - swingAngle * Player.direction * swingDir;
			Projectile.velocity = Vector2.Zero;
		}

        public override void AI()
        {
            if (Player.ItemAnimationEndingOrEnded)
            {
				swingDir = -swingDir;
				Projectile.Kill();
				return;
            }

			Projectile.rotation = startAngle + 2 * swingAngle * swingDir * Player.direction * ((float)(Player.itemAnimationMax - Player.itemAnimation) / Player.itemAnimationMax);

			Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter, true);
			Player.heldProj = Projectile.whoAmI;

			Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
        }

		const int swordLength = 84;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * swordLength);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			hitDirection = Math.Sign(Projectile.Center.DirectionTo(target.Center).X);

			if (!target.boss && target.life < 2000 && target.type != NPCID.TargetDummy && Main.rand.NextBool(10))
			{
				damage = 99999999;
				crit = true;

				DarknessFallenUtils.NewDustCircular(target.Center, DustID.Blood, 1, speedFromCenter: 4, amount: 48);
			}
		}

        public override bool PreDraw(ref Color lightColor)
        {
			Projectile.DrawProjectileInHBCenter(Color.White, origin: new Vector2(-5, 64), rotOffset: MathHelper.PiOver4);

			return false;
        }
    }

	public class UmbralEdgeFart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
			Main.projFrames[Type] = 4;
			ProjectileID.Sets.TrailCacheLength[Type] = 10;
			ProjectileID.Sets.TrailingMode[Type] = 3;
        }

		const int originalSize = 34;
        public override void SetDefaults()
		{
			Projectile.width = originalSize;
			Projectile.height = originalSize;
			Projectile.penetrate = -1;
			Projectile.aiStyle = 0;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 240;
			Projectile.alpha = 10;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 5;

		}

        public override void OnSpawn(IEntitySource source)
        {
			Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			/*
			Player player = Main.player[Projectile.owner];

			if (player.ownedProjectileCounts[Type] > 0)
			{
				Projectile proj = player.GetOldestProjectile(Type);
				if (proj.timeLeft > 50) proj.timeLeft = 50;
			}*/
		}

        const int dissapearTL = 50;
		const float scaleUp = 0.02f;
        public override void AI()
        {
			Projectile.velocity *= 0.93f;

			Projectile.BasicAnimation(8);

			if (Projectile.timeLeft <= dissapearTL)
            {
				Projectile.alpha += (int)(255f / dissapearTL);
            }

			if (Projectile.scale < 2.5f)
            {
				Projectile.scale += scaleUp;
				int newSize = (int)(Projectile.scale * originalSize);
				Projectile.Resize(newSize, newSize);
            }

			if (Main.rand.NextBool(18)) Dust.NewDust(
				Projectile.position, 
				Projectile.width, 
				Projectile.height, 
				DustID.Smoke, 
				newColor: Color.Lerp(Color.Black, Color.Purple, Main.rand.NextFloat()), 
				Scale: Main.rand.NextFloat(0.8f, Projectile.scale + 0.5f), 
				Alpha: Main.rand.Next(0, 40)
				);

			DarknessFallenUtils.ForeachNPCInRange(Projectile.Center, 2 * Projectile.width * Projectile.width, npc =>
            {
				if (npc.CanBeChasedBy() && !npc.boss)
                {
					npc.velocity += npc.Center.DirectionTo(Projectile.Center) * Math.Clamp(1000 / npc.DistanceSQ(Projectile.Center), 0, 0.2f);
                }
            });
        }

        public override bool PreDraw(ref Color lightColor)
        {
			Projectile.DrawAfterImage(prog => Color.Lerp(Color.Purple, Color.Black, 0.5f) * 0.3f * Math.Clamp(Projectile.velocity.LengthSquared(), 0.3f, 0.8f), true, true, true);
			Projectile.DrawProjectileInHBCenter(lightColor, true, centerOrigin: true);
			return false;
        }
    }
}