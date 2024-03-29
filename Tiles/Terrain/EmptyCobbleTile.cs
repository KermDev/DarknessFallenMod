﻿using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Tiles.Terrain
{
    public class EmptyCobbleTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMerge[Type][ModContent.TileType<ShadowGrassTile>()] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLavaDeath[Type] = false;

            DustType = DustID.Stone;
            HitSound = SoundID.Dig;
        }
    }
}
