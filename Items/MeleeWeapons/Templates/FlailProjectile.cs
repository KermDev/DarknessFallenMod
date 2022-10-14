using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.Templates
{
    public abstract class FlailProjectile : ModProjectile
    {
		protected Player Owner => Main.player[Projectile.owner];

        public override void SetDefaults()
        {
			Projectile.netImportant = true;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 0.8f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			
			Projectile.aiStyle = ProjAIStyleID.Flail;
			AIType = ProjectileID.Mace;
		}

		public virtual bool PreDrawFlailProj(Color lightColor)
        {
			Projectile.DrawProjectileInHBCenter(lightColor, centerOrigin: true);
			return false;
        }

        public sealed override bool PreDrawExtras()
        {
            return false;
        }

        public sealed override bool PreDraw(ref Color lightColor)
        {
			Vector2 pos = Main.GetPlayerArmPosition(Projectile);

			Vector2 dirToProj = pos.DirectionTo(Projectile.Center);
			float distToProj = pos.Distance(Projectile.Center);

			Texture2D chainTex = ModContent.Request<Texture2D>(Texture + "_Chain").Value;
			Vector2 origin = new Vector2(chainTex.Width * 0.5f, chainTex.Height);
			float rotation = dirToProj.ToRotation() + MathHelper.PiOver2;

			Vector2 drawPos = pos - dirToProj * (chainTex.Width + 2) * 0.5f - Main.screenPosition;

			float iter = distToProj / chainTex.Height;

			for (int i = 0; i < iter; i++)
			{
				Main.EntitySpriteDraw(
					chainTex,
					drawPos + i * dirToProj * (chainTex.Width + 2),
					null,
					lightColor,
					rotation,
					origin,
					1,
					SpriteEffects.None,
					0
					);
			}

			return PreDrawFlailProj(lightColor);
        }
    }
}
