using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DarknessFallenMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    internal class PhaloriteLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phalorite Leggings");
            Tooltip.SetDefault("20% increased Movement Speed");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = 2000;
            Item.rare = 3;
            Item.defense = 20;
            



        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += .20f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.PhaloriteBar>(), 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }



    }
}
