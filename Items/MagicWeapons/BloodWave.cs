using DarknessFallenMod.Items.MagicWeapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MagicWeapons
{
    public class BloodWaveBook : ModItem
    {
        public int BloodWaveProjectile { get; private set; }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Holds the power of Waves of blood");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 57;
            Item.DamageType = DamageClass.Magic;
            Item.width = 64;
            Item.height = 48;
            Item.useTime = 20; 
            Item.useAnimation = 20;
            Item.useStyle = 5;
            Item.knockBack = 20;
            Item.noMelee = true;
            Item.value = 18352;
            Item.rare = 5;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<BloodWaveProjectile>();
            Item.shootSpeed = 13f;
            Item.mana = 5;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SoulofFright, 20);
            recipe.AddIngredient(ItemID.SoulofNight, 20);
            recipe.AddIngredient(ItemID.HellstoneBar, 30);
            recipe.AddIngredient(ItemID.Book, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

