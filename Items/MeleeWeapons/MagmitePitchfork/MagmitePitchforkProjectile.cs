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

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
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
            Projectile.netImportant = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 999;
        }

        float rotOffset = MathHelper.PiOver4 + MathHelper.PiOver2;
        bool altAttack;
        public override void OnSpawn(IEntitySource source)
        {
            altAttack = Player.altFunctionUse == 2;

            if (Main.myPlayer == Player.whoAmI)
                Projectile.rotation = Player.Center.DirectionTo(Main.MouseWorld).ToRotation();
            rotationDirection = Projectile.rotation.ToRotationVector2();
        }

        public override bool ShouldUpdatePosition() => false;

        float spearLength => 155f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 startCollision = Projectile.Center + rotationDirection * (originMult - 1f) * spearLength;
            Vector2 endCollision = startCollision + rotationDirection * spearLength;
            //startCollision.ToPoint().DrawPoint(10);
            //endCollision.ToPoint().DrawPoint(10);
            float _ = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), startCollision, endCollision, 20, ref _);
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

            FX();
        }

        NPC stabbedNPC;
        float distanceToNPC;
        void Behaviour()
        {
            if (altAttack)
            {
                if (Main.myPlayer == Player.whoAmI)
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
                            999,
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
                if (Main.myPlayer == Player.whoAmI)
                    Projectile.rotation = Player.Center.DirectionTo(Main.MouseWorld).ToRotation() + MathF.Sin((1f - (float)Player.itemAnimation / Player.itemAnimationMax) * MathHelper.TwoPi) * 0.075f * Player.direction;
                originMult = 0.25f + MathF.Sin((1f - (float)Player.itemAnimation / Player.itemAnimationMax) * MathHelper.Pi) * 0.7f;
            }
        }


        void FX()
        {
            if ((float)Player.itemAnimation / Player.itemAnimationMax > 0.52f && Main.rand.NextBool(14))
            {
                Vector2 pos = Projectile.Center + rotationDirection * spearLength * originMult - rotationDirection * 15;
                Vector2 offset = rotationDirection.RotatedBy(MathHelper.PiOver2) * 15;

                Vector2 velocity = rotationDirection * 4;

                Dust.NewDust(pos, 0, 0, DustID.AmberBolt, velocity.X, velocity.Y, Alpha: (int)(Main.rand.NextFloat() * 255), Scale: Main.rand.NextFloat(0.4f, 1f));
                Dust.NewDust(pos + offset, 0, 0, DustID.AmberBolt, velocity.X, velocity.Y, Alpha: (int)(Main.rand.NextFloat() * 255), Scale: Main.rand.NextFloat(0.4f, 1f));
                Dust.NewDust(pos - offset, 0, 0, DustID.AmberBolt, velocity.X, velocity.Y, Alpha: (int)(Main.rand.NextFloat() * 255), Scale: Main.rand.NextFloat(0.4f, 1f));
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
            if (altAttack && (float)Player.itemAnimation / Player.itemAnimationMax > 0.52f && !target.boss && target.type != NPCID.TargetDummy)
            {
                stabbedNPC = target;
                distanceToNPC = Projectile.Center.Distance(target.Center);

                DarknessFallenUtils.NewDustCircular(target.Center, DustID.Blood, 15, amount: 32, speedFromCenter: 5);
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (altAttack) damage = 1;
        }

        float originMult;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;

            float rotation = Projectile.rotation + rotOffset;
            Vector2 scale = Vector2.One;

            if (altAttack) Projectile.DrawAfterImage(prog => Color.Lerp(Color.Red, Color.Yellow, prog) * 0.05f, origin: originMult * tex.Size(), rotOffset: i => Projectile.oldRot[i] == 0 ? Projectile.rotation + rotOffset : rotOffset, oldPos: false);

            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor, rotation, originMult * tex.Size(), scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, Projectile.Center - Main.screenPosition, null, Color.White, rotation, originMult * tex.Size(), scale, SpriteEffects.None, 0);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("DarknessFallenMod/Assets/Glow2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 glowPos = Projectile.Center + rotationDirection * originMult * spearLength - rotationDirection * 8;

            if (!Main.dedServ) 
                Lighting.AddLight(glowPos, 0.7f, 0.22f, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.BeginAdditive();

            Main.spriteBatch.Draw(
                texture,
                glowPos - Main.screenPosition,
                null,
                Color.Lerp(Color.Gold, Color.Red, originMult) * originMult * 0.7f,
                0,
                texture.Size() * 0.5f,
                (originMult - 0.4f) * 1.2f,
                SpriteEffects.None,
                0
                );

            Main.spriteBatch.End();
            Main.spriteBatch.BeginDefault();
        }
    }
}
