using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.RangeWeapons
{
    public class ScaleLongbow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scale Longbow");
			Tooltip.SetDefault("placeholder tooltip");
		}

		public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 20;
            Item.height = 54;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(6537);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
            Item.noMelee = true;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.ZoneDesert)
            {
                damage *= 2;
            }
        }

        public override void AddRecipes()
        {
			CreateRecipe()
                //.AddIngredient(ModContent.ItemType<SandstoneScale>(), 7) Uncomment this line when Sandstone Scale is added
                .AddTile(TileID.WorkBenches)
				.Register();
        }
    }
}
