using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace DarknessFallenMod.Items.Throwables
{
    public class Spearpark : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spearpark");
            Tooltip.SetDefault("[c/85c992:Lets you double jump off of the thrown spears stuck in tiles]");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;

            Item.shoot = ModContent.ProjectileType<SpearparkProjectile>();
            Item.shootSpeed = 30;

            Item.maxStack = 999;
            Item.value = Item.buyPrice(copper: 12);
            Item.consumable = true;

            Item.damage = 4;
            Item.crit = 66;

            Item.DamageType = DamageClass.Throwing;

            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;

            Item.autoReuse = true;

            Item.noMelee = true;

            Item.rare = 2;
            Item.knockBack = 6;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position.Y -= 8;

            float xOffset = 25 * player.direction;
            if (Collision.CanHit(position, 0, 0, position + xOffset * Vector2.UnitX, 15, 15)) position += xOffset * Vector2.UnitX.RotatedBy(velocity.ToRotation());
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            return 0;
        }
    }

    public class SpearparkMerchantGlobalNPC : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type != NPCID.Merchant) return;

            shop.item[nextSlot] = new Item(ModContent.ItemType<Spearpark>());
            nextSlot++;
        }
    }
}
