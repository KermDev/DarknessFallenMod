using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

using System.Linq;
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.Items.RangeWeapons
{
    public class PhaloriteBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("On arrow hit creates a damaging field.".GetColored(Color.LightCyan));
        }
        public override void SetDefaults()
        {
            Item.damage = 170;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 17;
            Item.height = 39;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = 70000;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.PhaloriteBar>(), 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class PhaloriteBowProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        bool shotFromPhalo;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource)
            {
                if (itemSource.Item.type == ModContent.ItemType<PhaloriteBow>())
                {
                    shotFromPhalo = true;
                }
            }
        }

        public override void Kill(Projectile projectile, int timeLeft)
        {
            if (!shotFromPhalo) return;

            int type = ModContent.ProjectileType<PhaloriteBowAreaProjectile>();
            Player player = Main.player[projectile.owner];

            if (player.ownedProjectileCounts[type] == 3) player.GetOldestProjectile(type).Kill();

            Projectile.NewProjectile(projectile.GetSource_Death(), projectile.Center, Vector2.Zero, type, 4, 0, projectile.owner);
        }
    }

    public class PhaloriteBowAreaProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_1";

        const int radius = 48;
        public override void SetDefaults()
        {
            Projectile.width = radius * 2;
            Projectile.height = radius * 2;

            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.alpha = 255;
            Projectile.originalDamage = 4;
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        ref float timer => ref Projectile.ai[0];
        public override void AI()
        {
            timer++;

            if (timer > 12)
            {
                timer = 0;

                DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.ShadowbeamStaff, radius, speedFromCenter: -6, rotation: Main.rand.NextFloat(MathHelper.TwoPi), amount: 16, scale: Main.rand.NextFloat(0.7f, 1.3f));
                Array.ForEach(
                    DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.BlueTorch, 1, speedFromCenter: 9, rotation: Main.rand.NextFloat(MathHelper.TwoPi), amount: 8, scale: Main.rand.NextFloat(0.9f, 1.6f)),
                    dust => dust.noGravity = true
                    );
            }
        }
    }
}
