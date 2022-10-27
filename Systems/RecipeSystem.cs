using DarknessFallenMod.Items;
using DarknessFallenMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Linq;
using System;
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.Systems
{
    public class RecipeSystem : ModSystem
    {
        public static void RegisterRecipeGroup(string name, string desc, int iconItem, Func<Item, bool> predicate)
        {
            List<int> types = new List<int>();
            Array.ForEach(ContentSamples.ItemsByType.Values.ToArray(), item =>
            {
                if (predicate.Invoke(item))
                {
                    types.Add(item.type);
                }
            });

            RecipeGroup group = new RecipeGroup(() => name, types.ToArray());
            group.IconicItemId = iconItem;
            group.GetText = () => desc;
            RecipeGroup.RegisterGroup(name, group);
        }

        public override void AddRecipeGroups()
        {
            RegisterRecipeGroup("Consumables", "Potions, food etc.", ItemID.BottledWater, item => item.consumable);

            //ConsumableGroup();
            PlatformGroup();
            TorchGroup();
            FishingRodGroup();
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<Items.Tools.GamblerRod.GamblerRod>());
            recipe
                .AddIngredient(ItemID.Goldfish, stack: 5)
                .AddRecipeGroup(nameof(ItemID.WoodFishingPole))
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        /*
        void ConsumableGroup()
        {
            List<int> types = new List<int>();
            Array.ForEach(ContentSamples.ItemsByType.Values.ToArray(), item =>
            {
                if (item.consumable)
                {
                    types.Add(item.type);
                }
            });

            RecipeGroup consumables = new RecipeGroup(() => "Consumables", types.ToArray());
            consumables.IconicItemId = ItemID.PotionOfReturn;
            consumables.GetText = () => "Potions, food etc.";
            RecipeGroup.RegisterGroup("Consumables", consumables);
        }
        */

        void PlatformGroup()
        {
            List<int> types = new List<int>();
            Array.ForEach(ContentSamples.ItemsByType.Values.ToArray(), item =>
            {
                if (item.createTile == TileID.Platforms)
                {
                    types.Add(item.type);
                }
            });

            RecipeGroup platforms = new RecipeGroup(() => "Platforms", types.ToArray());
            platforms.IconicItemId = ItemID.WoodPlatform;
            platforms.GetText = () => "Any platform";
            RecipeGroup.RegisterGroup("Platforms", platforms);
        }

        void TorchGroup()
        {
            List<int> types = new List<int>();
            Array.ForEach(ContentSamples.ItemsByType.Values.ToArray(), item =>
            {
                if (item.createTile == TileID.Torches)
                {
                    types.Add(item.type);
                }
            });

            RecipeGroup torches = new RecipeGroup(() => "Torches", types.ToArray());
            torches.IconicItemId = ItemID.Torch;
            torches.GetText = () => "Any torch";
            RecipeGroup.RegisterGroup("Torches", torches);
        }

        void FishingRodGroup()
        {
            RecipeGroup FishingRods = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemName(ItemID.WoodFishingPole)}", ItemID.WoodFishingPole, ItemID.ReinforcedFishingPole, ItemID.FisherofSouls, ItemID.Fleshcatcher, ItemID.ScarabFishingRod, ItemID.BloodFishingRod, ItemID.FiberglassFishingPole, ItemID.MechanicsRod, ItemID.SittingDucksFishingRod, ItemID.HotlineFishingHook, ItemID.GoldenFishingRod, ModContent.ItemType<Items.Tools.LightRod.LightRod>());
            RecipeGroup.RegisterGroup(nameof(ItemID.WoodFishingPole), FishingRods);
        }
    }
}
