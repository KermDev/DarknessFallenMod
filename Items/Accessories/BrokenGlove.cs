using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Accessories
{
    public class BrokenGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Slightly increases attack speed" +
                $"\n in memory of RocketLauncher269");
        }

        public override void SetDefaults()
        {
            Item.width = 29;
            Item.height = 29;
            Item.value = Item.buyPrice(silver: 40);
            Item.rare = ItemRarityID.Gray;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player Player, bool hideVisual)
        {
            Player.GetAttackSpeed(DamageClass.Generic) += 0.06f;
        }
    }
}
