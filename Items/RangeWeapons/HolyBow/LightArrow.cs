using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace DarknessFallenMod.Items.RangeWeapons.HolyBow
{
    public class LightArrow : ModProjectile, IPrimitiveDrawer
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.light = 0.4f;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.ai[0] == 0)
                trail.PushBack(Projectile.Center + Projectile.velocity);
            else
            {
                if (trail.Count > 1)
                {
                    trail.PopFront();
                    Projectile.scale = trail.Count / 30f;
                }
                else
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("Terraria/Images/Projectile_" + ProjectileID.RainbowCrystalExplosion).Value;
            Main.spriteBatch.Reload(BlendState.Additive);
            for (int i = 0; i < 5; i++)
            {
                Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.Yellow * (1f - i / 5f), Projectile.rotation, tex.Size() / 2, Projectile.scale * (0.1f + 0.15f * i), SpriteEffects.None, 0);
                Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.Yellow * (1f - i / 5f), MathHelper.PiOver2 + Projectile.rotation, tex.Size() / 2, Projectile.scale * (0.1f + 0.15f * i), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] != 0) return;
            Projectile.ai[0] = 1;
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
            Projectile.tileCollide = false;
        }

        public override bool PreKill(int timeLeft)
        {
            if (Projectile.ai[0] != 0) return true;
            Projectile.ai[0] = 1;
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
            Projectile.tileCollide = false;
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.ai[0] == 0;
        }

        private CircularBuffer<Vector2> trail = new(30);

        public float Wrap(float x)
        {
            return (x % 1 + 1) % 1;
        }
        public void DrawPrimitives()
        {
            if (trail.Count < 2)
                return;
            PrimitiveList list = new((trail.Count - 1) * 6, PrimitiveType.TriangleList);
            Texture2D tex = ModContent.Request<Texture2D>("DarknessFallenMod/Assets/Trail2").Value;
            list.SetTexture(tex);
            for (int i = 0; i < trail.Count - 1; i++)
            {
                Vector2 pos1 = trail[i];
                Vector2 pos2 = trail[i + 1];
                Vector2 dir1 = DarknessFallenUtils.GetRotation(trail, i);
                Vector2 dir2 = DarknessFallenUtils.GetRotation(trail, i + 1);
                float prog1 = (float)i / trail.Count;
                float prog2 = (float)(i + 1) / trail.Count;
                Color color1 = Color.Lerp(Color.Yellow, Color.LightYellow, prog1) * prog1;
                Color color2 = Color.Lerp(Color.Yellow, Color.LightYellow, prog2) * prog2;
                float width1 = MathHelper.Lerp(20f, 1f, prog1);
                float width2 = MathHelper.Lerp(20f, 1f, prog2);
                Vector2 v1 = pos1 + dir1 * width1; // top left
                Vector2 v2 = pos1 - dir1 * width1; // bottom left
                Vector2 v3 = pos2 + dir2 * width2; // top right
                Vector2 v4 = pos2 - dir2 * width2; // bottom right
                prog1 = Wrap(prog1 + Main.GlobalTimeWrappedHourly);
                prog2 = Wrap(prog2 + Main.GlobalTimeWrappedHourly);
                Vector2 c1 = new Vector2(prog1, 1); // top left
                Vector2 c2 = new Vector2(prog1, 0); // bottom left
                Vector2 c3 = new Vector2(prog2, 1); // top right
                Vector2 c4 = new Vector2(prog2, 0); // bottom right
                
                list.AddVertex(v1, color1, c1);
                list.AddVertex(v2, color1, c2);
                list.AddVertex(v3, color2, c3);
                
                list.AddVertex(v2, color1, c2);
                list.AddVertex(v4, color2, c4);
                list.AddVertex(v3, color2, c3);
            }
            list.Draw();
        }
    }
}