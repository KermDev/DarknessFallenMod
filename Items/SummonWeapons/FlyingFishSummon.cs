using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.SummonWeapons
{
    public class FlyingFishSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Fish Staff");
            Tooltip.SetDefault("[c/005599:Summons a flying fish to fight for you]");
        }

        public readonly static int baseDamage = 8;
        public readonly static int damageDuringRain = 13;
        public override void SetDefaults()
        {
            Item.damage = baseDamage;
            Item.DamageType = DamageClass.Summon;
            Item.width = 37;
            Item.height = 37;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.knockBack = 0;
            Item.noMelee = true;
            Item.mana = 5;

            Item.value = 34320;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.stack = 1;

            Item.shoot = ModContent.ProjectileType<FlyingFishSummonMinion>();
            Item.shootSpeed = 0;
            Item.buffType = ModContent.BuffType<FlyingFishSummonBuff>();
        }

        public override void UseItemFrame(Player player)
        {
            player.itemLocation -= Vector2.UnitY * 12;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            
            int proj = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
            Main.projectile[proj].originalDamage = damage;

            Array.ForEach(DarknessFallenUtils.NewDustCircular(position, DustID.ShadowbeamStaff, 10, speedFromCenter: 5), dust => dust.noGravity = true);

            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var line = tooltips.FirstOrDefault(line => line.Name == "Damage");
            if (line is not null)
            {
                line.Text += $" ({damageDuringRain} during rain)";
            }
        }
    }

    public class FlyingFishSummonMinion : ModProjectile
    {
        Player Player => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;

            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 35;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.damage = 0;
            Projectile.minionSlots = 1f;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;

            Projectile.localNPCHitCooldown = 20;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            currentTarget = Player.Center;
        }

        Vector2 currentTarget;
        const float speed = 9;
        const float inertia = 8;
        public override void AI()
        {
            Projectile.originalDamage = Main.raining ? FlyingFishSummon.damageDuringRain : FlyingFishSummon.baseDamage;

            if (Player.HasBuff<FlyingFishSummonBuff>()) Projectile.timeLeft = 2;

            float moveSpeed = speed;
            if (DarknessFallenUtils.TryGetClosestEnemyNPC(Player.Center, out NPC closest, npc => npc.boss, 640000))
            {
                currentTarget = closest.Center + Main.rand.NextVector2Unit() * closest.width * 0.4f;
            }
            else
            {

                if (Main.rand.NextBool(20)) currentTarget = Player.Center + Main.rand.NextVector2Unit() * 120;
                moveSpeed = Math.Clamp(Projectile.DistanceSQ(currentTarget) * 0.0001f, 1f, 1000f);
            }

            Vector2 velToTarg = Projectile.Center.DirectionTo(currentTarget) * moveSpeed;
            Projectile.velocity = (Projectile.velocity * (inertia - 1) + velToTarg) / inertia;

            Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
            Projectile.rotation = Projectile.velocity.Y * 0.07f * -Projectile.spriteDirection;

            Projectile.BasicAnimation(10);
        }

        public override bool MinionContactDamage() => true;

        public override bool PreDraw(ref Color lightColor)
        {
            DarknessFallenUtils.DrawProjectileInHBCenter(Projectile, lightColor, true, centerOrigin: true);
            return false;
        }
    }

    public class FlyingFishSummonBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Fish");
            Description.SetDefault("A flying fish will fight for you");

            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<FlyingFishSummonMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
