using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class PhaloriteSwordProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phalorite Orb"); // Name of the projectile. It can be appear in chat
            Main.projFrames[Projectile.type] = 1; //number of frames in the animation;
        }

        // Setting the default parameters of the projectile
        // You can check most of Fields and Properties here https://github.com/tModLoader/tModLoader/wiki/Projectile-Class-Documentation
        public override void SetDefaults()
        {
            Projectile.width = 16; // The width of projectile hitbox
            Projectile.height = 16; // The height of projectile hitbox
            Projectile.penetrate = 10;
            Projectile.aiStyle = 1; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Melee; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 0.9f; // How much light emit around the projectile
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 120; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(9))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff);
            }

            Projectile.rotation += 0.2f;
        }

        public override void Kill(int timeLeft)
        {
            DarknessFallenUtils.NewDustCircular(Projectile.Center, DustID.ShadowbeamStaff, 5, speedFromCenter: 5, amount: 16);
        }
    }
}