using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Tiles.Terrain
{
    public class DarknessBrickTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMerge[Type][ModContent.TileType<DarknessRockTile>()] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLavaDeath[Type] = false;

            ItemDrop = ModContent.ItemType<Items.Placeable.Terrain.DarknessBrick>();
            DustType = DustID.Ash;
            HitSound = SoundID.Dig;

            AddMapEntry(new Color(22, 22, 22));
        }
    }
}
