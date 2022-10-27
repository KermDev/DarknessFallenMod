using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.RangeWeapons
{
    public class MagmiteBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots 2 arrows instead of 1".GetColored(Color.Orange) + "\nInflicts an OnFire debuff on arrow hit\nDoes [c/bb6666:2x] the damage when using Fire Arrows");
        }

        public override void SetDefaults()
        {
            Item.damage = 34;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 17;
            Item.height = 39;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(silver: 85);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
            Item.noMelee = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 offset = new Vector2(0, 8).RotatedBy(velocity.ToRotation());
            Vector2 pos = position + offset;
            for (int i = 0; i < 2; i++)
            {
                Projectile proj = Projectile.NewProjectileDirect(source, pos, velocity, type, damage, knockback, player.whoAmI);

                proj.usesIDStaticNPCImmunity = false;
                proj.usesLocalNPCImmunity = true;
                proj.localNPCHitCooldown = 60;

                pos -= 2 * offset;
            }

            return false;
        }

        //public override void UseItemFrame(Player player)
        //{
        //    player.itemLocation += Vector2.UnitX.RotatedBy(player.itemRotation) * -8 * player.direction;
        //}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.MagmiteBar>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class MagmiteBowProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        bool shotFromMagmite;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource)
            {
                if (itemSource.Item.type == ModContent.ItemType<MagmiteBow>())
                {
                    shotFromMagmite = true;
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (!shotFromMagmite) return;

            target.AddBuff(BuffID.OnFire, 240);
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (!shotFromMagmite) return;

            if (projectile.type == ProjectileID.HellfireArrow || projectile.type == ProjectileID.FlamingArrow || projectile.type == ProjectileID.FireArrow)
            {
                damage *= 2;
            }
        }
    }
}
