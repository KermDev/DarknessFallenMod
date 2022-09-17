using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class SlimeGuardHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 21;
            Item.height = 19;

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 32);
            Item.vanity = true;
            Item.maxStack = 1;
        }
    }
}
