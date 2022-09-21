using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Pets
{
	public class FruityPetProj : ModProjectile
	{
		Vector2 Velocity;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fruity Light");

			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 43;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.timeLeft *= 5;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.scale = 0.8f;
			Projectile.tileCollide = false;
		}

		float spinRot;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (!player.active)
			{
				Projectile.active = false;
				return;
			}
			
			if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.FruityPetBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			
			if (!Main.dedServ)
			{
				Lighting.AddLight(Projectile.Center, 1f, 175f/255f, 0f);
			}

			Vector2 flyToPos = player.Center + new Vector2(player.direction, 1) * -40;
			Projectile.Center = Vector2.Lerp(Projectile.Center, flyToPos, 0.2f);

			Projectile.spriteDirection = -player.direction;

			if (Main.rand.NextBool(999)) spinRot += MathHelper.TwoPi;

			if (spinRot - Projectile.rotation > 0.02f) Projectile.rotation = MathHelper.Lerp(Projectile.rotation, spinRot, 0.05f);

			Projectile.BasicAnimation(10, 120, 0);
		}

        public override bool PreDraw(ref Color lightColor)
        {
			Projectile.DrawProjectileInHBCenter(Color.White, true, origin: new Vector2(21, 38));
			return false;
        }
    }

	
}