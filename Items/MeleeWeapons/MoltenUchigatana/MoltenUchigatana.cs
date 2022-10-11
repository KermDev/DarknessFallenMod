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

		public override void SetDefaults()
		{
			Item.damage = 140;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 45;
			Item.useAnimation = 45;
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
		}
	}
}
