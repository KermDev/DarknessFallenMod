using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI.Chat;

namespace DarknessFallenMod.Utils
{
    public static partial class DarknessFallenUtils
    {
        public static float InverseLerp(float raw, float min, float max)
        {
            return (raw - min) / (max - min);
        }

        public static float Map(float value, float min, float max, float newMin, float newMax)
        {
            return (value - min) / (max - min) * (newMax - newMin) + newMin;
        }
    }
}