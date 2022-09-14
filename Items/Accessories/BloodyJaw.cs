using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Accessories
{
    public class BloodyJaw : ModItem
    {
        private const float V = 0.25f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BloodyJaw"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("Gives 4% additional damage" + 
                $"\n3% increased critical damage chance");
            //ItemID.Sets.ItemIconPulse[Item.type] = true; // The item pulses while in the player's inventory
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
            Player.GetDamage(DamageClass.Generic) += 0.04f;
            Player.GetCritChance(DamageClass.Generic) += 3f;

        }


    }
}