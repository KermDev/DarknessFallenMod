using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.MagmitePitchfork
{
    public class MagmitePitchforkThrownProjectile : ModProjectile
    {
        public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/MagmitePitchfork/MagmitePitchforkProjectile";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);
        }

        public NPC StabbedNPC { get; set; }
        public Vector2 OffsetFromCenter { get; set; }

        int damageMultiplierTimer;
        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;
            Projectile.rotation = Projectile.velocity.ToRotation();

            StabbedNPC.Center = OffsetFromCenter + Projectile.Center;
            StabbedNPC.velocity = Vector2.Zero;

            damageMultiplierTimer++;
            
            if (DarknessFallenUtils.SolidTerrain(StabbedNPC.Hitbox))
            {
                Explode();
                return;
            }

            foreach (NPC npc in Main.npc)
            {
                if (npc.whoAmI != StabbedNPC.whoAmI && !npc.friendly && npc.life > 0 && npc.active && npc.Hitbox.Intersects(StabbedNPC.Hitbox))
                {
                    Explode();
                    return;
                }
            }
        }

        void Explode()
        {
            SoundEngine.PlaySound(SoundID.Item66, Projectile.Center);

            int maxDamageTimer = 120;
            int damage = (int)(Projectile.damage * ((float)Math.Clamp(damageMultiplierTimer, 1, maxDamageTimer) / maxDamageTimer));

            DarknessFallenUtils.ForeachNPCInRange(Projectile.Center, MathF.Pow((StabbedNPC.width > StabbedNPC.height ? StabbedNPC.width : StabbedNPC.height) + 78, 2), npc =>
            {
                if (!npc.friendly && npc.life > 0 && npc.active && npc.immune[Projectile.owner] <= 0)
                {
                    Main.player[Projectile.owner].ApplyDamageToNPC(npc, damage, 2, Projectile.HitDirection(npc.Center), true);

                    DarknessFallenUtils.NewGoreCircular(npc.Center, GoreID.Smoke1 + Main.rand.Next(3), StabbedNPC.width * 0.5f, amount: Main.rand.Next(2, 3), scale: Main.rand.NextFloat(0.6f, 1.2f), speedFromCenter: 4);
                    DarknessFallenUtils.NewDustCircular(npc.Center, DustID.Torch, 15, speedFromCenter: 8, amount: 5);
                }
            });

            Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawAfterImage(prog => Color.Lerp(Color.Red, Color.Yellow, prog) * 0.1f, origin: Vector2.Zero, rotOffset: i => (Projectile.oldRot[i] == 0 ? Projectile.rotation : 0) + MathHelper.PiOver2 + MathHelper.PiOver4, oldPos: true);
            Projectile.DrawProjectileInHBCenter(lightColor, origin: Vector2.Zero, rotOffset: MathHelper.PiOver2 + MathHelper.PiOver4);

            Texture2D tex = ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + MathHelper.PiOver2 + MathHelper.PiOver4, Vector2.Zero, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("DarknessFallenMod/Assets/Glow2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 glowPos = Projectile.Center - Projectile.rotation.ToRotationVector2() * 8;

            Main.spriteBatch.End();
            Main.spriteBatch.BeginAdditive();

            Main.spriteBatch.Draw(
                texture,
                glowPos - Main.screenPosition,
                null,
                Color.Lerp(Color.Gold, Color.Red, 0.5f) * 0.5f,
                0,
                texture.Size() * 0.5f,
                0.7f,
                SpriteEffects.None,
                0
                );

            Main.spriteBatch.End();
            Main.spriteBatch.BeginDefault();
        }
    }
}
