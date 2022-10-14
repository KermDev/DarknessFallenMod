using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs
{
    public class AngelArmour : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
            DisplayName.SetDefault("Angel Armour");
        }

        public override void SetDefaults()
        {
            NPC.width = 72;
            NPC.height = 72;
            NPC.lifeMax = 180;
            NPC.damage = 17;
            NPC.defense = 35;
            NPC.value = Item.buyPrice(silver: 68);
            NPC.aiStyle = -1;
            NPC.noGravity = true;
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            NPC.BasicAnimation(frameHeight, 10);
        }
    }
}
