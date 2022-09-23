using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Consumables
{
    public class PrinceSlimeBossBag : BossBag
    {
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.MeleeWeapons.Slimescaliber>(), 2));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.SummonWeapons.CultSlime>(), 4));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.BottleOSlime>(), 3));

			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.MagicWeapons.SlimyRain>(), 100));

			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.SlimeGuardHelmet>(), 3));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.SlimeGuardChestplate>(), 3));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.SlimeGuardLeggings>(), 3));
		}
	}
}
