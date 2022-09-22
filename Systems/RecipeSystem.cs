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
        public override void AddRecipeGroups()
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
            RecipeGroup.RegisterGroup("Consumables", consumables);
        }
    }
}
