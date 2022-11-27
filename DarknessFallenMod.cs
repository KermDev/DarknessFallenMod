using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI;
using DarknessFallenMod.Core;
using static Terraria.ModLoader.Core.TmodFile;
using System.Reflection;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.Core;
using System.Linq;

namespace DarknessFallenMod
{
	public class DarknessFallenMod : Mod
	{
		public static int GelCurrency;
		public static DarknessFallenMod Instance { get; private set; }
		public static Effect TrailShader;
		public override void Load()
		{
			Instance = this;
			TrailShader = ModContent.Request<Effect>("DarknessFallenMod/Effects/TrailShader").Value;
			// Registers a new custom currency
			GelCurrency = CustomCurrencyManager.RegisterCurrency(new Systems.Currencies.GelCurrency(ItemID.Gel, 999L, "Gel"));
			LoadShaders();
		}
		public override void Unload()
		{
			Instance = null;
			TrailShader = null;
		}

        public void LoadShaders()
        {
            if (Main.dedServ)
                return;

            MethodInfo info = typeof(Mod).GetProperty("File", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(true);
            var file = (TmodFile)info.Invoke(Instance, null);

            var shaders = file.Where(n => n.Name.StartsWith("Effects/") && n.Name.EndsWith(".xnb"));

            foreach (FileEntry entry in shaders)
            {
                var name = entry.Name.Replace(".xnb", "").Replace("Effects/", "");
                var path = entry.Name.Replace(".xnb", "");
                LoadShader(name, path);
            }
        }


        public static void LoadShader(string name, string path)
        {
            var screenRef = new Ref<Effect>(Instance.Assets.Request<Effect>(path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Terraria.Graphics.Effects.Filters.Scene[name] = new Filter(new ScreenShaderData(screenRef, name + "Pass"), EffectPriority.High);
            Terraria.Graphics.Effects.Filters.Scene[name].Load();
        }
    }
}