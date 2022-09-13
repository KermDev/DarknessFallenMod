using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DarknessFallenMod.Items.Materials;

namespace DarknessFallenMod.Items.Tools
{
    internal class PhaloritePickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phalorite Pickaxe");
            Tooltip.SetDefault("Can mine pre moonlord ores");


        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.scale = 1f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.autoReuse = true;
            Item.pick = 160;
            Item.useTurn = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 22;
            Item.knockBack = 5f;
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