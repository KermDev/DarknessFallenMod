//TODO:
//Description
//Sellprice
//Rarity
//Bloom
//Fix spritesheet
//Hitdirection
//Grass breaking
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
using IL.Terraria.GameContent;
using On.Terraria.GameContent;
using System.Linq;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;

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
			Item.damage = 8;
			Item.mana = 10;
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
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
        }

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
                Item.useTime = Item.useAnimation = 5;
			}
			else
			{
                Item.useTime = Item.useAnimation = 35;
            }
			return base.CanUseItem(player);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse == 2)
			{
                damage *= 2;
				type = ModContent.ProjectileType<MycanCandleSpear>();
				velocity.Normalize();
			}
			else
			{
                type = ModContent.ProjectileType<MycanCandleHeld>();
            }
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse != 2)
			{
				int tries = 0;
				for (int i = 0; i < 2; i++)
				{
					Vector2 pos = player.Center + Main.rand.NextVector2Circular(65, 65);
					Vector2 vel = pos.DirectionTo(Main.MouseWorld) * 10;

					Tile tile = Framing.GetTileSafely((int)(pos.X / 16), (int)(pos.Y / 16));

                    if (tile.HasTile && Main.tileSolid[tile.TileType] && tries++ < 40)
					{
						i--;
						continue;
					}
                    Projectile.NewProjectile(source, pos, Vector2.Zero, ModContent.ProjectileType<MycanCandleRing>(), 0, 0, player.whoAmI, Main.rand.Next(15,25), vel.ToRotation());
					Projectile.NewProjectile(source, pos, vel.RotatedByRandom(0.2f), ModContent.ProjectileType<MycanCandleProj>(), damage, knockback, player.whoAmI);
				}
				return true;
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
	}

	class MycanCandleProj : ModProjectile
	{
		Vector2 oldPos = Vector2.Zero;

        bool stuck = false;

        NPC stuckTarget = default;

        Vector2 offset = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mycan Candle");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 150;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
			Projectile.hide = true;
        }

		public override void AI()
		{
            if (stuck)
            {
                if (!stuckTarget.active)
                {
                    Projectile.active = false;
                    return;
                }
                Projectile.Center = stuckTarget.Center + offset;
            }

			if (oldPos == Vector2.Zero)
				oldPos = Projectile.Center;

			for (int i = 0; i < 5; i++)
			{
				Vector2 pos = Vector2.Lerp(oldPos, Projectile.Center, (float)(i / 5f));
				Dust.NewDustPerfect(pos, ModContent.DustType<MycanCandleFlameDust>(), Main.rand.NextVector2Circular(0.5f, 0.5f), 0, Color.Lerp(Color.Magenta, Color.Purple, Main.rand.NextFloat()), Main.rand.NextFloat(0.2f, 0.5f));
			}
            oldPos = Projectile.Center;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.penetrate++;
            stuck = true;
            stuckTarget = target;
            offset = Projectile.Center - target.Center;
        }
    }

    public class MycanCandleFlameDustBig : MycanCandleFlameDust
    {
        public override bool Update(Dust dust)
        {
            if (dust.customData is null)
            {
                dust.position -= Vector2.One * 32 * dust.scale;
                dust.customData = true;
            }

            Vector2 currentCenter = dust.position + Vector2.One.RotatedBy(dust.rotation) * 32 * dust.scale;

            dust.scale *= 0.92f;
            Vector2 nextCenter = dust.position + Vector2.One.RotatedBy(dust.rotation + 0.06f) * 32 * dust.scale;

            dust.rotation += 0.06f;
            dust.position += currentCenter - nextCenter;

            dust.shader.UseColor(dust.color);


            dust.position += dust.velocity;

            if (!dust.noGravity)
                dust.velocity.Y += 0.1f;

            dust.velocity *= 0.95f;

            if (!dust.noLight)
                Lighting.AddLight(dust.position, dust.color.ToVector3() * 0.5f);

            if (dust.scale < 0.05f)
                dust.active = false;

            return false;
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

    public class MycanCandleRing : ModProjectile
    { 
        private List<NPC> alreadyHit = new List<NPC>();
        private List<Vector2> cache;

        private Trail trail;
        private Trail trail2;

        public int timeLeftStart = 40;

        public float skew = 0.4f;
        private float Progress => 1 - (Projectile.timeLeft / (float)timeLeftStart);

        private float Radius => Projectile.ai[0] * (float)Math.Sqrt(Math.Sqrt(Progress));

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
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
                Vector2 offset = new Vector2((float)Math.Sin(rad) * skew, (float)Math.Cos(rad));
                offset *= radius;
                offset = offset.RotatedBy(Projectile.ai[1]);
                cache.Add(Projectile.Center + offset);
            }

            while (cache.Count > 33)
            {
                cache.RemoveAt(0);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 line = targetHitbox.Center.ToVector2() - Projectile.Center;
            line.Normalize();
            line *= Radius;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + line))
            {
                return true;
            }
            return false;
        }

        private void ManageTrail()
        {

            trail = trail ?? new Trail(Main.instance.GraphicsDevice, 33, new TriangularTip(1), factor => 28 * (1 - Progress), factor =>
            {
                return Color.Magenta;
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

        public override bool? CanHitNPC(NPC target)
        {
            if (alreadyHit.Contains(target))
                return false;
            return base.CanHitNPC(target);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.penetrate++;
            alreadyHit.Add(target);
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
                Dust.NewDustPerfect(pos + Main.rand.NextVector2Circular(10, 10), ModContent.DustType<MycanCandleFlameDust>(), Main.rand.NextVector2Circular(0.5f, 0.5f), 0, Color.Magenta, Main.rand.NextFloat(0.3f, 0.6f));

            var flame = Main.projectile.Where(n => n.active && n.type == ModContent.ProjectileType<MycanCandleProj>() && Collision.CheckAABBvLineCollision(n.position, n.Size, Projectile.Center, Projectile.Center + (Projectile.rotation.ToRotationVector2() * 85))).OrderBy(n => n.Distance(Projectile.Center)).FirstOrDefault();

            if (flame != default)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), flame.Center, Vector2.Zero, ModContent.ProjectileType<MycanCandleRing>(), Projectile.damage, 0, Player.whoAmI, Main.rand.Next(70, 90), 0);
                (proj.ModProjectile as MycanCandleRing).skew = 1;
                (proj.ModProjectile as MycanCandleRing).timeLeftStart = 20;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, flame.Center);
                for (int i = 0; i < 12; i++)
                {
                    Dust.NewDustPerfect(flame.Center, ModContent.DustType<MycanCandleFlameDustBig>(), Main.rand.NextVector2Circular(12, 12), 0, Color.Magenta, Main.rand.NextFloat(0.6f, 1.8f));
                }
                Player.GetModPlayer<DarknessFallenPlayer>().ShakeScreen(8, 0.82f);
                proj.timeLeft = 20;
                flame.active = false;
            }
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

            for (int i = 0; i < oldPosition.Count; i++)
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

    class MycanCandleHeld : ModProjectile
    {
        int direction = 0;

        float white = 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mycan Candle");
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.timeLeft = 50;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (white > 0)
                white -= 0.05f;
            else
                white = 0;
            if (Projectile.velocity != Vector2.Zero)
            {
                direction = Math.Sign(Projectile.velocity.X);
                Projectile.velocity = Vector2.Zero;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 5 == 4)
                Projectile.frame++;
            Projectile.frame %= Main.projFrames[Projectile.type];

            var Player = Main.player[Projectile.owner];

            var center = Player.Center + Vector2.UnitY * Player.gfxOffY + (Vector2.UnitX * direction * 15);

            Player.itemTime = Player.itemAnimation = 5;
            Player.heldProj = Projectile.whoAmI;
            Player.ChangeDir(direction);

            Projectile.rotation = 0;
            Projectile.Center = center;

            Player.itemRotation = 0;

            Lighting.AddLight(center, Color.Magenta.ToVector3() * 0.5f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player owner = Main.player[Projectile.owner];
            SpriteEffects effects = owner.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D bloomTex = ModContent.Request<Texture2D>(Texture + "_Bloom").Value;
            Texture2D glowTex = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            Texture2D whiteTex = ModContent.Request<Texture2D>(Texture + "_White").Value;

            int frameHeight = tex.Height / Main.projFrames[Projectile.type];
            Rectangle frameBox = new Rectangle(0, frameHeight * Projectile.frame, tex.Width, frameHeight);

            Vector2 origin = frameBox.Size() * new Vector2(0.5f, 0.8f);

            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, frameBox, lightColor, Projectile.rotation, origin, Projectile.scale, effects, 0f);
            Main.spriteBatch.Draw(glowTex, Projectile.Center - Main.screenPosition, frameBox, Color.White, Projectile.rotation, origin, Projectile.scale, effects, 0f);
            Main.spriteBatch.Draw(whiteTex, Projectile.Center - Main.screenPosition, frameBox, Color.White * white, Projectile.rotation, origin, Projectile.scale, effects, 0f);


            Color glowColor = Color.Magenta;
            glowColor.A = 0;
            glowColor *= 0.25f;
            Main.spriteBatch.Draw(bloomTex, Projectile.Center - Main.screenPosition, frameBox, glowColor, Projectile.rotation, origin, Projectile.scale, effects, 0f);
            return false;
        }
    }
}