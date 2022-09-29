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
                    path = npc.ModNPC.Texture[0.. (npc.ModNPC.Texture.IndexOf("/") + 1)] + altTexNPC.AltTextureNames[index];
                }

                Asset<Texture2D> texAsset = ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad);
            }
        }
    }
}
