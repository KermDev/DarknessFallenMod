using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Pets
{
    public class BloodyTentacle : ModItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("squid");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<BloodyTentaclePet>();
			Item.width = 16;
			Item.height = 30;
			Item.UseSound = SoundID.Item2;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.Yellow;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 5, 50);
			Item.buffType = ModContent.BuffType<BloodyTentacleBuff>();
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Bone, 50);
			recipe.AddIngredient(ItemID.Candle, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600);
			}
		}
	}

	public class BloodyTentaclePet : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Squid");

			Main.projFrames[Projectile.type] = 7;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 31;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.timeLeft *= 5;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
		}

		
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (!player.active)
			{
				Projectile.active = false;
				return;
			}

			if (!player.dead && player.HasBuff(ModContent.BuffType<BloodyTentacleBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			if (!Main.dedServ)
			{
				Lighting.AddLight(Projectile.Center, 1f, 175f / 255f, 0f);
			}

			Vector2 flyToPos = player.Center + new Vector2(player.direction, 1) * -40;
			Projectile.Center = Vector2.Lerp(Projectile.Center, flyToPos, 0.2f);

			float rotTo = 0;
			if (Projectile.DistanceSQ(flyToPos) > 10)
            {
				rotTo = Projectile.DirectionTo(flyToPos).X * MathHelper.PiOver2 * 0.8f;
            }
			Projectile.rotation = MathHelper.Lerp(Projectile.rotation, rotTo, 0.2f);

			Projectile.spriteDirection = -player.direction;

			Projectile.BasicAnimation(5);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.DrawProjectileInHBCenter(Color.White, true, centerOrigin: true);
			return false;
		}
	}

	public class BloodyTentacleBuff : ModBuff
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Squid");
			Description.SetDefault("squid");

			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;

			int projType = ModContent.ProjectileType<BloodyTentaclePet>();
			
			if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] <= 0)
			{
				var entitySource = player.GetSource_Buff(buffIndex);

				Projectile.NewProjectile(entitySource, player.Center, Vector2.Zero, projType, 0, 0f, player.whoAmI);
			}
		}
	}
}
