using DarknessFallenMod.Utils;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Core
{
    public static class SoundManager
    {
        static string soundsPath => DarknessFallenUtils.SoundsPath;

        public static SoundStyle BeetleHit => new SoundStyle(soundsPath + "BeetleHit");
        public static SoundStyle BeetleDeath => new SoundStyle(soundsPath + "BeetleDeath");
    }
}
