using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Materials
{
    public class CrimFlesh : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Flesh");
            Tooltip.SetDefault("Remains that are in the crimson for long enough degrades and absorbs the crimson.");

          

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5; // Configure the amount of this item that's needed to research it in Journey mode.
        }

        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(silver: 7);
            Item.maxStack = 999;
        }
    }
}
