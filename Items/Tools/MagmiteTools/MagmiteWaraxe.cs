using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DarknessFallenMod.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using DarknessFallenMod.Core;

namespace DarknessFallenMod.Items.Tools.MagmiteTools
{
    internal class MagmiteWaraxe : ModItem, IGlowmask
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magmite Waraxe");
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.scale = 1f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 30;
            Item.autoReuse = true;
            Item.axe = 22;
            Item.useTurn = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 22;
            Item.knockBack = 3.2f;
            Item.crit = 4;

            Item.value = Item.buyPrice(silver: 80);
            Item.rare = ItemRarityID.Blue;

            Item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<MagmiteBar>(15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}