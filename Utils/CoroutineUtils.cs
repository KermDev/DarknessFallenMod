using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI.Chat;
using static DarknessFallenMod.Systems.CoroutineSystem;

namespace DarknessFallenMod.Utils
{
    public static partial class DarknessFallenUtils
    {
        public static Texture2D LineTexture { get; private set; }
        public static IEnumerator EDrawPoint(Point point, int timeInFrames)
        {
            if (LineTexture is null)
            {
                LineTexture = new Texture2D(Main.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                LineTexture.SetData(new Color[] { Color.White });
            }

            int width = 1;

            int pointX = point.X - (int)(Main.screenWidth * 0.5f);
            int pointY = point.Y - (int)(Main.screenWidth * 0.5f);

            for (int i = 0; i < timeInFrames + 1; i++)
            {
                Main.spriteBatch.BeginDefault();

                Point screenPosPoint = Main.screenPosition.ToPoint();

                Main.spriteBatch.Draw(LineTexture, new Rectangle(pointX - screenPosPoint.X, point.Y - screenPosPoint.Y, Main.screenWidth, width), Color.Red);
                Main.spriteBatch.Draw(LineTexture, new Rectangle(point.X - screenPosPoint.X, pointY - screenPosPoint.Y, width, Main.screenWidth), Color.Red);

                Main.spriteBatch.End();

                yield return null;
            }
        }

        public static IEnumerator DrawCustomAnimation(
            Texture2D texture,
            Func<int, Vector2> positionOnScreen,
            int frames,
            int frequency,
            Func<int, Color> color = null,
            Vector2? origin = null,
            Func<int, float> rotation = null,
            float scale = 1f,
            SpriteEffects spriteEffects = SpriteEffects.None,
            Action<int> onFrame = null
            )
        {
            Vector2 texSize = texture.Size();
            int sourceHeight = (int)texSize.Y / frames;
            Vector2 drawOrigin = origin ?? texSize * 0.5f;

            int currFrame = 0;
            while (currFrame < frames)
            {
                for (int i = 0; i < frequency; i++)
                {
                    Main.spriteBatch.Begin(BeginType.Default);
                    Main.EntitySpriteDraw(
                        texture,
                        positionOnScreen.Invoke(currFrame),
                        new Rectangle(0, currFrame * sourceHeight, (int)texSize.X, sourceHeight),
                        color?.Invoke(currFrame) ?? Color.White,
                        rotation?.Invoke(currFrame) ?? 0,
                        drawOrigin,
                        scale,
                        spriteEffects,
                        0
                        );
                    Main.spriteBatch.End();
                    yield return null;
                }

                onFrame?.Invoke(currFrame);
                currFrame++;
            }
        }
    }
}
