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
using DarknessFallenMod.Core;

namespace DarknessFallenMod
{
	public class DarknessFallenMod : Mod
	{
		public static int GelCurrency;
		public static DarknessFallenMod Instance { get; private set; }

		public override void Load()
		{
			Instance = this;

			// Registers a new custom currency
			GelCurrency = CustomCurrencyManager.RegisterCurrency(new Systems.Currencies.GelCurrency(ItemID.Gel, 999L, "Gel"));
		}

	}
}