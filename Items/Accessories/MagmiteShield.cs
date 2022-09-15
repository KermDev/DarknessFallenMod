using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Accessories
{
    
    public class MagmiteShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                "2 defense" +
                "\nGrants immunity to knockback" +
                "\nGrants immunity to fire blocks" +
                "\n[c/bb6666:Burns attacking enemies]"
                );
        }

        public override void SetDefaults()
        {
            Item.width = 23;
            Item.height = 23;

            Item.value = Item.buyPrice(gold: 4);

            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense += 2;
            player.fireWalk = true;
            player.noKnockback = true;

            player.GetModPlayer<MagmiteShieldPlayer>().MagmiteShieldEquiped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CobaltShield)
                .AddIngredient(ModContent.ItemType<Items.Materials.MagmiteBar>(), 5)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }

    public class MagmiteShieldPlayer : ModPlayer
    {
        public bool MagmiteShieldEquiped;
        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (MagmiteShieldEquiped)
            {
                npc.AddBuff(BuffID.OnFire, 180);
            }
        }

        public override void ResetEffects()
        {
            MagmiteShieldEquiped = false;
        }
    }
}
