using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DarknessFallenMod.Systems
{
    public class DownedBossSystem : ModSystem
    {
        public static bool downedPrinceSlime;

        public override void OnWorldLoad()
        {
            downedPrinceSlime = false;
        }

        public override void OnWorldUnload()
        {
            downedPrinceSlime = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag[nameof(downedPrinceSlime)] = downedPrinceSlime;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey(nameof(downedPrinceSlime))) downedPrinceSlime = tag.GetBool(nameof(downedPrinceSlime));
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedPrinceSlime;

            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();

            downedPrinceSlime = flags[0];
        }
    }
}
