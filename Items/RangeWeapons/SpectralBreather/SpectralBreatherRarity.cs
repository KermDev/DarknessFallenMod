using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.RangeWeapons.SpectralBreather
{
    public class SpectralBreatherRarity : ModRarity
    {
        public override Color RarityColor => ColorMethod();

        public static Color ColorMethod()
        {
            return Color.Lerp(Color.Lerp(Color.Purple, Color.White, 0.3f), Color.Orange, MathF.Pow(MathF.Sin(Main.GameUpdateCount * 0.04f), 2));
        }
    }
}
