using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Core
{
	/// <summary>
	/// Draws an item glowmask with a texture named "ItemName_Glow.png"
	/// </summary>
	public interface IGlowmask
	{
		/// <summary>
		/// Allows for custom glowmask drawing. Return false to disable automatic drawing.
		/// </summary>
		/// <returns></returns>
		public virtual bool PreDraw(ref PlayerDrawSet drawInfo, Texture2D glowTex) => true;
		Texture2D GlowmaskTexture
		{
			get
			{
				if (this is ModItem modItem)
					return ModContent.Request<Texture2D>($"{modItem.Texture}_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				return null;
			}
		}
	}

	public class GlowmaskPlayerDrawLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			Item item = drawPlayer.HeldItem;

			if (drawPlayer.ItemAnimationActive && item.ModItem is IGlowmask glowMaskItem)
			{
				Texture2D texture = glowMaskItem.GlowmaskTexture;
				if (texture is null || !glowMaskItem.PreDraw(ref drawInfo, texture)) return;

				SpriteEffects drawEffect = drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

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

	/// <summary>
	/// Draws glowmasks on dropped items.
	/// </summary>
	public class GlowmaskItem : GlobalItem
	{
		public override bool AppliesToEntity(Item entity, bool lateInstantiation)
		{
			return entity.ModItem is IGlowmask glowmask && glowmask.GlowmaskTexture != null;
		}

		public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			// Code adapted from vanilla.
			Texture2D texture = (item.ModItem as IGlowmask).GlowmaskTexture;
			Rectangle frame = Main.itemAnimations[item.type]?.GetFrame(texture) ?? texture.Frame();
			Vector2 drawOrigin = frame.Size() / 2f;
			Vector2 drawOffset = new((item.width / 2) - drawOrigin.X, item.height - frame.Height);
			Vector2 drawPosition = item.position - Main.screenPosition + drawOrigin + drawOffset;

			spriteBatch.Draw(texture, drawPosition, frame, new Color(250, 250, 250, item.alpha), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
	}
}