using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class FungiteHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                ""
                );
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 72);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<FungiteChestplate>() && legs.type == ModContent.ItemType<FungiteLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "3 defense\nMushroom minion that does small damage to enemies";
            player.statDefense += 3;

            int minionType = ModContent.ProjectileType<FungiteArmorMinion>();
            if (player.ownedProjectileCounts[minionType] == 0)
            {
                Projectile minion = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, Vector2.Zero, minionType, 10, 1, player.whoAmI);
                minion.originalDamage = 20;
                minion.netUpdate = true;
            }

            Projectile proj = Main.projectile.FirstOrDefault(proj => proj.owner == player.whoAmI && proj.type == minionType);
            if (proj is not null) proj.timeLeft = 2;
        }
    }

    public class FungiteArmorMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 9;

            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 20;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.minion = true;
            Projectile.damage = 0;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.netImportant = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;
        public override bool MinionContactDamage() => true;
        

        Player Player => Main.player[Projectile.owner];

        public override void AI()
        {
            FindFrame();

            switch (aiState)
            {
                case AIState.Idle:
                    Idle();
                    break;
                case AIState.Walk:
                    Walk();
                    break;
                case AIState.Roll:
                    Roll();
                    break;
            }

            Projectile.spriteDirection = -MathF.Sign(Projectile.velocity.X);

            FX();
        }

        void FX()
        {
            if (Main.rand.NextBool(16) && aiState == AIState.Walk)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GlowingMushroom, Scale: Main.rand.NextFloat(0.2f, 1f));
            }

            if (!Main.dedServ) Lighting.AddLight(Projectile.Center, 0.2f, 0.1f, 0.5f);
        }

        const float gravity = 0.3f;
        const float acceleration = 0.05f;
        const float maxSpeed = 5;
        const float walkThreshold = 3200;
        const float teleportThreshold = 2000000f;
        const float targetThreshold = 640000f;
        void Idle()
        {
            Projectile.velocity.Y += gravity;
            Projectile.velocity.X *= 0.6f;

            if (Player.Center.DistanceSQ(Projectile.Center) > walkThreshold) aiState = AIState.Walk;

            if (DarknessFallenUtils.TryGetClosestEnemyNPC(Projectile.Center, out NPC npc, targetThreshold))
            {
                Target = npc;
                aiState = AIState.Roll;
                Projectile.tileCollide = false;
                return;
            }
        }

        void Walk()
        {
            if (Main.rand.NextBool(10) && Projectile.velocity.Y == 0) Projectile.velocity.Y -= 5;

            Projectile.velocity.Y += gravity;
            Projectile.velocity.X += MathF.Sign(Projectile.Center.DirectionTo(Player.Center).X) * acceleration;
            Projectile.velocity.X = Math.Clamp(Projectile.velocity.X, -maxSpeed, maxSpeed);

            float playerDistSQ = Player.Center.DistanceSQ(Projectile.Center);
            if (playerDistSQ < walkThreshold)
            {
                aiState = AIState.Idle;
                return;
            }
            else if (playerDistSQ > teleportThreshold)
            {
                Projectile.Center = Player.Center;
            }

            if (DarknessFallenUtils.TryGetClosestEnemyNPC(Player.Center, out NPC npc, npc => npc.boss, targetThreshold))
            {
                Target = npc;
                aiState = AIState.Roll;
                Projectile.tileCollide = false;
                return;
            }
        }

        NPC Target;
        bool shouldChase => Target is not null && Target.CanBeChasedBy();
        float inertia = 20;
        float speed = 10;
        void Roll()
        {
            if (shouldChase)
            {
                Vector2 velToTarg = (Target.Center + Main.rand.NextVector2Unit() * Target.width * 0.5f).DirectionFrom(Projectile.Center) * speed;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + velToTarg) / inertia;
            }
            else
            {
                if (DarknessFallenUtils.TryGetClosestEnemyNPC(Player.Center, out NPC npc, npc => npc.boss, targetThreshold))
                {
                    Target = npc;
                    return;
                }

                float playerDistSQ = Player.Center.DistanceSQ(Projectile.Center);
                if (playerDistSQ < walkThreshold)
                {
                    aiState = AIState.Idle;
                    Projectile.tileCollide = true;
                    return;
                }
                else
                {
                    Vector2 velToPlayer = Player.Center.DirectionFrom(Projectile.Center) * speed;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + velToPlayer) / inertia;
                }
            }
        }

        enum AIState
        {
            Idle,
            Walk,
            Roll
        }
        AIState aiState = AIState.Idle;
        void FindFrame()
        {
            Projectile.frameCounter++;

            if (Projectile.frameCounter > 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                switch (aiState)
                {
                    case AIState.Idle:
                        if (Projectile.frame < 4 || Projectile.frame > 5) Projectile.frame = 4;
                        break;
                    case AIState.Walk:
                        if (Projectile.frame < 6 || Projectile.frame > 8) Projectile.frame = 6;
                        break;
                    case AIState.Roll:
                        if (Projectile.frame < 0 || Projectile.frame > 3) Projectile.frame = 0;
                        break;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileInHBCenter(lightColor, true, origin: new Vector2(14, 14));

            return false;
        }
    }
}
