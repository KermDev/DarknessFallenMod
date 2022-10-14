using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    public class FungiteSword : ModItem
    {
        public override void SetDefaults()
        {
			Item.width = 40;
			Item.height = 40;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.autoReuse = true;

			Item.DamageType = DamageClass.Melee;
			Item.damage = 19;
			Item.knockBack = 6;
			Item.crit = 6;

			Item.value = Item.buyPrice(silver: 70);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item1;
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            float speed = 2;
            int dmg = 9;
            float randX = Main.rand.NextFloat(0.2f, 1f);
            Projectile.NewProjectileDirect(Item.GetSource_FromThis(), target.Center, new Vector2(speed * randX, -speed * 2), ModContent.ProjectileType<FungiteSwordProjectile>(), dmg, knockBack, player.whoAmI).netUpdate = true;
            Projectile.NewProjectileDirect(Item.GetSource_FromThis(), target.Center, new Vector2(-speed * randX, -speed * 2), ModContent.ProjectileType<FungiteSwordProjectile>(), dmg, knockBack, player.whoAmI).netUpdate = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.FungiteBar>(), 20)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

	public class FungiteSwordProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.GlowingMushroom;

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;

            Projectile.knockBack = 0;

            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 500;
            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.LengthSquared() * 0.03f;
            Projectile.velocity.Y += 0.1f;

            if (Main.rand.NextBool(12))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GlowingMushroom, Scale: Main.rand.NextFloat(0.2f, 1f));
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, Scale: Main.rand.NextFloat(0.2f, 1f));
            }
            if (!Main.dedServ) Lighting.AddLight(Projectile.Center, TorchID.Purple);
        }

        public override void OnSpawn(IEntitySource source)
        {
            indexToIgnore = Main.npc.FirstOrDefault(npc => Projectile.Hitbox.Intersects(npc.Hitbox))?.whoAmI ?? -1;
        }

        int indexToIgnore;
        public override bool? CanHitNPC(NPC target)
        {
            if (indexToIgnore == target.whoAmI) return false;

            return null;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++) Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GlowingMushroom, Scale: Main.rand.NextFloat(0.2f, 1f));
            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.ShadowbeamStaff, 10, speedFromCenter: 8);
        }
    }
}
