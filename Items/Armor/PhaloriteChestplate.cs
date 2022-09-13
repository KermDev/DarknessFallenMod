using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DarknessFallenMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    internal class PhaloriteChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phalorite Chestplate");
            Tooltip.SetDefault("15% increased Melee Damage\n10% increased Melee Speed");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = 2000;
            Item.rare = 3;
            Item.defense = 25;
            



        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += .15f;
            player.GetAttackSpeed(DamageClass.Melee) += .1f;
        }

      
       

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.PhaloriteBar>(), 25)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }



    }
}
