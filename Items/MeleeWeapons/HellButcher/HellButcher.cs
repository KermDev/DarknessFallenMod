using DarknessFallenMod.Items.MagicWeapons;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.HellButcher
{
    public class HellButcher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell Butcher");
            Tooltip.SetDefault("The power of Hell");
        }

        public override void SetDefaults()
        {
            Item.damage = 400; // doesnt matter whats here check ModifyTooltips
            Item.crit = 34;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = -1;
            Item.knockBack = 8;
            Item.value = 17500;
            Item.rare = 8;;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<HellButcherProjectile>();
            Item.shootSpeed = 9f;
            Item.reuseDelay = 0;
            Item.channel = true;
            Item.useTurn = false;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] == 0;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Find(t => t.Name == "Damage").Text = $"{(HellButcherProjectile.MAX_DAMAGE * (4f / HellButcherProjectile.MAX_DAMAGE_TIMER_COUNT)).ToString("F0")}-{HellButcherProjectile.MAX_DAMAGE} damage";
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.MagmiteBar>(), 15);
            recipe.AddIngredient(ItemID.HellstoneBar, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}