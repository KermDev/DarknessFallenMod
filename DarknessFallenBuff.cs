using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod
{
    public class DarknessFallenBuff : GlobalBuff
    {
        public override void Update(int type, NPC npc, ref int buffIndex)
        {
            switch(type)
            {
                default:
                    break;
                case 30:
                    //bleeding;
                    npc.lifeRegen = -10;
                    break;
            }
        }
    }
}
