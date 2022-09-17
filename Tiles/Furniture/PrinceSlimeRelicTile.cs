using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace DarknessFallenMod.Tiles.Furniture
{
    public class PrinceSlimeRelicTile : BossRelic
    {
        public override string RelicTextureName => "DarknessFallenMod/Tiles/Furniture/PrinceSlimeRelicTile";
        protected override int RelicItem => ModContent.ItemType<Items.Placeable.Furniture.PrinceSlimeRelic>();
    }
}
