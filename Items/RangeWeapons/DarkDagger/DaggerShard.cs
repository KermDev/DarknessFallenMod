using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.RangeWeapons.DarkDagger
{
    public class DaggerShard : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dagger Shard");
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Shuriken);
            Projectile.width = 18;
            Projectile.damage = 180;
            Projectile.height = 18;
            Projectile.timeLeft = 150;
            Projectile.aiStyle = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
        }
    }
}
