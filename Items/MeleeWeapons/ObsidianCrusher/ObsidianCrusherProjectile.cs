using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static DarknessFallenMod.Systems.CoroutineSystem;

namespace DarknessFallenMod.Items.MeleeWeapons.ObsidianCrusher
{
    public class ObsidianCrusherProjectile : ModProjectile
    {
        Player Player => Main.player[Projectile.owner];

        public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/ObsidianCrusher/ObsidianCrusher";

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;

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
            Projectile.MaxUpdates = 3;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * Player.itemAnimationMax - 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - (MathHelper.Pi * Player.direction);
        }

        Vector2 rotatedDirection;
        float rotVel;

        bool shotBoulders;
        public override void AI()
        {
            if (Player.ItemAnimationEndingOrEnded)
            {
                Projectile.Kill();
                return;
            }

            Player.heldProj = Projectile.whoAmI;
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter);

            if (Player.itemAnimation > 10)
            {
                rotVel = (MathF.Cos(MathHelper.PiOver2 * ((float)Player.itemAnimation / Player.itemAnimationMax) + MathHelper.PiOver2) + 1) * 0.23f;
            }
            else
            {
                rotVel *= 0.75f;

                Vector2 hitPoint = Player.MountedCenter + rotatedDirection * bladeLenght + new Vector2(-22, 10 * Player.direction).RotatedBy(Projectile.rotation);

                hitPoint.ToPoint().DrawPoint(10);

                bool hitTile = Collision.SolidTiles(hitPoint, 1, 1);
                if (hitTile && !shotBoulders)
                {
                    shotBoulders = true;
                    StartCoroutine(ShootBoulders(hitPoint, Player.direction, Projectile.GetSource_FromAI()));
                }
            }

            Projectile.rotation += rotVel * Player.direction;

            rotatedDirection = Projectile.rotation.ToRotationVector2();

            Projectile.direction = Player.direction;
            Projectile.spriteDirection = Projectile.direction;
        }

        float bladeLenght => TextureAssets.Projectile[Type].Value.Width * 1.7f;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Player.MountedCenter, Player.MountedCenter + rotatedDirection * bladeLenght);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 texSize = tex.Size();

            Main.EntitySpriteDraw(
                tex,
                Player.MountedCenter - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + rotatedDirection * bladeLenght * 0.5f,
                null,
                lightColor,
                Projectile.rotation + (Player.direction == -1 ? -MathHelper.Pi - MathHelper.PiOver4 : MathHelper.PiOver4),
                texSize * 0.5f,
                1,
                Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0
                );

            return false;
        }

        IEnumerator ShootBoulders(Vector2 startPos, int direction, IEntitySource source)
        {
            for (int i = 0; i < 4; i++)
            {
                Projectile.NewProjectile(source, startPos + Vector2.UnitX * direction * i * 30, Vector2.UnitY * -10, ProjectileID.Boulder, 20, 1);
                yield return WaitFor.Frames(15);
            }
        }
    }
}
