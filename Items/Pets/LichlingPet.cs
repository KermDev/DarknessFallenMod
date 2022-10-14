using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Pets
{
    public class LichlingPet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lichling");

            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active)
            {
                Projectile.active = false;
                return;
            }

            if (!player.dead && player.HasBuff(ModContent.BuffType<LichlingBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            Vector2 flyToPos = player.Center + new Vector2(player.direction * -40, -22);
            Projectile.Center = Vector2.Lerp(Projectile.Center, flyToPos, 0.2f);

            Projectile.spriteDirection = player.direction;

            Projectile.BasicAnimation(8);
        }
    }
}
