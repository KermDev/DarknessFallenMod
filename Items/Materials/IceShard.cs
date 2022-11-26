using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Materials
{
    public class IceShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 9));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            SacrificeTotal = 10;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice(silver: 2));
        }
    }
}