using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.Localization;

namespace DarknessFallenMod.Systems.Currencies
{
	public class GelCurrency : CustomCurrencySingleCoin
	{
		public GelCurrency(int coinItemID, long currencyCap, string CurrencyTextKey) : base(coinItemID, currencyCap)
		{
			this.CurrencyTextKey = CurrencyTextKey;
			CurrencyTextColor = Color.LightBlue;
		}
	}
}