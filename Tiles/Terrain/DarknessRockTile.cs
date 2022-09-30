using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Tiles.Terrain
{
    public class DarknessRockTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMerge[Type][ModContent.TileType<DarknessBrickTile>()] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLavaDeath[Type] = false;

            ItemDrop = ModContent.ItemType<Items.Placeable.Terrain.DarknessRock>();
            DustType = DustID.Ash;
            HitSound = SoundID.Dig;

            AddMapEntry(new Color(34, 34, 34));
        }
    }
}
