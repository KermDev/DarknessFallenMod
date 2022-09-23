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
using Terraria.GameContent.Personalities;

namespace DarknessFallenMod.NPCs.TownNPCs
{
    [AutoloadHead]
    class SlimePrinceNPC : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Prince");

            Main.npcFrameCount[NPC.type] = 26;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 0;
            NPCID.Sets.AttackFrameCount[NPC.type] = 5;
            NPCID.Sets.DangerDetectRange[NPC.type] = 450;
            NPCID.Sets.AttackType[NPC.type] = 3;
            NPCID.Sets.AttackTime[NPC.type] = 12;
            NPCID.Sets.AttackAverageChance[NPC.type] = 20;
            //NPCID.Sets.HatOffsetY[NPC.type] = 0;

            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Love) // Example Person prefers the .
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike) // Example Person dislikes the .
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate) // Example Person dislikes the .
                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Love) // understands the food chain of slimes;
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Dislike) // he always kills slimes;
                .SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Like) // cool technology
                .SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Hate); // should be giving tax to the lsime npc
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
            
            AnimationType = NPCID.Guide;
        }

        public override void AI()
        {
            if (NPC.collideY && NPC.velocity.X != 0 && Main.rand.NextBool(8))
            {
                int dust = Dust.NewDust(NPC.Center + new Vector2(new Random().Next(-14, 14), 24), 7, 7, DustID.GreenFairy, 0, 0, 0, new Color(150, 150, 150), 1.5f);
                Main.dust[dust].velocity = Vector2.Zero;
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            if (Systems.DownedBossSystem.downedPrinceSlime)
            {
                return true;
            }

            return false;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            { "Albert",
                "Charles",
                "Harry",
                "Edward",
                "Bert",
                "Arthur",
                "Uther",
                "William",
                //william;
                "Henry",
                "Steven",
                //henry;
                "Richard",
                "John"
                //oi;
                //horrible historise king and squeen song;
            };
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Slime Shop";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
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
            switch (Main.rand.Next(7))
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
                case 6:
                    {
                        return "Y'Know, once upon a slime, it was daylight savings slime. I was outside walking with a slime, having a great slime, when they asked if i had watched CSI: slime scene investigation. Have you?";
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
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement("The eldest son of the royal slime family. While he may be weaker than the king and queen, he is far from the slime army's weakest soilders.")
            });
        }
    }
}