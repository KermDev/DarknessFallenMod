using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Accessories
{
    public class HellFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell Flame"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("6% increased damage" + 
                $"\n5% increased critical strike chance");
            ItemID.Sets.ItemIconPulse[Item.type] = true; // The item pulses while in the player's inventory
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = 9825;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player Player, bool hideVisual)
        {
            Player.GetDamage(DamageClass.Generic) += 0.06f;
            Player.GetCritChance(DamageClass.Generic) += 5f;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}