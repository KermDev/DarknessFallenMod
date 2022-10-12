using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.ObsidianCrusher
{
    public class ObsidianCrusher : ModItem
    {
        public override void SetDefaults()
        {
			Item.damage = 140;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.useStyle = -1;
			Item.knockBack = 8;
			Item.value = 17500;
			Item.rare = 8;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<ObsidianCrusherProjectile>();
			Item.shootSpeed = 9f;
			Item.reuseDelay = 0;
			Item.channel = true;
		}
    }
}
