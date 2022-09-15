using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Buffs
{
    public class ExoflameBuff : ModBuff
    {

        public override string Texture => "Terraria/Images/UI/InfoIcon_1";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            
        }
    }
}
