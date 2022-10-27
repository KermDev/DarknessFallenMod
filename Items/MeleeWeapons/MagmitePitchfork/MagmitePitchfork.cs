using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.MagmitePitchfork
{
    public class MagmitePitchfork : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Greatsword");
			Tooltip.SetDefault("A great sword powered by the souls of light");
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
			Item.shoot = ModContent.ProjectileType<MagmitePitchforkProjectile>();
			Item.shootSpeed = 9f;
			Item.reuseDelay = 0;
		}
	}
}
