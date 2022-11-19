using System;
using DarknessFallenMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DarknessFallenMod.Core.PrimitiveDrawing;

public class SwordTrailAlt : PrimitiveTrail
{
    private CircularBuffer<Vector2> Starts;
    private CircularBuffer<Vector2> Tips;
    public void AddPos(Vector2 start, Vector2 tip)
    {
        Starts.PushBack(start);
        Tips.PushBack(tip);
    }

    private Texture2D tex;
    private Func<float, Color> ColorFunc;
    public SwordTrailAlt(Entity parent, int size, Func<float, Color> colors, Texture2D tex = null) : base(parent, TrailLayer.PreProjectiles)
    {
        Starts = new CircularBuffer<Vector2>(size);
        Tips = new CircularBuffer<Vector2>(size);
        this.tex = tex;
        ColorFunc = colors;
    }
    public override void Draw()
    {
        if (Starts.Count < 2) return;
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
        if (Starts.Count < 2) return;
        for (int i = 0; i < Starts.Count - 1; i++)
        {
            Vector2 pos1 = Parent.Center + Starts[i]; // bottom left
            Vector2 pos2 = Parent.Center + Tips[i]; // top left
            Vector2 pos3 = Parent.Center + Starts[i + 1]; // bottom right
            Vector2 pos4 = Parent.Center + Tips[i + 1]; // top right

            float prog1 = (float)i / Starts.Count;
            float prog2 = (float)(i + 1) / Starts.Count;

            Color c1 = ColorFunc(prog1);
            Color c2 = ColorFunc(prog2);
            
            // make a triangle strip using AddVertex with texCoord x value as progress
            AddVertex(pos1, c1, new Vector2(prog1, 1));
            AddVertex(pos2, c1, new Vector2(prog1, 0));
            AddVertex(pos3, c2, new Vector2(prog2, 1));
            
            AddVertex(pos2, c1, new Vector2(prog1, 0));
            AddVertex(pos3, c2, new Vector2(prog2, 1));
            AddVertex(pos4, c2, new Vector2(prog2, 0));
        }
    }

    public override void FadeTrail()
    {
        if (Starts.Count < 2)
        {
            Faded = true;
            return;
        }
        Starts.PopFront();
        Tips.PopFront();
    }
}