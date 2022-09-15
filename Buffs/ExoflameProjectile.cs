using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Buffs
{
    public class ExoflameProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_1"; 

        public override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;

            Projectile.knockBack = 8;

            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.light = 0.6f;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 9999;
            Projectile.ownerHitCheck = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }


        Entity stickTarget;
        Vector2 relativePos;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            stickTarget = target;
            relativePos = target.Center - Projectile.Center;
        }

        Vector2 rotToVector2;
        public override void AI()
        {
            rotToVector2 = Projectile.rotation.ToRotationVector2();

            if (stickTarget is null) return;

            Projectile.Center = relativePos + stickTarget.Center;

        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 lineStart = rotToVector2 * scaledProjWidth * 0.5f + Projectile.Center;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lineStart, lineStart - rotToVector2 * scaledProjWidth);
        }

        Texture2D tex => TextureAssets.Projectile[Type].Value;
        float scaledProjWidth => tex.Width * Projectile.scale;
        public override bool PreDraw(ref Color lightColor)
        {


            return false;
        }
    }
}
