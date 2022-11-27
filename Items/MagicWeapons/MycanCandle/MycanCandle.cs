//TODO:
//Description
//Sellprice
//Rarity
//Bloom
//Balance
//Fix spritesheet
//Hitdirection
//Grass breaking
//Add combo logic
//Improve m1 visuals
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DarknessFallenMod.Items.Materials;
using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework.Graphics;
using System;
using DarknessFallenMod.Core;
using static Humanizer.In;
using System.Collections.Generic;
using DarknessFallen.Core;
using DarknessFallenMod.Helpers;
using Terraria.Graphics.Effects;

namespace DarknessFallenMod.Items.MagicWeapons.MycanCandle
{
    public class MycanCandle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mycan Candle");
			Tooltip.SetDefault("update this later");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 5));
        }

		public override void SetDefaults()
		{
			Item.damage = 17;
			Item.mana = 5;
			Item.DamageType = DamageClass.Magic;
			Item.width = 30;
			Item.height = 46;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.knockBack = 3;
			Item.value = 18764;
			Item.rare = 3;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MycanCandleSpear>();
			Item.shootSpeed = 13f;
			Item.noMelee = true;
        }

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noUseGraphic = true;
			}
			else
			{
				Item.useStyle = ItemUseStyleID.HoldUp;
                Item.noUseGraphic = false;
            }
			return base.CanUseItem(player);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				type = ModContent.ProjectileType<MycanCandleSpear>();
				velocity.Normalize();
			}
			else
			{
				velocity = Vector2.Zero;
                type = ModContent.ProjectileType<MycanCandleProj>();
            }
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse != 2)
			{
				int tries = 0;
				for (int i = 0; i < 3; i++)
				{
					Vector2 pos = player.Center + Main.rand.NextVector2Circular(45, 45);
					Vector2 vel = pos.DirectionTo(Main.MouseWorld) * 10;

					Tile tile = Framing.GetTileSafely((int)(pos.X / 16), (int)(pos.Y / 16));

                    if (tile.HasTile && Main.tileSolid[tile.TileType] && tries++ < 40)
					{
						i--;
						continue;
					}
                    Projectile.NewProjectile(source, pos, Vector2.Zero, ModContent.ProjectileType<MycanCandlePortal>(), 0, 0, player.whoAmI, Main.rand.Next(15,25), vel.ToRotation());
					Projectile.NewProjectile(source, pos, vel, type, damage, knockback, player.whoAmI);
				}
				return false;
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
	}

	class MycanCandleProj : ModProjectile
	{
		Vector2 oldPos = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mycan Candle");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
			Projectile.hide = true;
        }

		public override void AI()
		{
			if (oldPos == Vector2.Zero)
				oldPos = Projectile.Center;

			for (int i = 0; i < 5; i++)
			{
				Vector2 pos = Vector2.Lerp(oldPos, Projectile.Center, (float)(i / 5f));
				Dust.NewDustPerfect(pos, ModContent.DustType<MycanCandleFlameDust>(), Main.rand.NextVector2Circular(0.5f, 0.5f), 0, Color.Lerp(Color.Magenta, Color.Purple, Main.rand.NextFloat()), Main.rand.NextFloat(0.2f, 0.5f));
			}
            oldPos = Projectile.Center;
        }
	}

    public class MycanCandleFlameDust : ModDust
	{ 
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, 0, 64, 64);

            dust.shader = new Terraria.Graphics.Shaders.ArmorShaderData(new Ref<Effect>(ModContent.Request<Effect>("DarknessFallenMod/Effects/GlowingDust", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value), "GlowingDustPass");
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return dust.color;
        }

        public override bool Update(Dust dust)
        {
            if (dust.customData is null)
            {
                dust.position -= Vector2.One * 32 * dust.scale;
                dust.customData = true;
            }

            Vector2 currentCenter = dust.position + Vector2.One.RotatedBy(dust.rotation) * 32 * dust.scale;

            dust.scale *= 0.85f;
            Vector2 nextCenter = dust.position + Vector2.One.RotatedBy(dust.rotation + 0.06f) * 32 * dust.scale;

            dust.rotation += 0.06f;
            dust.position += currentCenter - nextCenter;

            dust.shader.UseColor(dust.color);

			dust.color *= 0.95f;

            dust.position += dust.velocity;

            if (!dust.noGravity)
                dust.velocity.Y += 0.1f;

            dust.velocity *= 0.97f;

            if (!dust.noLight)
                Lighting.AddLight(dust.position, dust.color.ToVector3() * 0.5f);

            if (dust.scale < 0.05f)
                dust.active = false;

            return false;
        }
    }

    public class MycanCandlePortal : ModProjectile
    { 
        private List<Vector2> cache;

        private Trail trail;
        private Trail trail2;

        public int timeLeftStart = 40;
        private float Progress => 1 - (Projectile.timeLeft / (float)timeLeftStart);

        private float Radius => Projectile.ai[0] * (float)Math.Sqrt(Math.Sqrt(Progress));

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = timeLeftStart;
            Projectile.extraUpdates = 1;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mycan Portal");
        }

        public override void AI()
        {
            Projectile.velocity *= 0.95f;
            if (Main.netMode != NetmodeID.Server)
            {
                ManageCaches();
                ManageTrail();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawPrimitives();
            return false;
        }

        private void ManageCaches()
        {
            cache = new List<Vector2>();
            float radius = Radius;
            for (int i = 0; i < 33; i++) //TODO: Cache offsets, to improve performance
            {
                double rad = (i / 32f) * 6.28f;
                Vector2 offset = new Vector2((float)Math.Sin(rad) * 0.4f, (float)Math.Cos(rad));
                offset *= radius;
                offset = offset.RotatedBy(Projectile.ai[1]);
                cache.Add(Projectile.Center + offset);
            }

            while (cache.Count > 33)
            {
                cache.RemoveAt(0);
            }
        }

        private void ManageTrail()
        {

            trail = trail ?? new Trail(Main.instance.GraphicsDevice, 33, new TriangularTip(1), factor => 28 * (1 - Progress), factor =>
            {
                return Color.MediumPurple;
            });

            trail2 = trail2 ?? new Trail(Main.instance.GraphicsDevice, 33, new TriangularTip(1), factor => 10 * (1 - Progress), factor =>
            {
                return Color.White;
            });
            float nextplace = 33f / 32f;
            Vector2 offset = new Vector2((float)Math.Sin(nextplace), (float)Math.Cos(nextplace));
            offset *= Radius;

            trail.Positions = cache.ToArray();
            trail.NextPosition = Projectile.Center + offset;

            trail2.Positions = cache.ToArray();
            trail2.NextPosition = Projectile.Center + offset;
        }

        public void DrawPrimitives()
        {
            Effect effect = Filters.Scene["RingTrail"].GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("StarlightRiver/Assets/GlowTrail").Value);
            effect.Parameters["alpha"].SetValue(1);

            trail?.Render(effect);
            trail2?.Render(effect);
        }
    }

    class MycanCandleSpear : ModProjectile
    {
		int afterImageLength = 10;

		List<float> oldRotation = new List<float>();

		List<Vector2> oldPosition = new List<Vector2>();

		Vector2 initialDirection = Vector2.Zero;

		float initialRotation => initialDirection.ToRotation();

		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Mycan Candle");
			Main.projFrames[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.timeLeft = 30;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = true;
			Projectile.tileCollide = false;
        }

        public override void AI()
        {
			if (Projectile.velocity != Vector2.Zero)
			{
				initialDirection = Projectile.velocity;
				Projectile.velocity = Vector2.Zero;
			}
			Projectile.frameCounter++;
			if (Projectile.frameCounter % 5 == 4)
				Projectile.frame++;
			Projectile.frame %= Main.projFrames[Projectile.type];

            var Player = Main.player[Projectile.owner];

            var center = Player.Center + Vector2.UnitY * Player.gfxOffY + initialDirection * 15;

			Player.itemTime = Player.itemAnimation = 5;
            Player.heldProj = Projectile.whoAmI;
            Player.ChangeDir(Math.Sign(initialDirection.X));

            float progress = 1 - (Projectile.timeLeft / 30f);
			progress = EaseFunction.EaseCubicInOut.Ease(progress);
			Projectile.rotation = initialRotation + MathHelper.Lerp(0.15f * Player.direction, -0.15f * Player.direction, EaseFunction.EaseQuadInOut.Ease(progress));
			Projectile.Center = center + (initialDirection * 45 * (float)(Math.Sin(progress * 3.14f) - 0.95f));

            Player.itemRotation = MathHelper.WrapAngle(initialRotation - ((Player.direction == 1) ? 0 : MathHelper.Pi));

			oldRotation.Add(Projectile.rotation);
			oldPosition.Add(Projectile.Center);

			if (oldRotation.Count > afterImageLength)
			{
				oldRotation.RemoveAt(0);
				oldPosition.RemoveAt(0);
            }

            Vector2 pos = Projectile.Center + (Projectile.rotation.ToRotationVector2() * 65);
            Lighting.AddLight(pos, Color.Magenta.ToVector3());

            if (Main.rand.NextBool(6))
                Dust.NewDustPerfect(pos + Main.rand.NextVector2Circular(10,10), ModContent.DustType<MycanCandleFlameDust>(), Main.rand.NextVector2Circular(0.5f, 0.5f), 0, Color.Magenta, Main.rand.NextFloat(0.3f, 0.6f));
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + (Projectile.rotation.ToRotationVector2() * 65), 15, ref collisionPoint);
		}

		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D glowTex = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            Texture2D fireTex = ModContent.Request<Texture2D>(Texture + "_Fire").Value;

            int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frameBox = new Rectangle(0, frameHeight * Projectile.frame, tex.Width, frameHeight);

			Vector2 origin = frameBox.Size() * new Vector2(0.5f, 0.9f);

            for(int i = 0; i < oldPosition.Count; i++)
			{
                float opacity = i / (float)afterImageLength;
				float scale = MathHelper.Lerp(0.5f, 1, opacity);
                Main.spriteBatch.Draw(tex, oldPosition[i] - Main.screenPosition, frameBox, lightColor * opacity * 0.4f, oldRotation[i] + 1.57f, origin, Projectile.scale * scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(fireTex, oldPosition[i] - Main.screenPosition, frameBox, Color.White * opacity * 0.4f, oldRotation[i] + 1.57f, origin, Projectile.scale * scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, frameBox, lightColor, Projectile.rotation + 1.57f, origin, Projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(fireTex, Projectile.Center - Main.screenPosition, frameBox, Color.White, Projectile.rotation + 1.57f, origin, Projectile.scale, SpriteEffects.None, 0f);


            Color glowColor = Color.Magenta;
			glowColor.A = 0;
			glowColor *= 0.25f;
            Main.spriteBatch.Draw(glowTex, Projectile.Center - Main.screenPosition, frameBox, glowColor, Projectile.rotation + 1.57f, origin, Projectile.scale * 1.05f, SpriteEffects.None, 0f);
            return false;
        }
    }
}