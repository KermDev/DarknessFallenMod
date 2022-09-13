using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace DarknessFallenMod.Items.Pets
{
	public class BoneCandle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Candle");
			Tooltip.SetDefault("Summons a candle skeleton to help light your way");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<Items.Pets.SkelPetProj>();
			Item.width = 16;
			Item.height = 30;
			Item.UseSound = SoundID.Item2;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.Yellow;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 5, 50);
			Item.buffType = ModContent.BuffType<Buffs.SkelPetBuff>();
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Bone, 100);
			recipe.AddIngredient(ItemID.WaterCandle, 1);
			recipe.AddIngredient(ItemID.Torch, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600);
			}
		}
	}
}