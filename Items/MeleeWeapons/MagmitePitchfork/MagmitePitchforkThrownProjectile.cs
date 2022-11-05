using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
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
        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;
            Projectile.rotation = Projectile.velocity.ToRotation();

            StabbedNPC.Center = OffsetFromCenter + Projectile.Center;
            StabbedNPC.velocity = Vector2.Zero;

            
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
            int radius = 2304;

            SoundEngine.PlaySound(SoundID.Item66, Projectile.Center);

            DarknessFallenUtils.ForeachNPCInRange(Projectile.Center, MathF.Pow((StabbedNPC.width > StabbedNPC.height ? StabbedNPC.width : StabbedNPC.height) + 48, 2), npc =>
            {
                if (!npc.friendly && npc.life > 0 && npc.active && npc.immune[Projectile.owner] <= 0)
                {
                    Main.player[Projectile.owner].ApplyDamageToNPC(npc, Projectile.damage, 2, Projectile.HitDirection(npc.Center), true);
                }
            });

            DarknessFallenUtils.NewGoreCircular(StabbedNPC.Center, GoreID.Smoke1 + Main.rand.Next(3), StabbedNPC.width * 0.5f, amount: Main.rand.Next(2, 4));

            Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileInHBCenter(lightColor, origin: Vector2.Zero, rotOffset: MathHelper.PiOver2 + MathHelper.PiOver4);
            return false;
        }
    }
}
