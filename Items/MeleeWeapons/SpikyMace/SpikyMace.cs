using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace DarknessFallenMod.Items.MeleeWeapons.SpikyMace
{
	public class SpikyMace : ModItem
	{
		bool canShoot => Main.player[Item.whoAmI].ownedProjectileCounts[ModContent.ProjectileType<SpikyMaceProj>()] < 1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiky Mace"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Hold to swing around and release to throw" +
				"\nInflicts bleeding on hit");
		}

		public override void SetDefaults()
		{
			Item.damage = 75;
			Item.DamageType = DamageClass.Melee;
			Item.width = 43;
			Item.height = 41;
			Item.useTime = 0;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 7;
			Item.value = 13240;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			Item.noUseGraphic = true;
			Item.noMelee = true;

			Item.shoot = ModContent.ProjectileType<SpikyMaceProj>();
			Item.shootSpeed = 0f;
        }

        public override bool CanUseItem(Player player)
        {
			return canShoot;
        }
    }

    public class SpikyMaceProj : ModProjectile
    {
		bool holding = true;
		int direction = 1;
		public Player owner => Main.player[Projectile.owner];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiky Mace");
			Main.projFrames[Projectile.type] = 1;

			ProjectileID.Sets.TrailCacheLength[Type] = 8;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}


		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 42;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = true;
			Projectile.timeLeft = -1;

			Projectile.localNPCHitCooldown = 20;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Bleeding, 150);
		}

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			target.AddBuff(BuffID.Bleeding, 129); //129 is not a random number i swear. Coming back to this, i am wondering why 129;
        }

        public override void OnSpawn(IEntitySource source)
        {
			direction = owner.direction;
			Projectile.ai[0] = -1.8f;
        }

        public override void AI()
        {
			Projectile.timeLeft = 3;
			if (!PlayerInput.Triggers.Old.MouseLeft && holding)
            {
				holding = false;
				Throw();
            }

			Projectile.ai[0] += 0.10471975512f * (holding ? 2 : 3) * direction; //in radians. one full rotation is 2 pi radians. if it spins at 1 rps, it needs to increase by approx. 6.28 every 60 ticks or 0.10471975512. 2 rps = ~0.20943951024 if my math is good (should have used a claculator);
																	//Projectile.ai[0] += 0.020943951024f; 
			if (holding)
			{
				Projectile.Center = (owner.Center - new Vector2(7, -1)) + (Vector2.One * 12).RotatedBy(Projectile.ai[0], center: Vector2.Zero);
				Projectile.rotation = -0.5f;
				Projectile.Center += new Vector2(4, 0).RotatedBy(Projectile.ai[0]);
				Projectile.rotation = Vector2.Normalize(Projectile.Center - (owner.Center - new Vector2(7, -1))).ToRotation();
				owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.Floor((Projectile.ai[0] - 1f) * 1.5f) / 1.5f);
			}
			else
            {
				Projectile.velocity += new Vector2(0, 1f);
				Projectile.rotation = Projectile.ai[0];
			}
		}

		public void Throw()
        {
			Projectile.velocity += Vector2.Normalize(Projectile.Center - owner.Center) * 25;
        }

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i <= 30; i++)
			{
				Dust.NewDust(Projectile.Center, 30, 30, DustID.RedMoss);
			}
		}
    }
}