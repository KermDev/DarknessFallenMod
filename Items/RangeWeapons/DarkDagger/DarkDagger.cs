using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;

namespace DarknessFallenMod.Items.RangeWeapons.DarkDagger
{
    public class DarkDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Dagger");
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = 34320;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.DD2_GoblinBomberThrow;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DarkDaggerProj>();
            Item.shootSpeed = 12f;
        }
    }
}
