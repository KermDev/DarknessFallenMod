using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Items.Materials;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class SwordOfDestruction : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword of Destruction");
			Tooltip.SetDefault("The sword powered by destruction souls".GetColored(Color.DimGray) + "\n" + "10% chance to one shot a non boss enemy".GetColored(Color.DarkGray));
		}

		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.DamageType = DamageClass.Melee;
			Item.width = 2;
			Item.height = 2;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 1;
			Item.knockBack = 5;
			Item.value = 4300;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<SwordOfDestructionProjectile>();
			Item.shootSpeed = 1;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient <SoulOfDestruction> (5); 
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}

	public class SwordOfDestructionProjectile : ModProjectile
    {
		Player Player => Main.player[Projectile.owner];

		public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/SwordOfDestruction";

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
			Projectile.ownerHitCheck = true;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 4;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = Projectile.extraUpdates * Player.itemAnimationMax - 10;
		}

		ref float startAngle => ref Projectile.ai[0];
		const float swingAngle = 1.3f;
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

			Projectile.rotation = startAngle + 4 * swingAngle * swingDir * Player.direction * ((float)(Player.itemAnimationMax - Player.itemAnimation) / Player.itemAnimationMax);

			Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter, true);
			Player.heldProj = Projectile.whoAmI;

			Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
        }

		const int swordLength = 60;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * swordLength);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss && target.life < 2000 && target.type != NPCID.TargetDummy && Main.rand.NextBool(10))
            {
				target.StrikeNPC(5000000, Projectile.knockBack, (int)Projectile.Center.DirectionTo(target.Center).X, noEffect: true, crit: true);

				DarknessFallenUtils.NewDustCircular(target.Center, DustID.Blood, 1, speedFromCenter: 4, amount: 128);
				DarknessFallenUtils.NewDustCircular(target.Center, DustID.Ghost, 30, speedFromCenter: 4, amount: 64);
			}
        }

        public override bool PreDraw(ref Color lightColor)
        {
			Projectile.DrawProjectileInHBCenter(lightColor, origin: new Vector2(-5, 48), rotOffset: (Player.direction == -1 ? MathHelper.PiOver2 : 0) + (swingDir == -1 ? MathHelper.PiOver2 * Player.direction : 0));

			return false;
        }
    }
}