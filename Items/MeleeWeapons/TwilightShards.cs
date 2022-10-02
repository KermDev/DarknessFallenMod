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
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.knockBack = 6.75f;
            Item.width = 30;
            Item.height = 10;
            Item.damage = 71;
            Item.crit = 7;
            Item.scale = 1.1f;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<TwilightShardsProjectile>();
            Item.shootSpeed = 12f;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 6);
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

            if (Main.myPlayer == Owner.whoAmI && !Owner.controlUseItem && Projectile.ai[0] == 2) 
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.oldVelocity, ModContent.ProjectileType<TwilightShardsProjectileShoot>(), 71, 0.4f, Projectile.owner);

                Projectile.Kill();
            }
        }
        
        /*
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(SoundID.Shatter with { Volume = 0.4f }, Projectile.Center);

            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.Glass, 10, speedFromCenter: 6, amount: Main.rand.Next(10, 24)).ForEach(dust => dust.alpha = Main.rand.Next(0, 220));
        }*/
    }

    public class TwilightShardsProjectileShoot : ModProjectile
    {
        public override string Texture => base.Texture.Replace("TwilightShardsProjectileShoot", "TwilightShardsProjectile");

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.penetrate = 7;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 190;

            Projectile.aiStyle = -1;

            Projectile.localNPCHitCooldown = 10;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            rotMult = Math.Sign(Projectile.velocity.X);
        }

        ref float rotMult => ref Projectile.ai[0];
        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;
            Projectile.rotation += 0.2f * rotMult;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0) return true;

            rotMult *= 0.7f;

            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.velocity *= 0.6f;
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            
            return false;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Shatter with { Volume = 0.4f }, Projectile.Center);

            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.Glass, 10, speedFromCenter: 6, amount: Main.rand.Next(10, 24)).ForEach(dust => dust.alpha = Main.rand.Next(0, 220));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileInHBCenter(lightColor, centerOrigin: true);
            return false;
        }
    }
}
