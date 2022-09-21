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

			Vector2 flyToPos = player.Center + new Vector2(player.direction, 1) * -34;
			Projectile.Center = Vector2.Lerp(Projectile.Center, flyToPos, 0.2f);

			Projectile.spriteDirection = player.direction;

			Projectile.BasicAnimation(10);
		}
	}
}