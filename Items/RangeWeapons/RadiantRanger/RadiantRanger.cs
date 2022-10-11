using static DarknessFallenMod.Systems.CoroutineSystem;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace DarknessFallenMod.Items.RangeWeapons.RadiantRanger
{
    public class RadiantRanger : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Arrows strike enemies in proximity with lightning".GetColored(Color.LightGoldenrodYellow));
        }

        public override void SetDefaults()
        {
            Item.damage = 46;
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

        const int lightningCD = 4;
        const int range = 96;
        int timer = lightningCD;
        public override void AI(Projectile projectile)
        {
            if (!shotFromRadiantRanger) return;
            /*
            if (Main.netMode != NetmodeID.Server)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.PortalBoltTrail);
            }*/

            timer++;
            if (timer > lightningCD)
            {
                float minDistSQ = range * range;
                if (DarknessFallenUtils.TryGetClosestEnemyNPC(projectile.Center, out NPC closest, minDistSQ, false))
                {
                    timer = 0;

                    Vector2 animPos = projectile.Center;
                    Vector2 dirToTarget = projectile.Center.DirectionTo(closest.Center);
                    float animRot = dirToTarget.ToRotation() - MathHelper.PiOver2;

                    float scale = 1;

                    IEnumerator drawE = DarknessFallenUtils.DrawCustomAnimation(
                        ModContent.Request<Texture2D>("DarknessFallenMod/Items/RangeWeapons/RadiantRanger/LightningFXProjectile", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                        frame => animPos - Main.screenPosition + Vector2.One * Main.rand.Next(-2, 2),
                        12,
                        3,
                        origin: new Vector2(26, 7) * scale,
                        rotation: frame => animRot + Main.rand.NextFloat(-0.2f, 0.2f),
                        color: frame => Color.White * Main.rand.NextFloat(0.7f, 1f),
                        onFrame: frame => LightningStrikeUpdate(frame, closest, dirToTarget, animPos, scale, projectile),
                        scale: scale
                        );

                    StartCoroutine(drawE, CoroutineType.PostDrawTiles);

                    //StartCoroutine(DelayedHitTarget(projectile.Center, closest));
                }
            }
        }

        void LightningStrikeUpdate(int frame, NPC npc, Vector2 dir, Vector2 center, float scale, Projectile proj)
        {
            float size = scale * 96;

            Vector2 endPos = center + dir * size;

            int dustType = DustID.AmberBolt;
            int width = 8;
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(center + dir * size * (frame / 12f), Main.rand.Next(-width, width), Main.rand.Next(-width, width), dustType, Alpha: 70);
                dust.noGravity = true;
                dust.velocity = dir.RotatedByRandom(0.3f) * 7;
            }

            if (frame == 3)
            {
                Player player = Main.player[proj.owner];
                player.GetModPlayer<DarknessFallenPlayer>().ShakeScreen(2f, 0.4f);

                player.ApplyDamageToNPC(npc, 46, 0, 0, false);

                SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, endPos);

                DarknessFallenUtils.NewDustCircular(endPos, dustType, 13, dir * 13, amount: 4, alpha: 70).ForEach(dust => 
                {
                    dust.velocity = dust.velocity.RotatedByRandom(0.1f);
                    dust.noGravity = true;
                });
            }
        }

        // old ver
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
