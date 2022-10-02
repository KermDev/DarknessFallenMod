using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Core
{
    public interface IAltTexture
    {
        public string[] AltTextureNames { get; }
    }

    public class AltTexGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        Texture2D texture;
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (texture is not null)
            {
                npc.DrawNPCInHBCenter(drawColor, altTex: texture);

                return false;
            }

            return true;
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.ModNPC is not null && npc.ModNPC is IAltTexture altTexNPC)
            {
                int index = Main.rand.Next(altTexNPC.AltTextureNames.Length + 1);
                string path;
                if (index == altTexNPC.AltTextureNames.Length)
                {
                    path = npc.ModNPC.Texture;
                }
                else
                {
                    path = npc.ModNPC.Texture[0..(npc.ModNPC.Texture.LastIndexOf("/") + 1)] + altTexNPC.AltTextureNames[index];
                }

                Main.NewText(path);
                texture = ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad).Value;
            }
        } 
    }
}
