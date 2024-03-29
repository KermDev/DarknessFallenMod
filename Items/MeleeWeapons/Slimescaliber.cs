﻿using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    public class Slimescaliber : ModItem
    {
		public override void SetDefaults()
		{
			Item.width = 49;
			Item.height = 49;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 28;
			Item.useAnimation = 20;
			Item.autoReuse = true;

			Item.DamageType = DamageClass.Melee;
			Item.damage = 28;
			Item.knockBack = 6;
			Item.crit = 5;

			Item.value = Item.buyPrice(silver: 21);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			target.AddBuff(BuffID.Slimed, 180);
			target.AddBuff(ModContent.BuffType<Buffs.SlownessBuff>(), 180);
		}
    }
}
