using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DarknessFallenMod.Utils;

public static class PrimitiveUtils
{
    private static int width;
    private static int height;
    private static Vector2 zoom;
    private static bool CheckGraphicsChanged()
    {
        var device = Main.graphics.GraphicsDevice;
        bool changed = device.Viewport.Width != width
                       || device.Viewport.Height != height
                       || Main.GameViewMatrix.Zoom != zoom;

        if (!changed) return false;

        width = device.Viewport.Width;
        height = device.Viewport.Height;
        zoom = Main.GameViewMatrix.Zoom;

        return true;
    }

    private static Matrix view;
    private static Matrix projection;
    public static Matrix GetMatrix()
    {
        if (!CheckGraphicsChanged()) return view * projection;
        view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up)
               * Matrix.CreateTranslation(width / 2f, height / -2f, 0)
               * Matrix.CreateRotationZ(MathHelper.Pi)
               * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
        projection = Matrix.CreateOrthographic(width, height, 0, 1000);
        return view * projection;
    }

    public static int GetPrimitiveCount(int vertexCount, PrimitiveType type)
    {
        return type switch
        {
            PrimitiveType.LineList => vertexCount / 2,
            PrimitiveType.LineStrip => vertexCount - 1,
            PrimitiveType.TriangleList => vertexCount / 3,
            PrimitiveType.TriangleStrip => vertexCount - 2,
            _ => 0
        };
    }
    
    public static Vector3 ToVector3(this Vector2 vector)
    {
        return new Vector3(vector.X, vector.Y, 0);
    }
    
    private static bool HasBegun(this SpriteBatch spriteBatch)
    {
        return (bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
    }
    public static void Reload(this SpriteBatch spriteBatch, BlendState blendState = default, SpriteSortMode sortMode = SpriteSortMode.Deferred)
    {
        if (spriteBatch.HasBegun())
        {
            spriteBatch.End();
        }
        SamplerState state = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
        DepthStencilState state2 = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
        RasterizerState state3 = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
        Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
        Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
        spriteBatch.Begin(sortMode, blendState, state, state2, state3, effect, matrix);
    }
    public static Vector2 GetRotation(IReadOnlyList<Vector2> oldPos, int index)
    {
        if (oldPos.Count == 1)
            return oldPos[0];

        if (index == 0)
        {
            return Vector2.Normalize(oldPos[1] - oldPos[0]).RotatedBy(MathHelper.Pi / 2);
        }

        return (index == oldPos.Count - 1
            ? Vector2.Normalize(oldPos[index] - oldPos[index - 1])
            : Vector2.Normalize(oldPos[index + 1] - oldPos[index - 1])).RotatedBy(MathHelper.Pi / 2);
    }
}

public class PrimitiveList
{
    private VertexPositionColorTexture[] vertices;
    private PrimitiveType type;
    private int currentIndex;
    private bool hasTexture;
    private Texture2D texture;
    public PrimitiveList(int capacity, PrimitiveType type)
    {
        vertices = new VertexPositionColorTexture[capacity];
        this.type = type;
    }

    public void SetTexture(Texture2D tex)
    {
        hasTexture = true;
        texture = tex;
    }
    public void AddVertex(Vector2 position, Color color, Vector2 coords = default, bool offset = true)
    {
        if (offset) position -= Main.screenPosition;
        vertices[currentIndex++] = new VertexPositionColorTexture(position.ToVector3(), color, coords);
    }
    public void Draw()
    {
        GraphicsDevice device = Main.graphics.GraphicsDevice;
        Effect effect = DarknessFallenMod.TrailShader;
        if (vertices.Length < 3) return;
        effect.Parameters["WorldViewProjection"].SetValue(PrimitiveUtils.GetMatrix());

        device.SetVertexBuffer(null);
        DynamicVertexBuffer vertexBuffer = new DynamicVertexBuffer(device, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
        vertexBuffer.SetData(vertices);
        device.SetVertexBuffer(vertexBuffer);
        
        if (hasTexture)
        {
            effect.Parameters["tex"].SetValue(texture);
            effect.CurrentTechnique.Passes["Texture"].Apply();
        }
        else
        {
            effect.CurrentTechnique.Passes["Default"].Apply();
        }
        device.DrawPrimitives(type, 0, PrimitiveUtils.GetPrimitiveCount(vertices.Length, type));
    }
}

public interface IPrimitiveDrawer
{
    void DrawPrimitives();
}

public class PrimitiveSystem : ModSystem
{
    public override void Load()
    {
        On.Terraria.Main.DrawProjectiles += DrawPrims;
    }

    private void DrawPrims(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
    {
        Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
        foreach (Projectile projectile in Main.projectile)
        {
            if (projectile.active && projectile.ModProjectile is IPrimitiveDrawer drawer)
            {
                drawer.DrawPrimitives();
            }
        }
        orig(self);
    }

    public override void Unload()
    {
        On.Terraria.Main.DrawProjectiles -= DrawPrims;
    }
}

// Circular buffer, ported from https://github.com/joaoportela/CircularBuffer-CSharp/blob/master/CircularBuffer/CircularBuffer.cs
// Has been modified quite a bit, removed most of the methods that wont actually be useful for trails
// Is used because its way more performance efficient than using a list
public class CircularBuffer<T> : IReadOnlyList<T>
{
    private readonly T[] buffer;
    private int start;
    private int end;
    private int size;
    public CircularBuffer(int capacity)
    {
        if (capacity < 1)
        {
            throw new ArgumentException(
                "Circular buffer cannot have negative or zero capacity.", nameof(capacity));
        }

        buffer = new T[capacity];
        size = 0;
        start = 0;
        end = 0;
    }
    public int Capacity => buffer.Length;

    public bool IsFull
    {
        get
        {
            return Count == Capacity;
        }
    }
    public bool IsEmpty => Count == 0;

    public T this[int index]
    {
        get
        {
            if (IsEmpty)
            {
                throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer is empty", index));
            }
            if (index >= size)
            {
                throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer size is {1}", index, size));
            }
            int actualIndex = InternalIndex(index);
            return buffer[actualIndex];
        }
        set
        {
            if (IsEmpty)
            {
                throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer is empty", index));
            }
            if (index >= size)
            {
                throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer size is {1}", index, size));
            }
            int actualIndex = InternalIndex(index);
            buffer[actualIndex] = value;
        }
    }
    public void PushBack(T item)
    {
        if (IsFull)
        {
            buffer[end] = item;
            Increment(ref end);
            start = end;
        }
        else
        {
            buffer[end] = item;
            Increment(ref end);
            ++size;
        }
    }
    public void PopFront()
    {
        ThrowIfEmpty("Cannot take elements from an empty buffer.");
        buffer[start] = default(T);
        Increment(ref start);
        --size;
    }
    public IEnumerator<T> GetEnumerator()
    {
        foreach (T val in buffer)
        {
            yield return val;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    private void ThrowIfEmpty(string message = "Cannot access an empty buffer.")
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException(message);
        }
    }
    private void Increment(ref int index)
    {
        if (++index == Capacity)
        {
            index = 0;
        }
    }
    private int InternalIndex(int index)
    {
        return start + (index < (Capacity - start) ? index : index - Capacity);
    }

    public int Count => size;
}