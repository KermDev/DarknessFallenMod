using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.MagmitePitchfork
{
    public class MagmitePitchforkProjectile : ModProjectile
    {
        Player Player => Main.player[Projectile.owner];

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
            Projectile.netImportant = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 999;
        }

        float rotOffset = MathHelper.PiOver4 + MathHelper.PiOver2;
        bool altAttack;
        public override void OnSpawn(IEntitySource source)
        {
            altAttack = Player.altFunctionUse == 2;
        }

        public override bool ShouldUpdatePosition() => false;

        float spearLength => 155f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 startCollision = Projectile.Center + rotationDirection * (originMult - 1f) * spearLength;
            float _ = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), startCollision, startCollision + rotationDirection * spearLength, 15, ref _);
        }

        Vector2 rotationDirection;
        public override void AI()
        {
            Player.heldProj = Projectile.whoAmI;

            if ((Player.ItemAnimationEndingOrEnded && !altAttack) || Player.HeldItem.type != ModContent.ItemType<MagmitePitchfork>())
            {
                Projectile.Kill();
                return;
            }

            rotationDirection = Projectile.rotation.ToRotationVector2();
            Projectile.Center = Player.Center;

            Behaviour();

            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
        }

        NPC stabbedNPC;
        float distanceToNPC;
        void Behaviour()
        {
            if (altAttack)
            {
                Projectile.rotation = Player.Center.DirectionTo(Main.MouseWorld).ToRotation();

                if (stabbedNPC is not null)
                {
                    if (stabbedNPC.life <= 0 || !stabbedNPC.active)
                    {
                        Projectile.Kill();
                        return;
                    }

                    stabbedNPC.Center = Projectile.Center + rotationDirection * distanceToNPC;
                    stabbedNPC.velocity = Vector2.Zero;

                    if (Main.myPlayer == Player.whoAmI && PlayerInput.Triggers.JustPressed.MouseLeft)
                    {
                        Projectile fork = Projectile.NewProjectileDirect(
                            Projectile.GetSource_FromAI(), 
                            Projectile.Center + rotationDirection * spearLength * originMult,
                            rotationDirection * 16,
                            ModContent.ProjectileType<MagmitePitchforkThrownProjectile>(),
                            300,
                            0,
                            Player.whoAmI
                            );

                        (fork.ModProjectile as MagmitePitchforkThrownProjectile).StabbedNPC = stabbedNPC;
                        (fork.ModProjectile as MagmitePitchforkThrownProjectile).OffsetFromCenter = stabbedNPC.Center - fork.Center;

                        Projectile.Kill();
                    }
                }
                else
                {
                    originMult = 0.5f + MathF.Sin((1f - (float)Player.itemAnimation / Player.itemAnimationMax) * MathHelper.Pi) * 0.4f;
                    if (Player.ItemAnimationEndingOrEnded)
                    {
                        Projectile.Kill();
                    }
                }
            }
            else
            {
                /* For Halbert
                if (Main.myPlayer == Player.whoAmI)
                {
                    float x = (1f - (float)Player.itemAnimation / Player.itemAnimationMax) * MathHelper.TwoPi;
                    float y;
                    if (x > MathHelper.PiOver2 && x < 3 * MathHelper.PiOver2)
                    {
                        y = MathF.Sin(x);

                    }
                    else
                    {
                        y = MathF.Pow(MathF.Sin(x), 3);
                    }

                    Projectile.rotation = Player.Center.DirectionTo(Main.MouseWorld).ToRotation() + y * 0.9f * -Player.direction;
                }
                */

                Projectile.rotation = Player.Center.DirectionTo(Main.MouseWorld).ToRotation() + MathF.Sin((1f - (float)Player.itemAnimation / Player.itemAnimationMax) * MathHelper.TwoPi) * 0.1f * Player.direction;
                originMult = 0.35f + MathF.Sin((1f - (float)Player.itemAnimation / Player.itemAnimationMax) * MathHelper.Pi) * 0.6f;
            }
        }


        public override bool? CanHitNPC(NPC target)
        {
            if (stabbedNPC is not null)
                return false;
            if (altAttack)
            {
                return true;
            }
            return null;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (altAttack && !target.boss && target.type != NPCID.TargetDummy)
            {
                stabbedNPC = target;
                distanceToNPC = Projectile.Center.Distance(target.Center);
            }
        }

        float originMult;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;

            float rotation = Projectile.rotation + rotOffset;
            Vector2 scale = Vector2.One;

            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor, rotation, originMult * tex.Size(), scale, SpriteEffects.None, 0);

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("DarknessFallenMod/Assets/Glow2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 glowPos = Projectile.Center + rotationDirection * originMult * spearLength - rotationDirection * 25;

            Main.spriteBatch.End();
            Main.spriteBatch.BeginAdditive();

            Main.spriteBatch.Draw(
                texture,
                glowPos - Main.screenPosition,
                null,
                Color.Lerp(Color.Gold, Color.Red, originMult) * originMult * 0.7f * Main.rand.Next(2),
                0,
                texture.Size() * 0.5f,
                (originMult + 1.3f) * 0.3f,
                SpriteEffects.None,
                0
                );

            Main.spriteBatch.End();
            Main.spriteBatch.BeginDefault();
        }
    }
}
