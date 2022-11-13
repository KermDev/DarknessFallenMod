using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;

namespace DarknessFallenMod.Items.RangeWeapons.DarkDagger
{
    public class DarkDaggerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Dagger");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.aiStyle = 2;
            Projectile.hostile = false;
            Projectile.timeLeft = 250;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        Vector2 impactPos;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (impactPos == Vector2.Zero)
                impactPos = target.Center - (target.Center - Projectile.Center) * 0.75f;

            SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
            if (Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 10; i++)
                {
                    //release projectile in random direction
                    Main.projectile[Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, new Vector2(4, 0).RotatedByRandom(2 * Math.PI), ModContent.ProjectileType<DaggerShard>(), Projectile.damage, Projectile.knockBack, Projectile.owner)].netUpdate = true;
                }
            }
        }

        public override void PostDraw(Color lightColor)
        {
            if (impactPos != Vector2.Zero)
            {
                DarknessFallenUtils.Reload(Main.spriteBatch, BlendState.Additive);
                float progress = Utils.GetLerpValue(0f, 50, Projectile.timeLeft - 200);
                Texture2D circle = ModContent.Request<Texture2D>("DarknessFallenMod/Textures/circle_01").Value;
                Texture2D glow = ModContent.Request<Texture2D>("DarknessFallenMod/Textures/circle_01").Value;
                Main.spriteBatch.Draw(circle, impactPos - Main.screenPosition, null, new Color(128, 128, 128) * Math.Clamp(progress * 3f, 0, 50), 0, circle.Size() / 2, 0.5f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                Main.spriteBatch.Draw(glow, impactPos - Main.screenPosition, null, new Color(192, 192, 192) * Math.Clamp(progress * 3f, 0, 50), Main.GameUpdateCount * 0.0025f, glow.Size() / 2, 0.5f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                DarknessFallenUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, Projectile.height / 2);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.Pink) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            DarknessFallenUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
    }
}
