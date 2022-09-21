using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Tiles.Furniture
{
    public class MawStatueTile : Statue
    {
        public override int[] NPCToSpawn => new int[] { ModContent.NPCType<NPCs.CrimMaw>(), ModContent.NPCType<NPCs.CorrMaw>() };
        public override int StatueItem => ModContent.ItemType<Items.Placeable.Furniture.MawStatue>();
        public override string MapName => "Maw Statue";
        public override int[] CoordinateHeights => new int[3] { 18, 18, 18 };
    }
}
