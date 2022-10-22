using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Core
{
	public class GlowmaskLoad : ILoadable
	{
		public void Load(Mod mod)
		{
			GlowmaskPlayerDrawLayer.glowMasks = new Dictionary<string, Texture2D>();

			foreach (string file in mod.GetFileNames())
			{
				if (file.EndsWith("_Glow.png"))
				{
					string path = file.Replace(".png", string.Empty);
					string name = path.Replace("_Glow", string.Empty)[(path.LastIndexOf("/"))..];

					GlowmaskPlayerDrawLayer.glowMasks[name] = ModContent.Request<Texture2D>(path).Value;
				}
			}
		}

		public void Unload()
		{

		}
	}
}
