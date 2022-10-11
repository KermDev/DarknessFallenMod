using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MeleeWeapons.MoltenUchigatana
{
    public class MoltenUchigatanaProjectile : ModProjectile
    {
        public override string Texture => "DarknessFallenMod/Items/MeleeWeapons/MoltenUchigatana/MoltenUchigatana";

        Player Player => Main.player[Projectile.owner];

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;

            Projectile.knockBack = 8;

            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 9999;
            Projectile.ownerHitCheck = true;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 3;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * Player.itemAnimationMax - 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - (MathHelper.PiOver2 * 0.75f * Player.direction);
        }

        public override void AI()
        {
            if (Player.ItemAnimationEndingOrEnded)
            {
                Projectile.Kill();
                return;
            }

            Player.heldProj = Projectile.whoAmI;
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter, true);

            float swingBy = MathF.Pow((float)Player.itemAnimation / Player.itemAnimationMax, 4) * 0.08f;
            Projectile.rotation += swingBy * Player.direction;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 texSize = tex.Size();

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation + MathHelper.PiOver4 * Player.direction,
                new Vector2(Player.direction == -1 ? texSize.X : 0, texSize.Y),
                1,
                Player.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0
                );

            return false;
        }
    }
}
