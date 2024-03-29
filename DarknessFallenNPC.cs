﻿using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod
{
    public class DarknessFallenNPC : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            switch (type)
            {
                case NPCID.Merchant:
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Throwables.Spearpark>());
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.Utility.AutoPlatform>());
                    break;
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.KingSlime:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.MeleeWeapons.SlimeBoomerang>(), 5));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.MagicWeapons.SlimeCloud>(), 5));
                    break;
                case NPCID.FlyingFish:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.SummonWeapons.FlyingFishSummon>(), 50));
                    break;
            }
        }
    }
}
