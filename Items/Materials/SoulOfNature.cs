using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Materials
{
    public class SoulOfNature : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul of Nature");
            Tooltip.SetDefault("the soul of nature creatures");

            // Registers a vertical animation with 4 frames and each one will last 5 ticks (1/12 second)
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation

            ItemID.Sets.ItemIconPulse[Item.type] = true; // The item pulses while in the player's inventory
            ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25; // Configure the amount of this item that's needed to research it in Journey mode.
        }

        public override void SetDefaults()
        {
            Item.value = 2300;
            Item.maxStack = 999;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.LightCyan.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }

      
    }
}
