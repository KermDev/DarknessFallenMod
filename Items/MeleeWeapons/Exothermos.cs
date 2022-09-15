using System;
using System.Collections.Generic;
using Terraria.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    public class Exothermos : ModItem
    {
		
        public override void SetDefaults()
        {
			Item.width = 45;
			Item.height = 45;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.autoReuse = true;

			Item.DamageType = DamageClass.Melee;
			Item.damage = 50;
			Item.knockBack = 6;
			Item.crit = 6;

			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item1;
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			target.AddBuff(ModContent.BuffType<Buffs.ExoflameBuff>(), 600);
            List<Vector2> Positions = new List<Vector2>();

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC temptarget = Main.npc[k];
                // Check if NPC able to be targeted. It means that NPC is
                // 1. active (alive)
                // 2. chaseable (e.g. not a cultist archer)
                // 3. max life bigger than 5 (e.g. not a critter)
                // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                // 5. hostile (!friendly)
                // 6. not immortal (e.g. not a target dummy)
                // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations

                float sqrDistanceToTarget = Vector2.Distance(temptarget.Center, player.Center);
                float sqrMaxDetectDistance = 100;
                // Check if it is within the radius
                if (sqrDistanceToTarget < sqrMaxDetectDistance)
                {
                    temptarget.StrikeNPC(damage, 0f, 1, crit);
                    Positions.Add(temptarget.Center);
                }
            }
            foreach (Vector2 pos in Positions)
            {
                Vector2 Direction = player.Center - pos;
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(pos + i * Direction / 20, new Random().Next(6, 10), new Random().Next(6, 10), DustID.InfernoFork, new Random().Next(0, 0), new Random().Next(0, 0));
                }
            }
        }
    }
}
