using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Buffs
{
    public class SlownessBuff : ModBuff
    {
        public float Time;


        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {

        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            Time += 0.016f;
            if (Time >= 0.5f)
            {
                npc.velocity /= 8;
                Time = 0f;
            }
        }
    }
}
