using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
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
			//DisplayName.SetDefault("");
			Tooltip.SetDefault("Use RMB to stab enemies then LMB to throw them with tremendous force.".GetColored(Color.OrangeRed));
		}

		public override void SetDefaults()
		{
			Item.damage = 85;
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
			Item.shoot = ModContent.ProjectileType<MagmitePitchforkProjectile>();
			Item.shootSpeed = 9f;
			Item.reuseDelay = 0;
		}

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<MagmitePitchforkThrownProjectile>()] == 0;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override void AddRecipes()
        {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Materials.MagmiteBar>(), 15);
			recipe.AddIngredient(ItemID.HellstoneBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}
