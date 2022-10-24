using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.MoltenUchigatana
{
    public class MoltenUchigatanaProjectile : ModProjectile
    {
        public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/MoltenUchigatana/MoltenUchigatana";

        Player Player => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 24;
        }

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
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Main.netMode == NetmodeID.Server) return;

            Projectile.friendly = true;

            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * Player.itemAnimationMax - 10;

            //float swingAmount = MoltenUchigatana.alt ? MathHelper.Pi : MathHelper.Pi; 
            Projectile.rotation = Projectile.velocity.ToRotation() - (MathHelper.Pi * Player.direction * swingDirection);
            swingDirection = -swingDirection;

            if (!MoltenUchigatana.alt) SoundEngine.PlaySound(SoundID.Item71, Player.Center);
        }

        static int swingDirection = 1;
        Vector2 rotationVector;
        bool shotAlt;

        public override void AI()
        {
            if (Main.netMode == NetmodeID.Server) return;

            // PLAYER AI
            if (Player.ItemAnimationEndingOrEnded)
            {
                //Main.NewText(MoltenUchigatana.speedMultiplier);
                //Main.NewText(Player.itemAnimationMax);
                if (MoltenUchigatana.speedMultiplier < MoltenUchigatana.maxSpeedMult) MoltenUchigatana.speedMultiplier += Player.itemAnimationMax / 240f;
                else MoltenUchigatana.speedMultiplier = MoltenUchigatana.maxSpeedMult;

                Projectile.Kill();
                return;
            }

            Player.heldProj = Projectile.whoAmI;
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            // PROJECTILE AI
            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter);

            if (MoltenUchigatana.alt)
            {
                float lower = 0.20f;

                float threshold = Player.itemAnimationMax * lower;

                if (Player.itemAnimation == (int)threshold && !shotAlt)
                {
                    shotAlt = true;
                    OnSwingAlt();
                }
                else if (Player.itemAnimation < threshold)
                {
                    Projectile.friendly = true;
                    DoSwingAlt(lower);
                }
                else
                {
                    Projectile.friendly = false;
                    Projectile.rotation = Player.MountedCenter.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.Pi * Projectile.direction;
                }
            }
            else 
            {
                DoSwingNormal();
            }

            rotationVector = Projectile.rotation.ToRotationVector2();

            Projectile.direction = Player.direction;
            Projectile.spriteDirection = Projectile.direction;
        }

        void OnSwingAlt()
        {
            SoundEngine.PlaySound(SoundID.Item70, Player.Center);

            var proj = Projectile.NewProjectileDirect(
                Projectile.GetSource_FromThis(),
                Player.MountedCenter - rotationVector * bladeLenght * 0.5f,
                -rotationVector * 25,
                ModContent.ProjectileType<MoltenUchigatanaFireProjectile>(),
                350,
                92,
                Projectile.owner
                );

            proj.spriteDirection = -Projectile.spriteDirection;
            proj.rotation = proj.velocity.ToRotation() + (proj.spriteDirection > 0 ? MathHelper.Pi : 0);
        }

        void DoSwingAlt(float lower)
        {
            float swingBy = MathF.Pow(Player.itemAnimation / (Player.itemAnimationMax * lower), 4) * 0.4f;
            Projectile.rotation += swingBy * Player.direction;

            // FX
            if (swingBy > 0.01f) SpawnSpinDust(2, -26, rotationVector);
        }

        void DoSwingNormal()
        {
            float swingBy = MathF.Pow((float)Player.itemAnimation / Player.itemAnimationMax, 4) * 0.2f * MoltenUchigatana.speedMultiplier;
            Projectile.rotation += swingBy * Player.direction * swingDirection;

            // FX
            if (swingBy > 0.01f) SpawnSpinDust(1, -13);
        }

        float bladeLenght => TextureAssets.Projectile[Type].Value.Width * 1.414213562373095f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Player.MountedCenter, Player.MountedCenter + rotationVector * bladeLenght))
            {
                SpawnSpinDust(14);
                return true;
            }
            return false;
        }

        void SpawnSpinDust(int amount, float speed = 10, Vector2? direction = null)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector2 dir = direction ?? rotationVector;
                Vector2 vel = dir.RotatedBy(MathHelper.PiOver2 * Player.direction) * speed * Main.rand.NextFloat(0.75f, 1.25f);

                Dust.NewDustDirect(
                    Projectile.Center + rotationVector * ((float)i / amount) * bladeLenght + Main.rand.NextFloat(-bladeLenght, bladeLenght) * rotationVector / amount, 
                    0,
                    0, 
                    DustID.AmberBolt, 
                    vel.X, 
                    vel.Y ,
                    Scale: Main.rand.NextFloat(0.4f, 1f),
                    Alpha: Main.rand.Next(0, 120)
                    ).noGravity = true;

                Dust.NewDustDirect(
                    Projectile.Center + rotationVector * ((float)i / amount) * bladeLenght + Main.rand.NextFloat(-bladeLenght, bladeLenght) * rotationVector / amount,
                    0,
                    0,
                    DustID.FireflyHit,
                    vel.X,
                    vel.Y,
                    Scale: Main.rand.NextFloat(0.4f, 1f),
                    Alpha: Main.rand.Next(0, 120)
                    ).noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 texSize = tex.Size();

            float drawRotOffset = (Player.direction == -1 ? -MathHelper.Pi - MathHelper.PiOver4 : MathHelper.PiOver4);
            Vector2 drawPosOffset = new Vector2(0f, Projectile.gfxOffY) + rotationVector * bladeLenght * 0.5f;

            // After Image
            Main.spriteBatch.BeginReset(DarknessFallenUtils.BeginType.Shader, DarknessFallenUtils.BeginType.Default, s =>
            {
                Projectile.DrawAfterImage(
                    prog => Color.Lerp(Color.OrangeRed, Color.White, prog) * 0.1f,
                    rotOffset: i => drawRotOffset,
                    posOffset: i => -Projectile.Center + Player.MountedCenter + Projectile.oldRot[i].ToRotationVector2() * bladeLenght * 0.5f + rotationVector.RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-5, 5),
                    scaleOffset: Vector2.One * (Main.rand.NextBool(7) ? 0.2f : 0)
                    );
            });

            // Blade
            Main.EntitySpriteDraw(
                tex,
                Player.MountedCenter - Main.screenPosition + drawPosOffset,
                null,
                lightColor,
                Projectile.rotation + drawRotOffset,
                texSize * 0.5f,
                1,
                Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0
                );

            return false;
        }
    }
}
