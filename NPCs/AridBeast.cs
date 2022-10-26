using DarknessFallenMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs
{
    public class AridBeast : ModNPC
    {
        int frames = 5;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 5;
            DisplayName.SetDefault("Arid Beast");
        }

        public override void SetDefaults()
        {
            NPC.width = 108;
            NPC.height = 60;
            NPC.lifeMax = 110;
            NPC.damage = 9;
            NPC.defense = 18;
            NPC.value = Item.buyPrice(silver: 68);
            NPC.aiStyle = 0;
            NPC.noGravity = false;
        }

        public override void AI()
        {
            #region Animation
            int frameSpeed = 9;
            NPC.frameCounter++;
            if (NPC.frameCounter >= frameSpeed)
            {
                NPC.frameCounter = 0;
                frames++;
                if (frames >= 4)
                {
                    frames = 0;
                }
                NPC.frame = new Rectangle(0, (int)(60 * frames), 100, 60);
            }
            #endregion
            //NOTE am using this way to get animatino because the other method crahses for some reason;
            NPC.ai[0] += 0.0166f;
            if(NPC.ai[0] >= 15f)
            {
                NPC.ai[0] = 0f;
            }
            float SpeedFactor = NPC.ai[0] < 13 ? (NPC.ai[0] < 11f ? 0.5f : 0.2f) : 2f;  

            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                NPC.velocity.Y = 20;
            }

            if (!player.active)
            {
                NPC.velocity.Y += 20f;
            }

            if (NPC.collideX && NPC.velocity.Y == 0)
            {
                NPC.velocity.Y += 6f;
            }

            NPC.spriteDirection = -NPC.direction;
            int TargetDirection = NPC.direction; //-1 is left 1 is right;
            int MoveDirection = NPC.velocity.X < 0 ? -1 : 1;

            if(SpeedFactor == 0.2f)
            {
                Dust.NewDust(NPC.Center, 4, 4, Terraria.ID.DustID.Gold, new Random().Next(-3, 3), new Random().Next(-3, 3));
            }

            if(SpeedFactor == 2f)
            {
                NPC.damage = 18;
            }
            else
            {
                NPC.damage = 9;
            }
            if (TargetDirection != MoveDirection)
            {
                NPC.velocity.X += 0.25f * TargetDirection * SpeedFactor; //going left;
            }
            else
            {
                NPC.velocity.X += 0.5f * TargetDirection * SpeedFactor; //going right
            }

            if(NPC.collideY && NPC.Center.Y > player.Center.Y && (NPC.ai[1] <= 0 || NPC.collideX))
            {
                NPC.ai[1] = 3;
                NPC.velocity.Y -= 7f;
            }

            NPC.ai[1] -= 0.01666f;
        }
    }
}
