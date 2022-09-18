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
            if (Main.rand.NextBool(20))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, 3, 4, Alpha: 100, newColor: Color.Green * 0.4f, Scale: 1.4f);
                if (Main.rand.NextBool(2))
                    dust.alpha += 25;
                if (Main.rand.NextBool(2))
                    dust.alpha += 25;
                dust.noLight = true;
                dust.velocity *= 0.2f;
                dust.velocity.Y += 0.2f;
                dust.velocity += player.velocity;
            }
        }
    }
}
