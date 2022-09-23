using Terraria.ID;
using Terraria.ModLoader;
using Terraria;


namespace DarknessFallenMod.Items.Materials
{
    public class PhaloriteBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phalorite Bar");
            Tooltip.SetDefault("A material forged by strong ores");

            Item.value = 8213;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MythrilBar);
            recipe.AddIngredient(ItemID.DemoniteBar);
            recipe.AddIngredient(ItemID.ChlorophyteBar);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }


    }
}
