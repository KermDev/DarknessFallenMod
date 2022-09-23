using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace DarknessFallenMod.Items.SummonWeapons
{
    public class CultSlime : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("[c/00aa22:Summons the slime cult to fight for you]");
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Summon;
            Item.width = 37;
            Item.height = 37;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.knockBack = 0;
            Item.noMelee = true;

            Item.value = 34320;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.stack = 1;

            Item.shoot = ModContent.ProjectileType<CultSlimeMinion>();
            Item.shootSpeed = 0;
            Item.buffType = ModContent.BuffType<CultSlimeBuff>();
        }

        readonly int[] damageNumbers = new int[] { 9, 6, 3 };

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            foreach (int dmg in damageNumbers)
            {
                int proj = Projectile.NewProjectile(source, position, Vector2.Zero, type, dmg, knockback, player.whoAmI);
                Main.projectile[proj].originalDamage = dmg;
            }

            return false;
        }

        public override void UseItemFrame(Player player)
        {
            player.itemLocation -= Vector2.UnitY * 12;
        }
    }

    public class CultSlimeMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;

            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 21;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.damage = 0;
            Projectile.alpha = 50;
            Projectile.minionSlots = 0.33333333333333333333334f;
            

            Projectile.localNPCHitCooldown = 20;
            Projectile.usesLocalNPCImmunity = true;
        }

        Player Player => Main.player[Projectile.owner];

        int slimeType;
        Color lightColor;
        public override void OnSpawn(IEntitySource source)
        {
            switch (Projectile.damage)
            {
                case 9:
                    slimeType = 0;
                    lightColor = Color.Red;
                    Projectile.ai[1] = -MathHelper.PiOver2;
                    break;
                case 6:
                    slimeType = 2;
                    lightColor = Color.Blue;
                    Projectile.ai[1] = - MathHelper.ToRadians(210);
                    break;
                case 3:
                    slimeType = 4;
                    lightColor = Color.Green;
                    Projectile.ai[1] = MathHelper.ToRadians(30);
                    break;
            }

            //if (slimeType == 0) Projectile.minionSlots = 1;
        }

        const float speed = 14f;
        const float inertia = 12f;
        public override void AI()
        {
            FindFrame();

            if (DarknessFallenUtils.TryGetClosestEnemyNPC(Player.Center, out NPC npc, npc => npc.boss, 640000))
            {
                Vector2 velToTarg = (npc.Center + Main.rand.NextVector2Unit() * npc.width * 0.5f).DirectionFrom(Projectile.Center) * speed;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + velToTarg) / inertia;

                Projectile.rotation = Projectile.velocity.X * 0.05f;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.Center = Vector2.Lerp(Projectile.Center, Player.Center + Projectile.ai[1].ToRotationVector2() * 80, Main.rand.NextFloat(0.1f, 0.15f));
            }

            Projectile.ai[1] += 0.1f;

            if (!Main.dedServ)
            {
                float lightAmount = 0.002f;
                Lighting.AddLight(Projectile.Center, lightColor.R * lightAmount, lightColor.G * lightAmount, lightColor.B * lightAmount);
            }

            if (Player.HasBuff<CultSlimeBuff>())
            {
                Projectile.timeLeft = 2;
            }
        }

        void FindFrame()
        {
            Projectile.frameCounter++;

            if (Projectile.frameCounter >= 45)
            {
                Projectile.ai[0] += 1;

                if (Projectile.ai[0] > 1)
                {
                    Projectile.ai[0] = 0;
                }
            }

            Projectile.frame = (int)Projectile.ai[0] + slimeType;
        }

        public override bool? CanCutTiles()
        {
            return true;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }
    }

    public class CultSlimeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cult Slime");
            Description.SetDefault("The slime cult will fight for you");

            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CultSlimeMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
