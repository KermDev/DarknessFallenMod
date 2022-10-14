using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using System.Linq;
using Terraria.DataStructures;
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.Items.MeleeWeapons.HellButcher
{
    public class HellButcherProjectile : ModProjectile
    {
        public object dust { get; private set; }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell Butcher");
            Main.projFrames[Projectile.type] = 3;

            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 4;
        }


        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;

            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.light = 0.4f;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.InfernoFork);

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.BasicAnimation(10);
        }

        public override void Kill(int timeLeft) //this is caled whenever the projectile expires (only once);
        {
            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.InfernoFork, 10, speedFromCenter: 3, amount: 30);
            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.RedTorch, 10, speedFromCenter: 6, amount: 10).ForEach(dust => dust.noGravity = true);
        }

        VertexStrip vertexStripMM = new VertexStrip();
        public override bool PreDraw(ref Color lightColor)
        {
            /*
            Main.spriteBatch.End();
            Main.spriteBatch.BeginWithShaderOptions();

            GameShaders.Misc["MagicMissle"]
                .UseSaturation(-2f)
                .UseOpacity(50)
                .Apply(new DrawData?());

            //Enumerable.Repeat<float>(0, Projectile.oldRot.Count()).ToArray()
            
            vertexStripMM.PrepareStrip(Projectile.oldPos,
                Enumerable.Repeat<float>(0, Projectile.oldRot.Count()).ToArray(),
                prog => Color.Lerp(Color.Red, Color.Yellow * 0.5f, prog),
                prog => MathHelper.Lerp(30, 1, prog),
                Projectile.Hitbox.Size() * 0.5f - Main.screenPosition,
                null,
                true
                );

            vertexStripMM.PrepareStripWithProceduralPadding(Projectile.oldPos,
                Projectile.oldRot,
                prog => Color.Lerp(Color.Red, Color.Yellow * 0.5f, prog),
                prog => MathHelper.Lerp(30, 1, prog),
                Projectile.Hitbox.Size() * 0.5f - Main.screenPosition,
                true,
                true
                );

            vertexStripMM.DrawTrail();
            
           

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.End();
            Main.spriteBatch.BeginWithDefaultOptions();
            */

            Projectile.DrawAfterImage(prog => Color.Lerp(Color.Yellow, Color.OrangeRed, prog) * 0.5f, animated: true, origin: new Vector2(49, 14));
            Projectile.DrawProjectileInHBCenter(lightColor, true, origin: new Vector2(49, 14));

            return false;
        }
    }
}