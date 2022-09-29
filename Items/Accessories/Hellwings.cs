using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class Hellwings : ModItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Hot wings ".GetColored(Color.OrangeRed) + $"[i/p57:{ItemID.Skull}]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(60);
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 1.5f;
			constantAscend = 0.135f;
		}

        public override bool WingUpdate(Player player, bool inUse)
        {
			if (!Main.dedServ && inUse && Main.rand.NextBool(3))
            {
				Vector2 pos = player.position;
				pos.X += player.direction == -1 ? player.width : -player.width;
				Dust.NewDust(pos, player.width, player.height, Main.rand.NextFromList(DustID.InfernoFork, DustID.OrangeTorch), SpeedX: -player.direction * Math.Abs(player.velocity.X) * 0.1f, SpeedY: 2, newColor: Color.Lerp(Color.Orange, Color.Yellow, Main.rand.NextFloat()));
			}

            return base.WingUpdate(player, inUse);
        }
    }
}
