using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.ObsidianCrusher
{
    public class ObsidianCrusherBoulder : ModProjectile
    {
        public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/ObsidianCrusher/Rock_1";

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.light = 2.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 600;
        }

        Vector2 goToPos;
        float goToScale = 1;

        byte rockNum = 1;
        public override void OnSpawn(IEntitySource source)
        {
            if (source.Context is not null)
            {
                if (byte.TryParse(source.Context, out byte n))
                {
                    rockNum = n;
                }
            }

            int width, height;

            switch (rockNum)
            {
                case 2:
                    width = 34;
                    height = 24;
                    Projectile.damage = 38;
                    break;
                case 3:
                    width = 42;
                    height = 38;
                    Projectile.damage = 48;
                    break;
                case 4:
                    width = 52;
                    height = 56;
                    Projectile.damage = 58;
                    break;
                default:
                    width = 26;
                    height = 16;
                    Projectile.damage = 68;
                    break;
            }

            Projectile.Resize(width, height);

            Projectile.position.Y -= Projectile.height * 0.5f;

            int goToHeight = 40;
            goToPos = Projectile.position;
            Projectile.position.Y += goToHeight;

            Projectile.velocity = Vector2.Zero;
            Projectile.scale = 0;

            int dustAmount = 40;
            float diff = 2f;
            for (int i = 0; i < dustAmount; i++)
            {
                Dust.NewDustDirect(
                    goToPos + new Vector2(Projectile.width * i / dustAmount, Projectile.height),
                    0,
                    0,
                    DustID.Stone,
                    Main.rand.NextFloat(-1, 1) * diff,
                    Main.rand.NextFloat(-1, 1) * diff * 3.5f,
                    Scale: Main.rand.NextFloat(0.5f, 1.7f)
                    );
            }
        }


        public override void AI()
        {
            /*
            if (Projectile.timeLeft == 60)
            {
                goToPos = Projectile.position + Vector2.UnitY * 40;
                goToScale = 0;
            }*/

            Projectile.position = Vector2.Lerp(Projectile.position, goToPos, 0.25f);
            Projectile.scale = MathHelper.Lerp(Projectile.scale, goToScale, 0.25f);

            if (Projectile.scale > 0.9999f) Projectile.friendly = false;
            
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.scale < 0.99f) 
            {
                target.velocity.Y -= 18 * target.knockBackResist; 
            }
        }

        public override void Kill(int timeLeft)
        {
            int dustAmount = 50;
            float diff = 2f;
            for (int i = 0; i < dustAmount; i++)
            {
                Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Stone,
                    Main.rand.NextFloat(-1, 1) * diff,
                    Main.rand.NextFloat(-1, 1) * diff * 3.5f,
                    Scale: Main.rand.NextFloat(0.5f, 1.7f)
                    ).noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture[..^1] + rockNum).Value;

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                0,
                tex.Size() * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0
                );

            return false;
        }
    }
}
