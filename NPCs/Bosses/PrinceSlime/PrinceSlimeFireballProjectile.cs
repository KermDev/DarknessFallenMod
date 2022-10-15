using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.NPCs.Bosses.PrinceSlime
{
    public class PrinceSlimeFireballProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Fireball;

        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;

            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = false;
            Projectile.light = 0.4f;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
        }

        public const float GRAVITY = 0.15f;
        public override void AI()
        {
            Projectile.velocity.Y += GRAVITY;
            //Main.NewText(Projectile.velocity);
        }
    }
}
