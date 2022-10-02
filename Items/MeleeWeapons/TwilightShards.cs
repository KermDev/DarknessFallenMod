using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using System.IO;
using Terraria.Audio;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    public class TwilightShards : ModItem
    {
        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.knockBack = 6.75f;
            Item.width = 30;
            Item.height = 10;
            Item.damage = 32;
            Item.crit = 7;
            Item.scale = 1.1f;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<TwilightShardsProjectile>();
            Item.shootSpeed = 12f;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.channel = true;
            Item.noMelee = true;
        }
    }

    public class TwilightShardsProjectile : Templates.FlailProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        
        public override void AI()
        {
            //Main.NewText($"AI0:{Projectile.ai[0]}, AI1:{Projectile.ai[1]}");

            if (Main.netMode != NetmodeID.MultiplayerClient && !Owner.controlUseItem) 
            {
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<TwilightShardsProjectileShoot>(), 20, 0, Projectile.owner);
                proj.friendly = true;
                proj.hostile = false;
                proj.DamageType = DamageClass.MeleeNoSpeed;

                Projectile.Kill();
            }
        }
        

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(SoundID.Shatter with { Volume = 0.4f }, Projectile.Center);

            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.Glass, 10, speedFromCenter: 6, amount: Main.rand.Next(10, 24)).ForEach(dust => dust.alpha = Main.rand.Next(0, 220));
        }
    }

    public class TwilightShardsProjectileShoot : ModProjectile
    {
        public override string Texture => base.Texture.Replace("TwilightShardsProjectileShoot", "TwilightShardsProjectile");

        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.penetrate = 10;
            Projectile.aiStyle = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 190;
            Projectile.extraUpdates = 2;

            Projectile.localNPCHitCooldown = 10;
            Projectile.usesLocalNPCImmunity = true;
        }
    }
}
