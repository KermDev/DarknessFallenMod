using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DarknessFallenMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    internal class PhaloriteHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phalorite Hood");
            Tooltip.SetDefault("10% increased Magic Damage\nIncreases maximum mana by 150");

        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = 1500;
            Item.rare = 3;
            Item.defense = 7;
            



        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PhaloriteChestplate>() && legs.type == ModContent.ItemType<PhaloriteLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Increases dealt damage by 12% and increased regeneration";
            player.GetDamage(DamageClass.Melee) += .12f;
            player.GetDamage(DamageClass.Ranged) += .12f;
            player.GetDamage(DamageClass.Magic) += .12f;
            player.GetDamage(DamageClass.Summon) += .12f;
            player.AddBuff(BuffID.Regeneration, 4);
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += .1f;
            player.statManaMax2 += 150;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.PhaloriteBar>(), 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }



    }
}
