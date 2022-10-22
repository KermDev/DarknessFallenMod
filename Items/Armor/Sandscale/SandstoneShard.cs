using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Armor.Sandscale
{
    public class SandstoneShard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 13;
            Projectile.height = 13;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 20;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.netImportant = true;

            Projectile.extraUpdates = 4;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
        }

        Player Player => Main.player[Projectile.owner];

        Vector2 PlayerFollowPos => Player.Center + Vector2.UnitX * Player.direction * -50 * (Projectile.whoAmI % 2 == 0 ? 1 : -1);
        public override void OnSpawn(IEntitySource source)
        {
            goPos = PlayerFollowPos;
        }

        Vector2 goPos;
        bool attacking;
        public override void AI()
        {
            float distSQ = goPos.DistanceSQ(Projectile.Center);
            if (distSQ < 0.02f)
            {
                if (!attacking && DarknessFallenUtils.TryGetClosestEnemyNPC(Player.Center, out NPC target, 640000f))
                {
                    goPos = target.Center;
                    attacking = true;
                }
                else
                {
                    goPos = PlayerFollowPos;
                    attacking = false;
                }

                Projectile.rotation = Projectile.Center.DirectionTo(goPos).ToRotation();
            }

            Projectile.Center = Vector2.Lerp(Projectile.Center, goPos, 0.1f);

            //if (Main.rand.NextBool(7)) Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.Sand);

            if (attacking)
            {
                for (int i = 0; i < 1; i++)
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Sand).noGravity = true;
            }
        }

        public override bool MinionContactDamage() => attacking;

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileInHBCenter(lightColor, centerOrigin: true);
            return false;
        }
    }
}
