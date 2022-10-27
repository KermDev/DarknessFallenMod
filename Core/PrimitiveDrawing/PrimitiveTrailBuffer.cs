using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Core.PrimitiveDrawing
{
    public class PrimitiveTrailBuffer : IDisposable
    {
        DynamicVertexBuffer _vertexBuffer;
        DynamicIndexBuffer _indexBuffer;

        GraphicsDevice graphicsDevice;

        public PrimitiveTrailBuffer(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            if (graphicsDevice is not null)
            {
                //_vertexBuffer = new(graphicsDevice, typeof(VertexPositionColorTexture), pointsLenght * 2, BufferUsage.None);
                //_indexBuffer = new(graphicsDevice, IndexElementSize.SixteenBits, )
            }
        }

        public void Dispose()
        {
            _vertexBuffer?.Dispose();
            _indexBuffer?.Dispose();
        }
    }
}
