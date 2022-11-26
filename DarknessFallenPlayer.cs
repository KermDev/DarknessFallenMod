using DarknessFallenMod.Items;
using DarknessFallenMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Generation;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace DarknessFallenMod
{
	public class DarknessFallenPlayer : ModPlayer
	{
        public bool HasRodGambled = false;
        public bool IsRodGambling = false;
        public int GambleRodBuff = -1;

		public override void clientClone(ModPlayer clientClone)
		{
			DarknessFallenPlayer clone = clientClone as DarknessFallenPlayer;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)Player.whoAmI);
			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			DarknessFallenPlayer clone = clientPlayer as DarknessFallenPlayer;
		}

        public override void ModifyCaughtFish(Item fish)
        {
            if(IsRodGambling)
            {
                switch(new Random().Next(0, 5))
                {
                    default:
                        {
                            Main.NewText("You have been blessed with speed!");
                            GambleRodBuff = BuffID.Swiftness;
                            break;
                        }
                    case 1:
                        {
                            Main.NewText("You have been blessed with endurance!");
                            GambleRodBuff = BuffID.Endurance;
                            break;
                        }
                    case 2:
                        {
                            Main.NewText("You have been cursed with slowness!");
                            GambleRodBuff = BuffID.Slow;
                            break;
                        }
                    case 3:
                        {
                            Main.NewText("You have been blessed with mana regeneration!");
                            GambleRodBuff = BuffID.ManaRegeneration;
                            break;
                        }
                    case 4:
                        {
                            Main.NewText("You have been cursed with blindness!");
                            GambleRodBuff = BuffID.Blackout;
                            break;
                        }
                }
                this.HasRodGambled = true;
            }
        }

        public override void PreUpdateBuffs()
        {
            if (GambleRodBuff != -1)
            {
                this.Player.AddBuff(GambleRodBuff, 2);
            }
        }

        float screenShakeStrenght;
        float screenShakeDesolve;
        public void ShakeScreen(float strenght, float desolve = 0.95f)
        {
            screenShakeStrenght = strenght;
            screenShakeDesolve = Math.Clamp(desolve, 0, 0.9999f);
        }

        public override void ModifyScreenPosition()
        {
            if (screenShakeStrenght > 0.001f)
            {
                Main.screenPosition += screenShakeStrenght * Main.rand.NextVector2Unit();
                screenShakeStrenght *= screenShakeDesolve;
            }
        }

        
    }
}
