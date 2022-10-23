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

        const float INERTIA = 3;
        const float SPEED = 4.5f;
        void Attack()
        {
            Vector2 velToTarg = (currentTarget.Center + Main.rand.NextVector2Unit() * currentTarget.width * 0.5f).DirectionFrom(Projectile.Center) * SPEED;
            Projectile.velocity = (Projectile.velocity * (INERTIA - 1) + velToTarg) / INERTIA;

            Projectile.rotation = Projectile.Center.DirectionTo(currentTarget.Center).ToRotation();

            if (!currentTarget.CanBeChasedBy())
            {
                state = AIState.Idle;
                return;
            }
        }

        NPC currentTarget;
        Vector2 PlayerFollowPos => Player.Center + Vector2.UnitX * 50 * (Projectile.whoAmI % 2 == 0 ? 1 : -1);
        void Idle()
        {
            Projectile.velocity = Vector2.Zero;

            Projectile.Center = Vector2.Lerp(Projectile.Center, PlayerFollowPos, 0.04f);

            Projectile.rotation = Projectile.Center.DirectionTo(PlayerFollowPos).ToRotation() + MathHelper.Pi;

            float distSQ = Projectile.Center.DistanceSQ(PlayerFollowPos);
            if (distSQ < 4096 && DarknessFallenUtils.TryGetClosestEnemyNPC(Player.Center, out NPC target, npc => npc.boss, 640000f))
            {
                currentTarget = target;
                state = AIState.Attack;
                return;
            }
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
            if (target.whoAmI == currentTarget.whoAmI) state = AIState.Idle;
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
