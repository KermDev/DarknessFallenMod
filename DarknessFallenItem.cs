using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod
{
    public class DarknessFallenItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
			//tooltips.Add(new TooltipLine(Mod, "ITEMID", $"Item Type : {item.type}"));
            switch (item.type)
            {
				case 4797:
					tooltips[2].Text += " the 2nd";
					break;
            }
        }
    }
}
