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
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 55;
            Projectile.penetrate = -1;

            //Projectile.localNPCHitCooldown = 60;
            //Projectile.usesLocalNPCImmunity = true;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
        }

        bool purple;
        float sizeMult;
        public override void OnSpawn(IEntitySource source)
        {
            float randRot = 0.1f * Main.rand.NextFloatDirection();
            sizeMult = Main.rand.NextFloat(0.7f, 1.2f);

            purple = Main.rand.NextBool();

            Projectile.velocity = Projectile.velocity.RotatedBy(randRot);
            Projectile.scale = 0f;
        }

        float alpha = 1f;
        public override void AI()
        {
            Projectile.velocity *= 0.95f;

            if (Projectile.timeLeft < 30)
            {
                alpha -= 0.03f;
                Projectile.velocity.Y -= 0.25f;
            }
            else if (Projectile.scale < 1)
            {
                Projectile.scale += 0.14f;
            }

            if (!Main.dedServ)
            {
                Lighting.AddLight(
                    Projectile.Center,
                    purple ? 0.2f : 0.4f,
                    purple ? 0 : 0.2f,
                    purple ? 0.6f : 0
                    );
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (purple)
            {
                target.AddBuff(BuffID.ShadowFlame, 300);
            }
            else
            {
                target.AddBuff(BuffID.OnFire, 300);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity * 0.4f;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            //Dust.NewDust(Projectile.Center, 0, 0, DustID.Firefly, Projectile.velocity.X, Projectile.velocity.Y);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.BeginAdditive();

            Texture2D texture = ModContent.Request<Texture2D>("DarknessFallenMod/Assets/Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            Color lerp1 = purple ? Color.Purple : Color.DarkOrange;
            Color lerp2 = purple ? Color.BlueViolet : Color.Red;

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition + Main.rand.NextVector2Unit() * 5 * Projectile.scale * sizeMult,
                null,
                Color.Lerp(lerp1, lerp2, Main.rand.NextFloat()) * 0.95f * alpha,
                Main.rand.NextFloatDirection(),
                texture.Size() * 0.5f,
                0.5f * (Main.rand.NextFloat(0.7f, 1.5f)) * Projectile.scale * sizeMult,
                SpriteEffects.None,
                0
                );

            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition + Main.rand.NextVector2Unit() * 12 * Projectile.scale * sizeMult,
                null,
                Color.Lerp(lerp1, lerp2, Main.rand.NextFloat()) * 0.8f * alpha,
                Main.rand.NextFloatDirection(),
                texture.Size() * 0.5f,
                0.3f * Main.rand.NextFloat() * Projectile.scale * sizeMult,
                SpriteEffects.None,
                0
                );
            }

            Main.spriteBatch.End();
            Main.spriteBatch.BeginDefault();

            return false;
        }
    }
}
