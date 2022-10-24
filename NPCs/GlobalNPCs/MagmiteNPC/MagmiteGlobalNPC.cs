using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs.GlobalNPCs.MagmiteNPC
{
    public class MagmiteGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public Texture2D MagmiteTexture;
        
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            /*
            string path = "DarknessFallenMod/NPCs/GlobalNPCs/MagmiteNPC/";
            switch (npc.type)
            {
                case NPCID.CaveBat:
                    MagmiteTexture = ModContent.Request<Texture2D>(path + "MagmiteBat", AssetRequestMode.ImmediateLoad).Value;
                    break;
            }
            */
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (MagmiteTexture is not null)
            {
                spriteBatch.Draw(MagmiteTexture, npc.Center - screenPos, npc.frame, drawColor, npc.rotation, MagmiteTexture.Size() * 0.5f, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

                return false;
            }
            else return true;
        }
    }
}
