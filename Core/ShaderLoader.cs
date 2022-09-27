using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace DarknessFallenMod.Core
{
    public class ShaderLoader : ILoadable
    {
        public void Load(Mod mod)
        {
            if (Main.dedServ) return;

            foreach (string file in mod.GetFileNames())
            {
                if (file.StartsWith("Effects/") && file.EndsWith(".xnb"))
                {
                    string path = file.Replace(".xnb", string.Empty);
                    string name = path.Replace("Effects/", string.Empty);

                    Ref<Effect> fx = new Ref<Effect>(ModContent.Request<Effect>(mod.Name + "/" + path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
                    Filters.Scene[name] = new Filter(new ScreenShaderData(fx, name), EffectPriority.High);
                    Filters.Scene[name].Load();
                }
            }
        }

        public void Unload()
        {

        }
    }
}
