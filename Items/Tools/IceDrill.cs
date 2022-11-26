using DarknessFallenMod.Items.Materials;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Tools
{
    public class IceDrill : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsDrill[Type] = true;
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.Size = new(20, 12);
            Item.SetWeaponValues(7, 0f);
            Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice(silver: 50));

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<IceDrillProjectile>();
            Item.shootSpeed = 32f;
            Item.useAnimation = 25;
            Item.useTime = 9;
            Item.UseSound = SoundID.Item23;
            Item.pick = 55;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<IceShard>(8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class IceDrillProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Size = new(22, 22);
            Projectile.aiStyle = ProjAIStyleID.Drill;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 0.9f;
        }
    }
}