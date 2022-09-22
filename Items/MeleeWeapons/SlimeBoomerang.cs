using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    public class SlimeBoomerang : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Melee;
            Item.width = 62;
            Item.height = 62;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = 1;
            Item.knockBack = 3;
            Item.value = 76320;
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SlimeBoomerangProjectile>();
            Item.shootSpeed = 12f;
            Item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }
    }

    public class SlimeBoomerangProjectile : ModProjectile
    {
        public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/SlimeBoomerang";

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenBoomerang);
            Projectile.aiStyle = ProjAIStyleID.Boomerang;
            AIType = ProjectileID.WoodenBoomerang;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(5)) Dust.NewDust(Projectile.position, Projectile.width, 2, DustID.TintableDust, SpeedY: Projectile.velocity.Y, SpeedX: Projectile.velocity.X, Scale: Main.rand.NextFloat(0.7f, 1.5f), newColor: new Color(0, 255, 80), Alpha: 170);
        }
    }
}
