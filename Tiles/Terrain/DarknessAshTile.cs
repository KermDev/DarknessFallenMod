using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Tiles.Terrain
{
    public class DarknessAshTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMerge[Type][TileID.Ash] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLavaDeath[Type] = false;

            DustType = DustID.Ash;
            HitSound = SoundID.Dig;
        }
    }
}
