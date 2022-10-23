using System;
using System.Collections.Generic;
using Terraria.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DarknessFallenMod.Core;

namespace DarknessFallenMod.Items.MeleeWeapons.Exothermos
{
    public class Exothermos : ModItem, IGlowmask
    {
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;

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

            Item.shoot = ModContent.ProjectileType<ExothermosProjectile>();
            Item.shootSpeed = 14;
        }

        /*
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.ExoflameBuff>(), 600);

            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC temptarget = Main.npc[k];

                float sqrDistanceToTarget = Vector2.Distance(temptarget.Center, player.Center);
                float sqrMaxDetectDistance = 150;

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
        */

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.PiOver4 * 0.09f);
        }

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (Main.rand.NextBool(3)) Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.InfernoFork);
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
