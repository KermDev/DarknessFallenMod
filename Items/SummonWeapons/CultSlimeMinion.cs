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
    public class CultSlimeMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 21;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 7200;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.damage = 0;
            Projectile.alpha = 50;

            Projectile.localNPCHitCooldown = 30;
            Projectile.usesLocalNPCImmunity = true;
        }

        Player Player => Main.player[Projectile.owner];

        int slimeType;
        Color lightColor;
        public override void OnSpawn(IEntitySource source)
        {
            switch (Projectile.damage)
            {
                case 6:
                    slimeType = 0;
                    lightColor = Color.Red;
                    Projectile.ai[1] = -MathHelper.PiOver2;
                    break;
                case 4:
                    slimeType = 2;
                    lightColor = Color.Blue;
                    Projectile.ai[1] = -MathHelper.PiOver2 - MathHelper.ToRadians(120);
                    break;
                case 2:
                    slimeType = 4;
                    lightColor = Color.Green;
                    Projectile.ai[1] = -MathHelper.PiOver2 + MathHelper.ToRadians(120);
                    break;
            }
        }

        
        public override void AI()
        {
            FindFrame();

            Vector2 goToPos = Player.Center + Projectile.ai[1].ToRotationVector2() * 80;
            if (DarknessFallenUtils.TryGetClosestEnemyNPC(Player.Center, out NPC npc, 102400))
            {
                goToPos = npc.Center;
            }

            Projectile.Center = Vector2.Lerp(Projectile.Center, goToPos, Main.rand.NextFloat(0.1f, 0.15f));

            Projectile.ai[1] += 0.1f;

            if (!Main.dedServ)
            {
                float lightAmount = 0.002f;
                Lighting.AddLight(Projectile.Center, lightColor.R * lightAmount, lightColor.G * lightAmount, lightColor.B * lightAmount);
            }

            Projectile.timeLeft = 2;
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
    }
}
