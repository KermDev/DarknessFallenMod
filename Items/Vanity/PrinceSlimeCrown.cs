using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class PrinceSlimeCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 21;
			Item.height = 11;

			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(silver: 32);
			Item.vanity = true;
			Item.maxStack = 1;
			
		}

        public override void EquipFrameEffects(Player player, EquipType type)
        {
			if (Main.rand.NextBool(15)) Dust.NewDust(player.position, player.width, 1, DustID.Crimslime, newColor: Color.Green);
        }
    }
}
