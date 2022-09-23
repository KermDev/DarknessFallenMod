using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Items.Materials;
using System.Collections.ObjectModel;

namespace DarknessFallenMod.Items.MeleeWeapons
{
	public class Valika : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valika"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("The sword powered by the Souls of all creatures");
		}

		public override void SetDefaults()
		{
			Item.damage = 340;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 13;
			Item.useAnimation = 13;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = 195300;
			Item.rare = 8;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<ValikaProjectile>();
			Item.shootSpeed = 22f;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient <SoulOfSpirits> (50); 
            recipe.AddIngredient(ItemID.LunarBar, 15);
            recipe.AddIngredient(ItemID.HellstoneBar, 20);
            recipe.AddIngredient(ItemID.MeteoriteBar, 20);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 20);
            recipe.AddIngredient(ItemID.MythrilBar, 20);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
			//if (line.Name == "ItemName") DarknessFallenUtils.DrawTooltipLineEffect(line, line.X, line.Y, DarknessFallenUtils.TooltipLineEffectStyle.Epileptic);

			return base.PreDrawTooltipLine(line, ref yOffset);
        }
    }
}