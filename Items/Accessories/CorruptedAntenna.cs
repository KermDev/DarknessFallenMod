using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Accessories
{
    public class CorruptedAntenna : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupted Antenna");
            Tooltip.SetDefault("Slightly increases movement speed" + 
                $"\n9% increased attack speed");
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
            Player.moveSpeed += 0.12f;
            Player.GetAttackSpeed(DamageClass.Generic) += 0.09f;
        }
    }
}