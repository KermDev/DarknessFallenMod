using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Materials
{
    public class RedChitin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Chitin");
            Tooltip.SetDefault("dropped by Red Beetles");

          

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5; // Configure the amount of this item that's needed to research it in Journey mode.
        }

        public override void SetDefaults()
        {
            Item.value = 2300;
            Item.maxStack = 999;
        }
      


    }
}
