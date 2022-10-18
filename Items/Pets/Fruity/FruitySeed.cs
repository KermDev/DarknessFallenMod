using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace DarknessFallenMod.Items.Pets.Fruity
{
    public class FruitySeed : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fruity Seed");
            Tooltip.SetDefault("Summons an fruity light");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ModContent.ProjectileType<FruityPetProj>();
            Item.width = 16;
            Item.height = 30;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.rare = ItemRarityID.Yellow;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 5, 50);
            Item.buffType = ModContent.BuffType<Pets.Fruity.FruityPetBuff>();
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Apple, 2);
            recipe.AddIngredient(ItemID.Apricot, 2);
            recipe.AddIngredient(ItemID.Grapefruit, 2);
            recipe.AddIngredient(ItemID.Lemon, 2);
            recipe.AddIngredient(ItemID.Peach, 2);
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