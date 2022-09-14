using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Pets
{
	public class SkelPetProj : ModProjectile
	{
		Vector2 Velocity;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeleton Candle");

			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.scale = 0.8f;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			
			if (!player.active)
			{
				Projectile.active = false;
				return;
			}
			
			if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.SkelPetBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			
			if (!Main.dedServ)
			{
				Lighting.AddLight(Projectile.Center, 1f, 175f/255f, 0f);
			}

			float DistanceSquare = Vector2.DistanceSquared(player.Center, Projectile.Center);
			Vector2 Direction = player.Center - Projectile.Center;
			if (Direction == Vector2.Zero)
            {
				Direction = Vector2.UnitY;
            }

			if (DistanceSquare >= 10000)
			{
				Velocity = Vector2.Zero;
				Velocity += Vector2.Normalize(Direction) * 5f;
			}
			else
            {
				Velocity *= 0.9f;
            }

			if (DistanceSquare >= 300000)
            {
				Projectile.Center = player.Center;
            }

			Projectile.Center += Velocity;

			Projectile.spriteDirection = (Velocity.X < 0 ? -1 : 1);
		}
	}
}