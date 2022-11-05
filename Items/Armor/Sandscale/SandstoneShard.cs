using DarknessFallenMod.Utils;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

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
            Projectile.localNPCHitCooldown = 120;
        }

        Player Player => Main.player[Projectile.owner];

        enum AIState
        {
            Idle,
            Attack
        }

        AIState state = AIState.Idle;
        public override void AI()
        {
            switch (state)
            {
                case AIState.Idle:
                    Idle();
                    break;
                case AIState.Attack:
                    Attack();
                    break;
            }

            FX();
        }


        const float SPEED = 9f;

        bool stop;

        Vector2 originalPos;
        void Attack()
        {
            if (!stop)
            {
                float distSQ = Projectile.Center.DistanceSQ(originalPos);
                if (distSQ > distToEnemy)
                {
                    stop = true;
                }
            }
            else
            {
                Projectile.velocity *= 0.97f;

                float velLengthSQ = Projectile.velocity.LengthSquared();

                if (velLengthSQ < 0.0001f)
                {
                    stop = false;

                    if (DarknessFallenUtils.TryGetClosestEnemyNPC(Player.Center, out NPC target, npc => npc.boss, MAXDIST))
                    {
                        AttackTarget(target);

                        return;
                    }
                    else
                    {
                        state = AIState.Idle;
                        return;
                    }
                }
            }
        }

        Vector2 PlayerFollowPos => Player.Center + Vector2.UnitX * 50 * (Projectile.whoAmI % 2 == 0 ? 1 : -1);
        const float MAXDIST = 640000f;
        float distToEnemy;
        void Idle()
        {
            Projectile.Center = Vector2.Lerp(Projectile.Center, PlayerFollowPos, 0.04f);
            Projectile.rotation = Player.velocity.ToRotation();

            if (DarknessFallenUtils.TryGetClosestEnemyNPC(Player.Center, out NPC target, npc => npc.boss, MAXDIST))
            {
                AttackTarget(target);
                
                return;
            }
        }

        void AttackTarget(NPC target)
        {
            originalPos = Projectile.Center;
            distToEnemy = Projectile.Center.DistanceSQ(target.Center);

            Projectile.velocity = Projectile.Center.DirectionTo(target.Center) * SPEED;
            Projectile.rotation = Projectile.velocity.ToRotation();

            state = AIState.Attack;
        }

        void FX()
        {
            if (Main.rand.NextBool(10))
            {
                for (int i = 0; i < 1; i++)
                    Dust.NewDustDirect(Projectile.position - Vector2.One * Projectile.width, Projectile.width * 3, Projectile.height * 3, DustID.TreasureSparkle).noGravity = true;
            }

            //if (Main.rand.NextBool(7)) Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.Sand);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Player.ZoneDesert || Player.ZoneUndergroundDesert) damage *= 2;
        }

        
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            stop = true;
        }

        public override bool MinionContactDamage() => state == AIState.Attack;

        public override bool PreDraw(ref Color lightColor)
        {
            var fx = Filters.Scene["SandstoneShard"].GetShader().Shader;
            var tex = TextureAssets.Projectile[Type].Value;

            fx.Parameters["imageSize"].SetValue(tex.Size());
            fx.Parameters["time"].SetValue(Main.GameUpdateCount * 0.1f);

            Main.spriteBatch.End();
            Main.spriteBatch.BeginShader();
            fx.CurrentTechnique.Passes[0].Apply();

            Projectile.DrawProjectileInHBCenter(lightColor, centerOrigin: true);

            Main.spriteBatch.End();
            Main.spriteBatch.BeginDefault();
            return false;
        }
    }
}
