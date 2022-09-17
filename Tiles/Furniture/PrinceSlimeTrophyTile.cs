using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DarknessFallenMod.Tiles.Furniture
{
    public class PrinceSlimeTrophyTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            this.SetTrophy();

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Prince Slime Trophy");
            AddMapEntry(Color.GreenYellow, name);

            DustType = 7;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<Items.Placeable.Furniture.PrinceSlimeTrophy>());
    }
}
