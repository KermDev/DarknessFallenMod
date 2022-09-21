using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod
{
    public class DarknessFallenItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

		public Texture2D WorldGlowMask { get; set; } = null;
    }

    public class DarknessFallenItemDrawLayer : PlayerDrawLayer
    {
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			Item item = drawPlayer.HeldItem;
			DarknessFallenItem darknessFallenItem = item.GetGlobalItem<DarknessFallenItem>();

			if (!drawPlayer.ItemAnimationActive || darknessFallenItem.WorldGlowMask == null) return;

			SpriteEffects drawEffect = drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Texture2D texture = darknessFallenItem.WorldGlowMask;

			Vector2 textureShakeFix = drawPlayer.position - drawPlayer.VisualPosition;

			Vector2 drawPosition = drawPlayer.itemLocation - Main.screenPosition - textureShakeFix;
			drawPosition.X = (int)drawPosition.X;
			drawPosition.Y = (int)drawPosition.Y;

			Vector2 drawOrigin = drawPlayer.direction == -1 ? new Vector2(texture.Width, texture.Height) : new Vector2(0, texture.Height);

			if (item.useStyle == ItemUseStyleID.Shoot)
			{
				drawPosition += new Vector2(texture.Width / 2, texture.Height / 2);

				Vector2 offset = new Vector2(10, 0);
				if (item.ModItem != null)
				{
					offset = item.ModItem.HoldoutOffset() ?? new Vector2(10, 0);
				}

				drawPosition.Y += offset.Y;

				drawOrigin = drawPlayer.direction == -1 ? new Vector2(texture.Width + offset.X, texture.Height / 2) : new Vector2(-offset.X, texture.Height / 2);
			}

			Rectangle drawRect = new Rectangle(0, 0, texture.Width, texture.Height);

			DrawData drawData = new DrawData(
				texture,
				drawPosition,
				drawRect,
				item.GetAlpha(Color.White),
				drawPlayer.itemRotation,
				drawOrigin,
				item.scale,
				drawEffect,
				0
				);

			drawInfo.DrawDataCache.Add(drawData);
		}
	}
}
