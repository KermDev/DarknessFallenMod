using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DarknessFallenMod.Items.Materials;
using Microsoft.Xna.Framework.Graphics;

namespace DarknessFallenMod.Items.Tools.MagmiteTools
{
    internal class MagmitePickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magmite Pickaxe");
            Tooltip.SetDefault("Toasty!");


        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.autoReuse = true;
            Item.pick = 110;
            Item.useTurn = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 12;
            Item.knockBack = 5f;
            Item.crit = 4;

            Item.value = Item.buyPrice(silver: 80);
            Item.rare = ItemRarityID.Blue;

            Item.UseSound = SoundID.Item1;

            Item.GetGlobalItem<DarknessFallenItem>().WorldGlowMask = ModContent.Request<Texture2D>(Texture + "Glowmask").Value;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<MagmiteBar>(12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}