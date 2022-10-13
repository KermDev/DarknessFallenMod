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
			Item.damage = 58;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
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

		public static float speedMult;
        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
            {
				speedMult = 0.55f;
            }
            else
            {
				speedMult = 1f;
            }

            return base.CanUseItem(player);
        }

		public override bool AltFunctionUse(Player player) => true;

        public override float UseSpeedMultiplier(Player player)
        {
            return speedMult;
        }
    }
}
