using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Tiles.Furniture
{
    public class ZombieFingerStatueTile : Statue
    {
        public override int[] NPCToSpawn => new int[] { ModContent.NPCType<NPCs.ZombieFinger>() };
        public override int StatueItem => ModContent.ItemType<Items.Placeable.Furniture.ZombieFingerStatue>();
        public override int[] CoordinateHeights => new int[] { 18, 16, 18 };
    }
}
