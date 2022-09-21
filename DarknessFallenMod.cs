using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI;

namespace DarknessFallenMod
{
	public class DarknessFallenMod : Mod
	{
		public static int GelCurrency;

		public override void Load()
		{
			// Registers a new custom currency
			GelCurrency = CustomCurrencyManager.RegisterCurrency(new Systems.Currencies.GelCurrency(ItemID.Gel, 999L, "Gel"));
		}
	}
}