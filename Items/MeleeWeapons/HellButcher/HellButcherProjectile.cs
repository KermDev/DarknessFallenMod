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
using Terraria.Audio;

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

        public override void OnSpawn(IEntitySource source)
        {
            StartCoroutine(DrawChargeEffect(1.2f), CoroutineType.PostDrawTiles);
            StartCoroutine(DrawChargeEffect(1.5f), CoroutineType.PostDrawTiles);
            StartCoroutine(DrawChargeEffect(2f), CoroutineType.PostDrawTiles);
        }

        public override bool ShouldUpdatePosition() => false;

        Player Player => Main.player[Projectile.owner];
        Vector2 rotationDirection;
        Vector2 directionToMouse;
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

        public static readonly int MAX_DAMAGE = 400;
        public static readonly int MAX_DAMAGE_TIMER_COUNT = 180;

        const int MAX_SWING_FRAMES = 28;

        int damageMultTimer;
        int swingTimer;

        const float MAX_SCALE_MULT = 0.8f;
        bool soundPlayed;
        int swingDir;
        void Behaviour()
        {
            if (!Player.channel)
            {
                if (!soundPlayed)
                {
                    SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
                    swingDir = Player.direction;
                    soundPlayed = true;
                }

                Player.direction = swingDir;

                Projectile.rotation += swingDir * -MathF.Sin(MathF.Pow((float)swingTimer / MAX_SWING_FRAMES * 1.35f - 1.35f, 3)) * 0.25f;

                if (swingTimer++ > MAX_SWING_FRAMES)
                {
                    Projectile.Kill();
                    return;
                }
            }
            else
            {
                float progress = (float)Math.Clamp(damageMultTimer, 1, MAX_DAMAGE_TIMER_COUNT) / MAX_DAMAGE_TIMER_COUNT;

                if (Projectile.scale < MAX_SCALE_MULT + 1)
                {
                    Projectile.scale = 1f + MAX_SCALE_MULT * progress;
                }

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

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            float hitStr = (float)Math.Clamp(damageMultTimer, 1, MAX_DAMAGE_TIMER_COUNT) / MAX_DAMAGE_TIMER_COUNT;
            damage = (int)(hitStr * MAX_DAMAGE);
            knockback = hitStr * 5;

            Vector2 hitDir = Player.DirectionTo(target.Center);

            if (damage == MAX_DAMAGE)
                for (int i = 0; i < 2; i++)
                    StartCoroutine(DrawHitEffect(target, hitDir), CoroutineType.PostDrawTiles);

            for (int i = 0; i < (int)(hitStr * 32); i++)
            {
                Vector2 vel = hitDir * 9 * hitStr * Main.rand.NextFloat();
                Dust.NewDust(target.position, target.width, target.height, DustID.Blood, vel.X, vel.Y, Scale: Main.rand.NextFloat(0.4f, 1.7f));
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + rotationDirection * 60 * Projectile.scale, 45 * Projectile.scale, ref _);
        }

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
                Projectile.scale,
                Player.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0
                );

            return false;
        }

        IEnumerator DrawHitEffect(NPC npc, Vector2 hitDir)
        {
            hitDir.Normalize();
            Vector2 lerpOffset = hitDir * 100;

            float npcScale = npc.scale;
            float scale = 1;
            float maxScale = 2;

            Texture2D tex = TextureAssets.Npc[npc.type].Value;

            Vector2 initPos = npc.Center;
            Vector2 pos = initPos;

            Rectangle frame = npc.frame;

            while (true)
            {
                bool began = Main.spriteBatch.HasBegun();

                if (began)
                {
                    Main.spriteBatch.End();
                }

                Main.spriteBatch.BeginDefault();

                Main.EntitySpriteDraw(
                    tex,
                    pos - Main.screenPosition + Main.rand.NextVector2Unit() * 3,
                    frame,
                    Color.Yellow * ((float)(maxScale - scale) / maxScale),
                    npc.rotation,
                    frame.Size() * 0.5f,
                    scale * npcScale,
                    npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    0
                    );

                Main.spriteBatch.End();

                if (began)
                {
                    Main.spriteBatch.BeginDefault();
                }

                pos = Vector2.Lerp(pos, initPos + lerpOffset, 0.15f);

                scale += 0.06f;
                if (scale > maxScale)
                {
                    break;
                }

                yield return null;
            }
        }

        IEnumerator DrawChargeEffect(float timeMult)
        {
            float maxScale = 2f;
            float scale = maxScale;

            while (true)
            {
                Texture2D tex = TextureAssets.Projectile[Type].Value;

                float originY = 64;
                float originX = 13;

                bool began = Main.spriteBatch.HasBegun();

                if (began)
                {
                    Main.spriteBatch.End();
                }

                Main.spriteBatch.BeginDefault();

                Main.EntitySpriteDraw(
                    tex,
                    Projectile.Center - Main.screenPosition + Main.rand.NextVector2Unit() * 3,
                    null,
                    Color.Yellow * ((float)(maxScale - scale) / maxScale),
                    Projectile.rotation + MathHelper.PiOver2,
                    Player.direction == -1 ? new Vector2(44 - originX, originY) : new Vector2(originX, originY),
                    Projectile.scale * scale,
                    Player.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    0
                    );

                Main.spriteBatch.End();

                if (began)
                {
                    Main.spriteBatch.BeginDefault();
                }

                scale -= (maxScale - 1f) / MAX_DAMAGE_TIMER_COUNT * Projectile.extraUpdates * timeMult;
                if (scale < 1 || swingTimer != 0 || Player.HeldItem.type != ModContent.ItemType<HellButcher>())
                {
                    break;
                }

                yield return null;
            }
            
        }
    }
}