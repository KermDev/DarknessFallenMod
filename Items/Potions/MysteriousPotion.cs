using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Potions
{
    public class MysteriousPotion : ModItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("[c/444444:Hmm I do wonder what it does...]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

			ItemID.Sets.DrinkParticleColors[Type] = new Color[3] {
				new Color(240, 240, 240),
				new Color(200, 200, 200),
				new Color(140, 140, 140)
			};
		}

		public override void SetDefaults()
		{
			Item.width = 23;
			Item.height = 31;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.value = Item.buyPrice(copper: 67);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.maxStack = 30;
			Item.useTurn = true;

			Item.buffType = Main.rand.Next(1, BuffLoader.BuffCount);
			Item.buffTime = Main.rand.Next(120, 900);
		}

        public override void OnConsumeItem(Player player)
        {
			Item.buffType = Main.rand.Next(1, BuffLoader.BuffCount);
			Item.buffTime = Main.rand.Next(120, 900);
		}

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup("Consumables", 2)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}
}
