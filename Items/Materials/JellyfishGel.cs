using Terraria;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Materials
{
    public class JellyfishGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jellyfish Gel");
            Tooltip.SetDefault("Dropped by ");

            SacrificeTotal = 5; // Configure the amount of this item that's needed to research it in Journey mode.
        }

        public override void SetDefaults()
        {
            Item.value = 2300;
            Item.maxStack = 9999;
        }
    }
}
