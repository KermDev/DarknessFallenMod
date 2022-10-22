using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using DarknessFallenMod.Core;

namespace DarknessFallenMod.Items.RangeWeapons.HolyBow
{
    public class HolyBow : ModItem

    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Holy Bow");
            Tooltip.SetDefault("A bow powered by the souls of light");
        }

        public override void SetDefaults()
        {
            Item.damage = 56;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.knockBack = 5;
            Item.value = 34320;
            Item.rare = 8;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LightArrow>();
            Item.shootSpeed = 12f;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            //DarknessFallenUtils.OffsetShootPos(ref position, velocity, Vector2.UnitX * 30);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SoulofLight, 30);
            recipe.AddIngredient(ItemID.HallowedBar, 30);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}