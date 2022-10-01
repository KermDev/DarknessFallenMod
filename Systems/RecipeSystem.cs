using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System.Linq;

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
    }
}
