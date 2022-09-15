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
			Item.damage = 41;
			Item.knockBack = 6;
			Item.crit = 6;

			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item1;
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			target.AddBuff(ModContent.BuffType<Buffs.ExoflameBuff>(), 600);
            
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC temptarget = Main.npc[k];

                float sqrDistanceToTarget = Vector2.Distance(temptarget.Center, player.Center);
                float sqrMaxDetectDistance = 100;
                
                if (temptarget.CanBeChasedBy() && sqrDistanceToTarget < sqrMaxDetectDistance)
                {
                    temptarget.StrikeNPC(damage, 0f, 1, crit);
                    Vector2 pos = temptarget.Center;

                    Vector2 Direction = player.Center - pos;
                    for (int i = 0; i < 20; i++)
                    {
                        Dust.NewDust(pos + i * Direction / 20, new Random().Next(6, 10), new Random().Next(6, 10), DustID.InfernoFork, 0, 0);
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.MagmiteBar>(), 20)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
