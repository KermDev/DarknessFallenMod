using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MagicWeapons
{
    public class SlimyRain : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Staff Of Slimy Rain");
            Tooltip.SetDefault("[c/222222:AnUncreativeName named this]");
        }

        public override void SetDefaults()
        {
			Item.damage = 17;
			Item.mana = 8;
			Item.DamageType = DamageClass.Magic;
			Item.width = 43;
			Item.height = 43;
			Item.useTime = 4;
			Item.useAnimation = 4;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.knockBack = 0;
			Item.value = 18764;
			Item.rare = 3;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SlimyRainProjectile>();
			Item.shootSpeed = 5.5f;
			Item.noMelee = true;
		}

        /*
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 pos = player.Center - Vector2.UnitY * Main.screenHeight;
			Projectile.NewProjectileDirect(source, pos, pos.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.PiOver4 * 0.1f) * Item.shootSpeed, type, damage, 0, player.whoAmI).netUpdate = true;

            return false;
        }
        */

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 pos = player.Center - Vector2.UnitY * Main.screenHeight;

            position = pos;
            velocity = pos.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.PiOver4 * 0.1f) * Item.shootSpeed;
        }

        public override void UseItemFrame(Player player)
        {
            player.itemLocation.Y -= 10;
            player.itemLocation.X -= 10 * player.direction;
        }
    }

	public class SlimyRainProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 650;

            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
        }

        Color color;
        public override void OnSpawn(IEntitySource source)
        {
            color = Main.rand.NextFromList(new Color[] { Color.Red, Color.Green, Color.Blue });
            color.A = 100;
        }

        public override void AI()
        {
            Projectile.frameCounter++;

            if (Projectile.frameCounter >= 25)
            {
                Projectile.frameCounter = 0;

                Projectile.frame++;

                if (Projectile.frame > 3)
                {
                    Projectile.frame = 0;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            
            if (Main.netMode != NetmodeID.MultiplayerClient && Main.rand.NextBool(8)) 
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TintableDustLighted, Scale: 0.2f, newColor: color * 0.3f);

            float light = 0.003f;
            if (!Main.dedServ) Lighting.AddLight(Projectile.Center, color.R * light, color.G * light, color.B * light);
        }

        /*
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(color.R);
            writer.Write(color.G);
            writer.Write(color.B);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            color.R = reader.ReadByte();
            color.B = reader.ReadByte();
            color.G = reader.ReadByte();
        }
        */

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.BeginWithShaderOptions();

            DarknessFallenUtils.DrawProjectileInHBCenter(Projectile, color, true);

            Main.spriteBatch.End();
            Main.spriteBatch.BeginWithDefaultOptions();

            return false;
        }
    }
}
