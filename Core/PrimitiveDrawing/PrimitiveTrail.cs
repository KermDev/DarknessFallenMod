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
