using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.UmbralEdge
{
    public class UmbralEdgeProjectile : ModProjectile
    {
        Player Player => Main.player[Projectile.owner];

        public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/UmbralEdge/UmbralEdge";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 4;
			ProjectileID.Sets.TrailCacheLength[Type] = 100;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;

            Projectile.knockBack = 5;

            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 9999;
            //Projectile.ownerHitCheck = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 16;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.extraUpdates * Player.itemAnimationMax - 1;
        }

        ref float startAngle => ref Projectile.ai[0];
        const float swingAngle = MathHelper.PiOver2 + MathHelper.PiOver4;
        static int swingDir = 1;
        public override void OnSpawn(IEntitySource source)
        {
            startAngle = Projectile.velocity.ToRotation() - swingAngle * Player.direction * swingDir;
            Projectile.velocity = Vector2.Zero;
        }

        public override void AI()
        {
            if (Player.ItemAnimationEndingOrEnded)
            {
                swingDir = -swingDir;
                Projectile.Kill();
                return;
            }

            Projectile.rotation = startAngle + 2 * swingAngle * swingDir * Player.direction * ((float)(Player.itemAnimationMax - Player.itemAnimation) / Player.itemAnimationMax);

            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter, true);
            Player.heldProj = Projectile.whoAmI;

            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
        }

        const int swordLength = 84;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * swordLength);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Math.Sign(Projectile.Center.DirectionTo(target.Center).X);

            if (!target.boss && target.life < 2000 && Main.rand.NextBool(10))
            {
                damage = target.life;
                crit = true;

                DarknessFallenUtils.NewDustCircular(target.Center, DustID.Blood, 1, speedFromCenter: 4, amount: 48);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawAfterImage(
                prog => Color.Lerp(Color.Black, 
                Color.White, prog) * 0.04f, 
                origin: new Vector2(-5, 64), 
                oldPos: false, 
                altTex: ModContent.Request<Texture2D>(Texture + "_Purple").Value,
                rotOffset: i => Projectile.oldRot[i] == 0 ? Projectile.rotation : 0
                
                );
            Projectile.DrawProjectileInHBCenter(Color.White, origin: new Vector2(-5, 64), rotOffset: MathHelper.PiOver4);

            return false;
        }
    }
}
