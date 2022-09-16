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
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace DarknessFallenMod.NPCs.Bosses
{
    [AutoloadBossHead]
    public class PrinceSlime : ModNPC
    {
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

		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
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
			NPC.knockBackResist = 0f;
			NPC.value = Item.buyPrice(gold: 5);
			NPC.SpawnWithHigherTime(30);
			NPC.boss = true;
			NPC.npcSlots = 20;
			NPC.aiStyle = NPCAIStyleID.KingSlime;
			
			NPC.BossBar = ModContent.GetInstance<PrinceSlimeBossBar>();
		}

		const int animationSpeed = 10;
        public override void FindFrame(int frameHeight)
        {
			NPC.frameCounter++;

			int maxFrame = Main.npcFrameCount[Type] - 1;
			
            if (NPC.frameCounter > animationSpeed)
            {
				NPC.frameCounter = 0;

				NPC.frame.Y += frameHeight;
            }
            
			if (NPC.velocity.Y != 0)
            {
				NPC.frame.Y = frameHeight * 2;
            }

			if (NPC.frame.Y > maxFrame * frameHeight) NPC.frame.Y = 0;
        }

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			cooldownSlot = ImmunityCooldownID.Bosses;
			
			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
        {
			if (Main.netMode == NetmodeID.Server) return;
			

			Main.NewText(Systems.DownedBossSystem.downedPrinceSlime);

            if (NPC.life <= 0)
            {
				int crown = Mod.Find<ModGore>("PrinceSlime_CrownGore").Type;
				Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vector2.UnitY * -3 + Vector2.UnitX * (Main.rand.NextBool() ? -1 : 1) * 1.5f, crown);
			}
        }

        public override void OnKill()
        {
			NPC.SetEventFlagCleared(ref Systems.DownedBossSystem.downedPrinceSlime, -1);
		}

        public override void OnSpawn(IEntitySource source)
        {
			ChatHelper.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("His slimy excellency has arrived"), Color.Green);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			if (Main.slimeRain && !NPC.AnyNPCs(Type) && !NPC.AnyNPCs(NPCID.KingSlime))
            {
				return 0.5f;
            }
			return 0;
        }
    }

	public class PrinceSlimeBossBar : ModBossBar
    {
		public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame)
		{
			return ModContent.Request<Texture2D>("DarknessFallenMod/NPCs/Bosses/PrinceSlime_Head_Boss");
		}
	}

    /*public class KingSlimeILEdit : ILoadable
    {
        
		
    }*/
}
