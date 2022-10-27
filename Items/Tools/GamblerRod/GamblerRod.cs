using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace DarknessFallenMod.Items.Tools.GamblerRod
{
	public class GamblerRod : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambler's Rod");
			Tooltip.SetDefault("Can only be used once a day." +
				"\nFishes up temporary status effects, both good and bad");

			// Allows the pole to fish in lava
			ItemID.Sets.CanFishInLava[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			// These are copied through the CloneDefaults method:
			// Item.width = 24;
			// Item.height = 28;
			// Item.useStyle = ItemUseStyleID.Swing;
			// Item.useAnimation = 8;
			// Item.useTime = 8;
			// Item.UseSound = SoundID.Item1;
			Item.CloneDefaults(ItemID.WoodFishingPole);

			Item.fishingPole = 777; // Sets the poles fishing power
			Item.shootSpeed = 12f; // Sets the speed in which the bobbers are launched. Wooden Fishing Pole is 9f and Golden Fishing Rod is 17f.
			Item.shoot = ModContent.ProjectileType<GamblerRodHook>(); // The Bobber projectile.
		}

        public override void OnCatchNPC(NPC npc, Player player, bool failed)
        {
			
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.GetModPlayer<DarknessFallenPlayer>().HasRodGambled)
            {
				return false;
            }
			return true;
		}

        // Grants the High Test Fishing Line bool if holding the item.
        // NOTE: Only triggers through the hotbar, not if you hold the item by hand outside of the inventory.
        public override void HoldItem(Player player)
		{
			player.accFishingLine = true;
		}
	}
}