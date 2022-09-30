using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Dusts
{
    // doesnt work lol
    public class TintableTrailingDust : ModDust
    {
        public override bool Update(Dust dust)
        {
            if (dust.scale < 0.05f)
            {
                dust.active = false;
            }

            dust.velocity *= 0.98f;
            dust.scale -= 0.05f;
            dust.position += dust.velocity;

            return false;
        }
        public override bool MidUpdate(Dust dust)
        {
            return true;
        }
    }
}
