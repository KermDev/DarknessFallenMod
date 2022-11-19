using System;
using System.Collections.Generic;
using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DarknessFallenMod.Core.PrimitiveDrawing
{
    public enum TrailLayer
    {
        PreNPCs,
        PostNPCs,
        PreProjectiles,
        PostProjectiles
    }

    public class SwordTrail : PrimitiveTrail
    {
        private CircularBuffer<Vector2> Positions;
        public void AddPos(Vector2 pos)
        {
            Positions.PushBack(pos);
        }

        private Texture2D tex;
        private Func<float, Color> ColorFunc;
        public SwordTrail(Entity parent, int size, Func<float, Color> colors, Texture2D tex = null) : base(parent, TrailLayer.PreProjectiles)
        {
            Positions = new CircularBuffer<Vector2>(size);
            this.tex = tex;
            ColorFunc = colors;
        }
        public override void Draw()
        {
            if (Positions.Count < 2) return;
            GraphicsDevice device = Main.graphics.GraphicsDevice;
            Effect effect = DarknessFallenMod.TrailShader;
            effect.Parameters["WorldViewProjection"].SetValue(DarknessFallenUtils.GetMatrix());

            device.SetVertexBuffer(null);
            DynamicVertexBuffer vertexBuffer = new DynamicVertexBuffer(device, typeof(VertexPositionColorTexture), Vertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData(Vertices.ToArray());
            device.SetVertexBuffer(vertexBuffer);
            if (tex != null)
            {
                effect.Parameters["tex"].SetValue(tex);
                effect.CurrentTechnique.Passes["Texture"].Apply();
            }
            else
            {
                effect.CurrentTechnique.Passes["Default"].Apply();
            }
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, DarknessFallenUtils.GetPrimitiveCount(Vertices.Count, PrimitiveType.TriangleList));
        }

        public override void PrepareVertices()
        {
            if (Positions.Count < 2) return;
            for (int i = 0; i < Positions.Count - 1; i++)
            {
                Vector2 pos = Parent.Center;
                Vector2 pos2 = Parent.Center + Positions[i];
                Vector2 pos3 = Parent.Center + Positions[i + 1];

                float prog1 = (float)i / Positions.Count;
                float prog2 = (float)(i + 1) / Positions.Count;

                Color c1 = ColorFunc(prog1);
                Color c2 = ColorFunc(prog2);

                AddVertex(pos2, c1, new Vector2(prog1, 1));
                AddVertex(pos, c1, new Vector2(prog1, 0));
                AddVertex(pos, c2, new Vector2(prog2, 0));

                AddVertex(pos, c2, new Vector2(prog2, 0));
                AddVertex(pos3, c2, new Vector2(prog2, 1));
                AddVertex(pos2, c1, new Vector2(prog1, 1));
            }
        }

        public override void FadeTrail()
        {
            if (Positions.Count < 2)
            {
                Faded = true;
                return;
            }
            Positions.PopFront();
        }
    }
    public abstract class PrimitiveTrail
    {
        public List<VertexPositionColorTexture> Vertices;
        public Entity Parent;
        public TrailLayer Layer;
        public bool Fading = false;
        public bool Faded = false;
        public abstract void Draw();
        public abstract void PrepareVertices();
        public virtual void Update() {}
        public abstract void FadeTrail();
        public void AddVertex(Vector2 position, Color color, Vector2 texCoord = default)
        {
            Vertices.Add(new VertexPositionColorTexture(new Vector3(position - Main.screenPosition, 0), color, texCoord));
        }
        public PrimitiveTrail(Entity parent, TrailLayer layer)
        {
            Vertices = new List<VertexPositionColorTexture>();
            Parent = parent;
            Layer = layer;
            TrailManager.AddTrail(this);
        }
        public void Kill()
        {
            Fading = true;
        }
    }
}
