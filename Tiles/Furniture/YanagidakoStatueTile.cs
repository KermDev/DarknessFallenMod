using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Tiles.Furniture
{
    public class YanagidakoStatueTile : Statue
    {
        public override int[] NPCToSpawn => new int[] { ModContent.NPCType<NPCs.RedBeetle>() };
        public override int StatueItem => ModContent.ItemType<Items.Placeable.Furniture.YanagidakoStatue>();
        public override int[] CoordinateHeights => new int[] { 20, 16, 18 };
    }
}
