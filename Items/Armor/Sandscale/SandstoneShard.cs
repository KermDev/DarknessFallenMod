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

        Vector2 PlayerFollowPos => Player.Center + Vector2.UnitX * 50 * (Projectile.whoAmI % 2 == 0 ? 1 : -1);
        public override void OnSpawn(IEntitySource source)
        {
            goPos = PlayerFollowPos;
        }

        Vector2 goPos;
        bool attacking;
        public override void AI()
        {
            if (attacking || DarknessFallenUtils.TryGetClosestEnemyNPC(Player.Center, out NPC _, npc => npc.boss, 640000f))
            {
                float distSQ = goPos.DistanceSQ(Projectile.Center);
                if (distSQ < 0.02f)
                {
                    if (!attacking && DarknessFallenUtils.TryGetClosestEnemyNPC(Player.Center, out NPC target, npc => npc.boss, 640000f))
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

            }
            else
            {
                goPos = PlayerFollowPos;
                Projectile.rotation = Player.velocity.ToRotation();
            }

            if (Main.rand.NextBool(15))
            {
                for (int i = 0; i < 1; i++)
                    Dust.NewDustDirect(Projectile.position - Vector2.One * Projectile.width, Projectile.width * 3, Projectile.height * 3, DustID.TreasureSparkle).noGravity = true;
            }

            Projectile.Center = Vector2.Lerp(Projectile.Center, goPos, 0.1f);

            //if (Main.rand.NextBool(7)) Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.Sand);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Player.ZoneDesert || Player.ZoneUndergroundDesert) damage *= 2;
        }

        public override bool MinionContactDamage() => attacking;

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
