using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Tiles.Terrain
{
    public class EmptyBark : Tree
    {
        protected override int[] GrowOn => new int[] { ModContent.TileType<ShadowGrassTile>() };
    }
}
