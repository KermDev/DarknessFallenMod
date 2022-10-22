using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.Exothermos
{
    public class ExothermosProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 220;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();


            //DarknessFallenUtils.NewDustCircular(Projectile.Hitbox.TopLeft() + Projectile.Hitbox.Size() * 0.5f + , DustID.FlameBurst, 7).ForEach(d => d.noGravity = true);
            
            for (int i = 0; i < 10; i++)
            {
                int resize = 5;
                Dust.NewDustDirect(Projectile.Hitbox.TopLeft() + (Projectile.velocity.X < 0 ? -3 : -3) * Vector2.UnitY + Vector2.One * resize, Projectile.width - resize, Projectile.height - resize, DustID.FlameBurst).noGravity = true;
            }
            
            Projectile.BasicAnimation(4);
        }

        public override void Kill(int timeLeft)
        {
            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.InfernoFork, 10, speedFromCenter: 2, amount: 12, dustVelocity: -Projectile.velocity * 0.4f);

            Player player = Main.player[Projectile.owner];

            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC temptarget = Main.npc[k];

                float sqrDistanceToTarget = Vector2.Distance(temptarget.Center, Projectile.Center);
                float sqrMaxDetectDistance = 150;

                if (!temptarget.friendly && temptarget.active && temptarget.life > 0 && temptarget.immune[player.whoAmI] <= 0 && sqrDistanceToTarget < sqrMaxDetectDistance)
                {
                    player.ApplyDamageToNPC(temptarget, Projectile.damage, 0f, 1, true);

                    Vector2 pos = temptarget.Center;

                    Vector2 Direction = Projectile.Center - pos;
                    for (int i = 0; i < 20; i++)
                    {
                        Dust.NewDust(pos + i * Direction / 20, new Random().Next(6, 10), new Random().Next(6, 10), DustID.InfernoFork, 0, 0);
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileInHBCenter(Color.White, animated: true, offset: new Vector2(10, 0));
            return false;
        }
    }
}
