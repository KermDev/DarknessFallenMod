using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace DarknessFallenMod.NPCs.Bosses
{
    [AutoloadBossHead]
    public class PrinceSlime : ModNPC
    {
        #region KING SLIME SPAWN PREVENTION IL EDIT, DONT TOUCH IT WILL BREAK
        public override void Load()
        {
			IL.Terraria.NPC.DoDeathEvents_AdvanceSlimeRain += ILPreventCountAdvance;
		}

        void ILPreventCountAdvance(ILContext il)
		{
			var c = new ILCursor(il);

			if (!c.TryGotoNext(i => i.MatchAdd())) return;

			c.EmitDelegate<Func<int, int>>((x) =>
			{
				if ((NPC.AnyNPCs(ModContent.NPCType<PrinceSlime>()) || !Systems.DownedBossSystem.downedPrinceSlime) && Main.slimeRainKillCount + x == (NPC.downedSlimeKing ? 75 : 150))
                {
					return 0;
                }
				return 1;
			});
		}
        #endregion

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 8;

			NPCID.Sets.BossBestiaryPriority.Add(Type);
		}

        public override void SetDefaults()
        {
			NPC.width = 95;
			NPC.height = 95;
			NPC.damage = 25;
			NPC.defense = 7;
			NPC.lifeMax = 1000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = -200f;
			NPC.value = Item.buyPrice(gold: 4, silver: 46, copper: 12);
			NPC.SpawnWithHigherTime(30);
			NPC.boss = true;
			NPC.npcSlots = 20;
			NPC.aiStyle = -1;
			NPC.netAlways = true;
			NPC.stairFall = true;

			BannerItem = ModContent.ItemType<Items.Placeable.Banners.PrinceSlimeBanner>();
			Banner = Type;

			NPC.BossBar = ModContent.GetInstance<PrinceSlimeBossBar>();
		}

		// !!! DISCLAIMER !!!
		// I know this AI is very messy, if you wanna rewrite this feel free ;)

		public int Phase = 1;

		float laserTimer = 61;
		ref float AiTimer => ref NPC.ai[0];
		Vector2 LaserPos => NPC.Center + new Vector2(NPC.direction * -3, -11);
		bool fell;

		int secondPhaseAnimTimer;
		const int timeMaxAnim2 = 180;
		public override void AI()
		{
			NPC.netUpdate = true;

			if (NPC.collideY) NPC.velocity.X *= 0.92f;

			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}

			if (!fell)
			{
				if (NPC.velocity.Y == 0)
				{
					for (int i = 0; i < 25; i++)
					{
						Dust.NewDust(NPC.Hitbox.BottomLeft(), NPC.width, 2, DustID.TintableDust, SpeedY: -5, SpeedX: Main.rand.Next(-5, 5), Scale: Main.rand.NextFloat(1f, 2.5f), newColor: new Color(0, 255, 80), Alpha: 170);
					}

					foreach (Player player in Main.player)
					{
						if (player.DistanceSQ(NPC.Center) < 5760000)
						{
							player.GetModPlayer<DarknessFallenPlayer>().ShakeScreen(7, 0.94f);
						}
					}

					SoundEngine.PlaySound(SoundID.Item167, NPC.Center);

					fell = true;
				}

				return;
			}

			if (secondPhaseAnimTimer < timeMaxAnim2 && NPC.life <= 0.5f * NPC.lifeMax)
			{
				secondPhaseAnimTimer++;

				foreach (Vector2 pos in LaserPos.GetCircularPositions(MathF.Pow((timeMaxAnim2 - secondPhaseAnimTimer) * 0.7f, 1.6f) * 0.7f, 8, secondPhaseAnimTimer * 0.03f))
				{
					// 21, 75, 304, 301
					Dust.NewDust(pos, 0, 0, DustID.TreasureSparkle, newColor: Color.Red, Scale: 0.6f);
					Dust.NewDust(pos, 0, 0, DustID.PortalBolt, newColor: Color.Red, Scale: 0.7f);
				}

				if (secondPhaseAnimTimer >= timeMaxAnim2) 
				{
					SoundEngine.PlaySound(SoundID.Tink, NPC.Center);

					DarknessFallenUtils.NewDustCircular(LaserPos, DustID.TreasureSparkle, 1, speedFromCenter: 12);
					DarknessFallenUtils.NewDustCircular(LaserPos, DustID.Smoke, 1, speedFromCenter: 7, amount: 9);

					Phase = 2;
				}

				return;
			}

			if (Main.netMode != NetmodeID.MultiplayerClient)
            {
				Player target = Main.player[NPC.target];

				Vector2 dirToPlayer = NPC.Center.DirectionTo(target.Center);
				int xdir = MathF.Sign(dirToPlayer.X);

				if (NPC.wet)
				{
					NPC.velocity.X += xdir * 0.3f;
					NPC.velocity.Y -= 0.3f;

					NPC.velocity.X = Math.Clamp(NPC.velocity.X, -1.5f, 1.5f);
					return;
				}

				if (NPC.velocity.Y == 0 && AiTimer >= 90 && laserTimer > 60)
				{
					AiTimer = 0;

					NPC.velocity.Y += Main.rand.Next(-10, -5);
					NPC.velocity.X += Math.Clamp(NPC.DistanceSQ(target.Center) * 0.0003f, 0.1f, 4f) * xdir;
					
					if (NPC.Center.Y - target.Center.Y < 100)
                    {
						NPC.stairFall = true;
                    }
                    else
                    {
						NPC.velocity.Y += Main.rand.Next(-10, -5);
						NPC.velocity.X += Math.Clamp(NPC.DistanceSQ(target.Center) * 0.0003f, 0.1f, 4f) * xdir;
					}
					
				}

				if (NPC.collideX && NPC.velocity.Y != 0) NPC.velocity.X += xdir * 2;

				if (Main.rand.NextBool(360)) NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)target.Center.X + Main.rand.Next(-300, 300), (int)target.Center.Y - 1200, ModContent.NPCType<PrinceSlimeMinion>()).netUpdate = true;

				if (NPC.velocity.Y == 0) AiTimer++;

				if (Phase == 2)
				{
					if (laserTimer <= 0)
					{
						laserTimer = Main.rand.Next(90, 200);

						Vector2 dir = LaserPos.DirectionTo(target.Center);

						float speed = 7;

						Lighting.AddLight(LaserPos, TorchID.Red);

						int proj = Projectile.NewProjectile(
							NPC.GetSource_FromAI(),
							LaserPos + dir * 45,
							dir.RotatedByRandom(MathHelper.PiOver4 * 0.1f) * speed,
							ProjectileID.Fireball,
							20,
							1
							);

						Main.projectile[proj].hostile = true;
					}
					else if (laserTimer < 60)
					{
						foreach (Vector2 pos in LaserPos.GetCircularPositions(laserTimer * laserTimer, 8, laserTimer * 0.01f))
						{
							// 21, 75, 304, 301
							Dust.NewDust(pos, 0, 0, DustID.TreasureSparkle, newColor: Color.Red, Scale: 0.5f);
						}

					}

					laserTimer--;
				}
			}
		}
		
        const int animationSpeed = 10;
        public override void FindFrame(int frameHeight)
        {
			NPC.frameCounter++;

			int maxFrame = Main.npcFrameCount[Type] - 1;
			
            if (NPC.frameCounter > animationSpeed)
            {
				NPC.frameCounter = 0;

				NPC.frame.Y += 108;
            }
            
			if (NPC.velocity.Y != 0)
            {
				NPC.frame.Y = (Phase == 1 ? 2 : 6) * 109;
            }

			if (NPC.frame.Y > (Phase == 1 ? 3 : maxFrame) * 108) NPC.frame.Y = (Phase == 1 ? 0 : 4) * 108;
        }

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			cooldownSlot = ImmunityCooldownID.Bosses;
			
			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
        {
			if (Main.netMode == NetmodeID.Server) return;

            if (NPC.life <= 0)
            {
				int crown = Mod.Find<ModGore>("PrinceSlime_CrownGore").Type;
				Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vector2.UnitY * -3 + Vector2.UnitX * (Main.rand.NextBool() ? -1 : 1) * 1.5f, crown);
			}
        }

        public override void OnKill()
        {
			NPC.SetEventFlagCleared(ref Systems.DownedBossSystem.downedPrinceSlime, -1);

			NPC.DropCustomBannerKillCount(50, ModContent.ItemType<Items.Placeable.Banners.PrinceSlimeBanner>());
		}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.MeleeWeapons.Slimescaliber>(), 2));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.SummonWeapons.CultSlime>(), 4));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.BottleOSlime>(), 3));

			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.MagicWeapons.SlimyRain>(), 100));

			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.SlimeGuardHelmet>(), 3));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.SlimeGuardChestplate>(), 3));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.SlimeGuardLeggings>(), 3));

			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.PrinceSlimeTrophy>(), 10));
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.Furniture.PrinceSlimeRelic>()));

			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Consumables.PrinceSlimeBossBag>()));

			npcLoot.Add(ItemDropRule.Common(ItemID.Gel, minimumDropped: 2, maximumDropped: 12));
		}

        public override void OnSpawn(IEntitySource source)
        {
			PrinceSlimeOnePerSlimeRain.PrinceSlimeSpawned = true;
			ChatHelper.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("His slimy excellency has arrived"), Color.Green);

			NPC.TargetClosest();
			NPC.Center = Main.player[NPC.target].Center;
			NPC.position.Y -= 1200;
			NPC.velocity.Y = 12;
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			if (Main.slimeRain && !PrinceSlimeOnePerSlimeRain.PrinceSlimeSpawned && !NPC.AnyNPCs(NPC.type) && !NPC.AnyNPCs(NPCID.KingSlime))
            {
				return 0.3f;
            }
			return 0;
        }

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.SlimeRain,
				new FlavorTextBestiaryInfoElement("The prince of all that is slime, it has somehow acquired a crown with magic powers.")
			});
		}
	}

	public class PrinceSlimeBossBar : ModBossBar
    {
		public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame)
		{
			return ModContent.Request<Texture2D>("DarknessFallenMod/NPCs/Bosses/PrinceSlime_Head_Boss");
		}
	}

	public class PrinceSlimeOnePerSlimeRain : ModSystem
	{
		public static bool PrinceSlimeSpawned { get; set; } = false;
        public override void PostUpdateEverything()
        {
			if (!Main.slimeRain) PrinceSlimeSpawned = false;
        }
    }
}
