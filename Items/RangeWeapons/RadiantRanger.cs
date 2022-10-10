using static DarknessFallenMod.Systems.CoroutineSystem;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.RangeWeapons
{
    public class RadiantRanger : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = 34320;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
            Item.noMelee = true;
        }

        public override void UseItemFrame(Player player)
        {
            player.itemLocation += Vector2.UnitX.RotatedBy(player.itemRotation) * -8 * player.direction;
        }

        public override void AddRecipes()
        {
            /*
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.FungiteBar>(), 15)
                .AddTile(TileID.Anvils)
                .Register();*/
        }
    }

    public class RadiantRangerProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        bool shotFromRadiantRanger;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource)
            {
                if (itemSource.Item.type == ModContent.ItemType<RadiantRanger>())
                {
                    shotFromRadiantRanger = true;
                }
            }
        }

        int timer = 10;
        public override void AI(Projectile projectile)
        {
            if (!shotFromRadiantRanger) return;
            /*
            if (Main.netMode != NetmodeID.Server)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.PortalBoltTrail);
            }*/

            timer++;
            if (timer > 10)
            {
                if (DarknessFallenUtils.TryGetClosestEnemyNPC(projectile.Center, out NPC closest, 20000f))
                {
                    timer = 0;
                    StartCoroutine(DelayedHitTarget(projectile.Center, closest));
                }
            }
        }

        // Non-Projectile Projectile 0_0
        const int delayHT = 1;
        const int speedHT = 24;
        const int maxTimeHT = 320;
        IEnumerator DelayedHitTarget(Vector2 startPos, NPC target)
        {
            if (Main.netMode == NetmodeID.Server) yield return false;

            int timeLeft = maxTimeHT / delayHT;
            Vector2 currPos = startPos;
            Vector2 currDir = startPos.DirectionTo(target.Center);
            while (timeLeft <= 0)
            {
                Vector2 inverseDustVel = -currDir * 10;
                //Dust.NewDustDirect(currPos, 0, 0, DustID.PortalBoltTrail, inverseDustVel.X, inverseDustVel.Y).noGravity = true;
                DarknessFallenUtils.NewDustCircular(currPos, DustID.PortalBoltTrail, 1, inverseDustVel).ForEach(dust => dust.noGravity = true);

                if (target.Hitbox.Contains((int)currPos.X, (int)currPos.Y))
                {
                    target.StrikeNPC(200, 0, 0);
                    break;
                }

                currPos += currDir * speedHT;

                if (target.CanBeChasedBy())
                {
                    currDir = currPos.DirectionTo(target.Center);
                }

                timeLeft--;
                yield return WaitFor.Frames(delayHT);
            }


        }
    }
}
