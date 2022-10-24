using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using DarknessFallenMod.Utils;

namespace DarknessFallenMod.Items.Armor.Sandscale
{
    [AutoloadEquip(EquipType.Head)]
    public class SandscaleHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                ""
                );
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 72);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 3;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SandscaleChestplate>() && legs.type == ModContent.ItemType<SandscaleLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "2 Sandstone Shards will slice your enemies (2x damage in the desert)".GetColored(Color.LightGoldenrodYellow);

            int minionType = ModContent.ProjectileType<SandstoneShard>();
            if (player.ownedProjectileCounts[minionType] < 2)
            {
                Projectile proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, Vector2.Zero, minionType, 10, 0, player.whoAmI);
                proj.originalDamage = 24;
            }

            Main.projectile.ForEach(proj =>
            {
                if (proj.owner == player.whoAmI && proj.type == minionType) proj.timeLeft = 10;
            });
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SandstoneScales>(), 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
