using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Tools.GamblerRod
{
	public class GamblerRodHook : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambler's Bobber");
		}

		public override void SetDefaults()
		{
			// These are copied through the CloneDefaults method
			// Projectile.width = 14;
			// Projectile.height = 14;
			// Projectile.aiStyle = 61;
			// Projectile.bobber = true;
			// Projectile.penetrate = -1;
			// Projectile.netImportant = true;
			Projectile.CloneDefaults(ProjectileID.BobberWooden);

			DrawOriginOffsetY = -8; // Adjusts the draw position
		}

        public override void Kill(int timeLeft)
        {

        }

        // What if we want to randomize the line color
        public override void AI()
		{

		}

		public override void ModifyFishingLine(ref Vector2 lineOriginOffset, ref Color lineColor)
		{
			// Change these two values in order to change the origin of where the line is being drawn.
			// This will make it draw 47 pixels right and 31 pixels up from the player's center, while they are looking right and in normal gravity.
			lineOriginOffset = new Vector2(45, -29);
		}
	}
}