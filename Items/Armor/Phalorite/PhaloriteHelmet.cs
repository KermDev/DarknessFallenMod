using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DarknessFallenMod.Items.Armor.Phalorite
{
    [AutoloadEquip(EquipType.Head)]
    internal class PhaloriteHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phalorite Helmet");
            Tooltip.SetDefault("13% increased Melee Damage");
        }

        public override void SetDefaults()
        {
            Item.width = 13;
            Item.height = 13;
            Item.value = 1500;
            Item.rare = 3;
            Item.defense = 20;




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
            player.GetDamage(DamageClass.Melee) += .13f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.PhaloriteBar>(), 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }



    }
}
