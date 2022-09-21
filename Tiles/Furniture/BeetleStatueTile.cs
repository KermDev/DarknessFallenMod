using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Tiles.Furniture
{
    public class BeetleStatueTile : Statue
    {
        public override int StatueItem => ModContent.ItemType<Items.Placeable.Furniture.BeetleStatue>();
    }
}
