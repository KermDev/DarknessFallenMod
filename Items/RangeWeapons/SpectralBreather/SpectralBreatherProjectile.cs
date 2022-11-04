using DarknessFallenMod.Utils;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace DarknessFallenMod.Items.RangeWeapons.SpectralBreather
{
    public class SpectralBreatherProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.BallofFire;

        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 55;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.scale = 0;

            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.usesIDStaticNPCImmunity = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity = Projectile.velocity.RotatedByRandom(0.15f);
        }

        public override void AI()
        {
            Projectile.velocity *= 0.98f;

            if (Projectile.timeLeft < 20)
            {
                Projectile.scale *= 0.9f;
            }
            else if (Projectile.scale < 1)
            {
                Projectile.scale += 0.14f;
            }
        }

        public override void Kill(int timeLeft)
        {
            //Dust.NewDust(Projectile.Center, 0, 0, DustID.Firefly, Projectile.velocity.X, Projectile.velocity.Y);
        }

        public override void PostDraw(Color lightColor)
        {
            var fx = Filters.Scene["FireShader"].GetShader().Shader;

            fx.Parameters["time"].SetValue(Main.GameUpdateCount * 0.001f);

            Main.spriteBatch.End();
            Main.spriteBatch.BeginAdditive();
            fx.CurrentTechnique.Passes[0].Apply();

            Texture2D texture = ModContent.Request<Texture2D>("DarknessFallenMod/Assets/Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                Color.Lerp(Color.Yellow, Color.Red, Main.rand.NextFloat()) * 0.8f,
                0,
                texture.Size() * 0.5f,
                0.6f * Main.rand.NextFloat() * Projectile.scale,
                SpriteEffects.None,
                0
                );

            for (int i = 0; i < Main.rand.Next(3, 7); i++)
            {
                Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition + Main.rand.NextVector2Unit() * 12 * Projectile.scale,
                null,
                Color.Lerp(Color.Red, Color.OrangeRed, Main.rand.NextFloat()) * 0.9f,
                0,
                texture.Size() * 0.5f,
                0.3f * Main.rand.NextFloat() * Projectile.scale,
                SpriteEffects.None,
                0
                );
            }

            Main.spriteBatch.End();
            Main.spriteBatch.BeginDefault();
        }
    }
}
