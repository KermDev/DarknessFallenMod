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
using Terraria.GameContent;
using System.Collections;

using static DarknessFallenMod.Systems.CoroutineSystem;

namespace DarknessFallenMod.Items.MeleeWeapons.HellButcher
{
    public class HellButcherProjectile : ModProjectile
    {
        public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/HellButcher/HellButcher";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell Butcher");

            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 4;
        }


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
            Projectile.tileCollide = false;
            Projectile.timeLeft = 9999;
            Projectile.ownerHitCheck = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 999;
        }

        public override bool ShouldUpdatePosition() => false;

        int initialPlayerDir;
        public override void OnSpawn(IEntitySource source)
        {
            initialPlayerDir = Player.direction;
        }

        Player Player => Main.player[Projectile.owner];
        Vector2 rotationDirection;
        Vector2 directionToMouse;
        float scaleMult = 1;
        public override void AI()
        {
            Player.heldProj = Projectile.whoAmI;
            

            if (Player.HeldItem.type != ModContent.ItemType<HellButcher>())
            {
                Projectile.Kill();
                return;
            }

            directionToMouse = Projectile.Center.DirectionTo(Main.MouseWorld);
            rotationDirection = Projectile.rotation.ToRotationVector2();

            Player.direction = directionToMouse.X > 0 ? 1 : -1;
            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter) - new Vector2(Player.direction * 5, 0);

            Behaviour();

            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
        }

        const int MAXDAMAGE = 500;
        const int MAXDAMAGETIMERCOUNT = 120;

        const int MAXSWINGFRAMES = 35;

        int damageMultTimer;
        int swingTimer;
        void Behaviour()
        {
            if (!Player.channel)
            {
                Projectile.rotation += Player.direction * -MathF.Sin(MathF.Pow((float)swingTimer / MAXSWINGFRAMES * 1.35f - 1.35f, 3)) * 0.2f;

                if (swingTimer++ > MAXSWINGFRAMES
                {
                    Projectile.Kill();
                    return;
                }
            }
            else
            {
                Projectile.rotation = directionToMouse.RotatedBy(-(MathHelper.PiOver2 + MathHelper.PiOver4) * Player.direction).ToRotation();
                damageMultTimer++;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (swingTimer > 0)
                return null;
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + rotationDirection * 60, 37, ref _);
        }

        int fxTimer;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;

            float originY = 64;
            float originX = 13;

            Main.EntitySpriteDraw(
                tex, 
                Projectile.Center - Main.screenPosition,
                null, 
                lightColor, 
                Projectile.rotation + MathHelper.PiOver2,
                Player.direction == -1 ? new Vector2(44 - originX, originY) : new Vector2(originX, originY), 
                Projectile.scale * scaleMult,
                Player.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 
                0
                );

            if (swingTimer == 0 && fxTimer++ > 10)
            {
                StartCoroutine(BeginEffect(), CoroutineType.PostDrawTiles);
            }

            return false;
        }

        IEnumerator BeginEffect()
        {
            float maxScale = 2f;
            float scale = maxScale;
            while (true)
            {
                Texture2D tex = TextureAssets.Projectile[Type].Value;

                float originY = 64;
                float originX = 13;

                Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White * ((float)(maxScale - scale) / maxScale),
                Projectile.rotation + MathHelper.PiOver2,
                Player.direction == -1 ? new Vector2(44 - originX, originY) : new Vector2(originX, originY),
                Projectile.scale * scale,
                Player.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0
                );

                scale -= 0.05f;
                if (scale < 1)
                {
                    break;
                }

                yield return null;
            }
            
        }
    }
}