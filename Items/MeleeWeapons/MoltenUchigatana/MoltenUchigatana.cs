using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.MoltenUchigatana
{
    public class MoltenUchigatana : ModItem
    {
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("");
			Tooltip.SetDefault("Uchigatana  is a type of katana basically , just wanted a cooler name then katana".GetColored(Color.Gray));
		}

		public static readonly int maxUseTime = 28;
		public override void SetDefaults()
		{
			Item.damage = 140;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = maxUseTime;
			Item.useAnimation = maxUseTime;
			Item.useStyle = -1;
			Item.knockBack = 8;
			Item.value = 17500;
			Item.rare = 8;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<MoltenUchigatanaProjectile>();
			Item.shootSpeed = 9f;
			Item.reuseDelay = 0;
			Item.channel = true;
		}

		public static float speedMultiplier = 1;
		public static readonly float maxSpeedMult = 2f;

        public override void UpdateInventory(Player player)
        {
			if (!player.controlUseItem) speedMultiplier = 1f;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            return speedMultiplier;
        }
    }
}
