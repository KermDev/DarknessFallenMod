using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using DarknessFallenMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace DarknessFallenMod.NPCs.TownNPCs
{
    [AutoloadHead]
    class SlimePrinceNPC : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Prince");
        }

        public override void SetDefaults()
        {
            NPC.height = 48;
            NPC.width = 28;
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.aiStyle = 7;
            NPC.defense = 10;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            Main.npcFrameCount[NPC.type] = 26;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 0;
            NPCID.Sets.AttackFrameCount[NPC.type] = 5;
            NPCID.Sets.DangerDetectRange[NPC.type] = 450;
            NPCID.Sets.AttackType[NPC.type] = 3;
            NPCID.Sets.AttackTime[NPC.type] = 12;
            NPCID.Sets.AttackAverageChance[NPC.type] = 20;
            //NPCID.Sets.HatOffsetY[NPC.type] = 0;
            AnimationType = NPCID.Guide;
        }

        public override void AI()
        {

        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            if(Systems.DownedBossSystem.downedPrinceSlime)
            {
                return true;
            }

            return false;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            { "Billy bobby jimmy jonny",
                "Got three balls into the goal last night",
                "hes doing pretty well this season",
                "...WHAAAAT"
            };
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Slime Shop";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if(firstButton)
            {
                shop = true;
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            #region banners
            shop.item[nextSlot].SetDefaults(ItemID.SlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.GreenSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.RedSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.PurpleSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.YellowSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.BlackSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.IceSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.SandSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.JungleSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.SpikedIceSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.SpikedJungleSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.MotherSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.LavaSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.PinkyBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.UmbrellaSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.ToxicSludgeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.CorruptSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.CrimslimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.GastropodBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.IlluminantSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.RainbowSlimeBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.HoppinJackBanner, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.Banners.PrinceSlimeMinionBanner>(), false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.Banners.NatureSlimeBanner>(), false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.Banners.DestructionSlimeBanner>(), false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.Banners.SporeSlimeBanner1>(), false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.Banners.SporeSlimeBanner2>(), false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.Banners.PrinceSlimeBanner>(), false);
            shop.item[nextSlot].shopCustomPrice = 200;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;
            #endregion

            shop.item[nextSlot].SetDefaults(ItemID.SlimeCrown, false);
            shop.item[nextSlot].shopCustomPrice = 300;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.SlimeGun, false);
            shop.item[nextSlot].shopCustomPrice = 50;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.BossSummons.SlimyBeaker>(), false);
            shop.item[nextSlot].shopCustomPrice = 350;
            shop.item[nextSlot].shopSpecialCurrency = DarknessFallenMod.GelCurrency;
            nextSlot++;

            /*
                Falling slime event summon - 350 gel
            */
        }

        public override string GetChat()
        {
            NPC.FindFirstNPC(ModContent.NPCType<SlimePrinceNPC>());
            switch(Main.rand.Next(6))
            {
                default:
                    {
                        return "Woooooooooooooo";
                    }
                case 0:
                    {
                        return "Whats white, sticky and slimy? A white slime!";
                    }
                case 1:
                    {
                        return "Know what this world needs more? Slime";
                    }
                case 2:
                    {
                        return "Gastropods aren't really slimes are they?";
                    }
                case 3:
                    {
                        return "Ok but what is a slime anyway.";
                    }
                case 4:
                    {
                        return "What do you mean this place shouldnt be covered in slime?";
                    }
                case 5:
                    {
                        return "Im going to cover this place in slime.";
                    }
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 35;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 12;
            randExtraCooldown = 0;
        }

        public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight)
        {
            itemWidth = 10;
            itemHeight = 10;
        }

        public override void DrawTownAttackSwing(ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)
        {
            item = ModContent.Request<Texture2D>("DarknessFallenMod/NPCs/TownNPCs/SlimePrinceNPCAttack").Value;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement("The eldest son of the royal slime family. While he may be weaker than the king and queen, he is far from the slime army's weakest soilders.")
            }) ;
        }
    }
}
