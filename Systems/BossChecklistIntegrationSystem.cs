using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Systems
{
    public class BossChecklistIntegrationSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod)) return;

            bossChecklistMod.Call(
                "AddBoss",
                Mod,
                "Prince Slime",
                ModContent.NPCType<NPCs.Bosses.PrinceSlime>(),
                0.9f,
                () => DownedBossSystem.downedPrinceSlime,
                () => true,
                new List<int>() { ModContent.ItemType<Items.Placeable.Furniture.PrinceSlimeRelic>(), ModContent.ItemType<Items.Placeable.Furniture.PrinceSlimeTrophy>() },
                0,
                "Spawns during slime rain.",
                null
                );
        }
    }
}
