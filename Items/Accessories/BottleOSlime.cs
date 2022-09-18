using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Accessories
{
    public class BottleOSlime : ModItem
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Bottle\'O\'Slime");
			Tooltip.SetDefault(
                "25% increased movement speed\n" +
                "[c/00aa22: Watch out its slippery!]"
                );
        }

        public override void SetDefaults()
		{
			Item.width = 17;
			Item.height = 27;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(0, 1);
			Item.accessory = true;
			Item.rare = ItemRarityID.Green;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed *= 1.25f;
            player.slippy = true;
        }
    }
}
