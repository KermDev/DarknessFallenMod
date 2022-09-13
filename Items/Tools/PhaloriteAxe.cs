using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DarknessFallenMod.Items.Materials;

namespace DarknessFallenMod.Items.Tools
{
    internal class PhaloriteAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phalorite Axe");
            Tooltip.SetDefault("can chop down trees fast... thats it");


        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.scale = 1f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;
            Item.axe = 32;
            Item.useTurn = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 31;
            Item.knockBack = 3.2f;
            Item.crit = 4;

            Item.value = Item.buyPrice(silver: 80);
            Item.rare = ItemRarityID.Blue;

            Item.UseSound = SoundID.Item1;


        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<PhaloriteBar>(20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }



    }
}