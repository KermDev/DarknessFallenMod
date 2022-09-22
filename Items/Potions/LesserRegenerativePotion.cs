using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Potions
{
    public class LesserRegenerativePotion : ModItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Restores 50 health and gives regeneration");
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.value = Item.buyPrice(silver: 1, copper: 20);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item3;
			Item.autoReuse = true;
			Item.potion = true;
			Item.consumable = true;
			Item.healLife = 50;
			Item.maxStack = 30;
			Item.buffType = BuffID.Regeneration;
			Item.buffTime = 1200;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LesserHealingPotion, 2);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
}
}
