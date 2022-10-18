using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Armor.Magmite
{
    [AutoloadEquip(EquipType.Head)]
    public class MagmiteHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                "8% increased damage"
                );
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = 2000;
            Item.rare = ItemRarityID.Red;
            Item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.08f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<MagmiteChestplate>() && legs.type == ModContent.ItemType<MagmiteLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Fireballs shoot out of armor every 15 seconds dealing small damage to enemies" + "\nWhen you crit an enemy an explosion happens setting everything in the explosion on fire";
            player.GetModPlayer<MagmiteSetPlayer>().MagmiteSetEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.MagmiteBar>(), 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class MagmiteSetPlayer : ModPlayer
    {
        public bool MagmiteSetEquipped;
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (!MagmiteSetEquipped) return;

            if (crit) MagmiteExplosion(target.Center);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (!MagmiteSetEquipped) return;

            if (crit) MagmiteExplosion(proj.Center);
        }

        const int blastWidth = 256;
        void MagmiteExplosion(Vector2 center)
        {
            SoundEngine.PlaySound(SoundID.Item62, center);
            DarknessFallenUtils.ShakeScreenInRange(3, center, 1638400, 0.87f);

            Rectangle rect = new Rectangle((int)center.X - blastWidth / 2, (int)center.Y - blastWidth / 2, blastWidth, blastWidth);

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(rect.TopLeft(), rect.Width, rect.Height, DustID.Torch);
                Dust.NewDust(rect.TopLeft(), rect.Width, rect.Height, DustID.Smoke);
                Dust.NewDust(rect.TopLeft(), rect.Width, rect.Height, DustID.Lava);
            }

            for (int i = 0; i < Main.rand.Next(7, 9); i++)
            {
                Vector2 randPos = Main.rand.NextVector2FromRectangle(rect);
                Gore.NewGore(Player.GetSource_FromThis(), center, randPos.DirectionFrom(center) * 2.5f, GoreID.Smoke1 + Main.rand.Next(2));
            }

            DarknessFallenUtils.ForeachNPCInRectangle(rect, npc =>
            {
                if (!npc.friendly)
                {
                    Player.ApplyDamageToNPC(npc, 15, 0.2f, (int)(npc.Center.X - Player.Center.X), false);
                    npc.AddBuff(BuffID.OnFire, 240);
                }
            });
        }

        int fireballTimer;
        const int fireballCd = 900;
        public override void PostUpdate()
        {
            if (!MagmiteSetEquipped) return;

            fireballTimer++;
            if (Player.active && fireballTimer >= fireballCd)
            {
                if (DarknessFallenUtils.TryGetClosestEnemyNPC(Player.Center, out NPC npc, 102400))
                {
                    int i = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Player.Center.DirectionTo(npc.Center) * 5, ProjectileID.Fireball, 40, 1, Player.whoAmI);
                    Main.projectile[i].friendly = true;
                    Main.projectile[i].hostile = false;

                    fireballTimer = 0;
                }
            }
        }

        public override void ResetEffects()
        {
            MagmiteSetEquipped = false;
        }
    }
}
