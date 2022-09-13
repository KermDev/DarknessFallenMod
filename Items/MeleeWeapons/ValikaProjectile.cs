using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace DarknessFallenMod.Items.MeleeWeapons
{
    public class ValikaProjectile : ModProjectile
    {
        List<NPC> HitNPCs = new List<NPC>();

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            DisplayName.SetDefault("Valika");
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 600;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.LightGreen;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if(target == null)
            {
                return;
            }

            if (target.CanBeChasedBy())
            {
                HitNPCs.Add(target);
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection -= Projectile.direction;

            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }

            }

            float maxDetectRadius = 800f; // The maximum radius at which a projectile can detect a target
            float projSpeed = 15f; // The speed at which the projectile moves towards the target

            // Trying to find NPC closest to the projectile 
            NPC closestNPC = FindClosestNPC(maxDetectRadius);
            if (closestNPC == null)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                return;
            }
            // If found, change the velocity of the projectile and turn it in the direction of the target
            // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero 
            Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        // Finding the closest NPC to attack within maxDetectDistance range
        // If not found then returns null
        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                // Check if NPC able to be targeted. It means that NPC is
                // 1. active (alive)
                // 2. chaseable (e.g. not a cultist archer)
                // 3. max life bigger than 5 (e.g. not a critter)
                // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                // 5. hostile (!friendly)
                // 6. not immortal (e.g. not a target dummy)
                if (target.CanBeChasedBy() && !HitNPCs.Contains(target))
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }
    }
}
