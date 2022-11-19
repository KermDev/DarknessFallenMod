using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace DarknessFallenMod.Core.PrimitiveDrawing;

public class TrailManager : ModSystem
{
    private static List<PrimitiveTrail> trails;
    public override void Load()
    {
        trails = new List<PrimitiveTrail>();
        On.Terraria.Main.DrawProjectiles += DrawProjTrails;
        On.Terraria.Main.DrawNPCs += DrawNPCTrails;
    }

    public override void Unload()
    {
        trails = null;
        On.Terraria.Main.DrawProjectiles -= DrawProjTrails;
        On.Terraria.Main.DrawNPCs -= DrawNPCTrails;
    }

    public static void AddTrail(PrimitiveTrail trail)
    {
        trails.Add(trail);
    }

    private static void DrawNPCTrails(On.Terraria.Main.orig_DrawNPCs orig, Terraria.Main self, bool behindtiles)
    {
        DrawTrails(TrailLayer.PreNPCs);
        orig(self, behindtiles);
        DrawTrails(TrailLayer.PostNPCs);
    }

    private static void DrawProjTrails(On.Terraria.Main.orig_DrawProjectiles orig, Terraria.Main self)
    {
        DrawTrails(TrailLayer.PreProjectiles);
        orig(self);
        DrawTrails(TrailLayer.PostProjectiles);
    }

    public override void PostUpdateEverything()
    {
        for (int i = 0; i < trails.Count; i++)
        {
            PrimitiveTrail trail = trails[i];
            if (trail.Faded)
            {
                trails.RemoveAt(i);
                i--;
            }
            else
            {
                if (trail.Fading)
                    trail.FadeTrail();
                else
                    trail.Update();
            }
        }
    }
    public static void DrawTrails(TrailLayer layer)
    {
        // draw all trails which are on the given layer
        foreach (PrimitiveTrail trail in trails.Where(trail => trail.Layer == layer))
        {
            trail.PrepareVertices();
            trail.Draw();
            trail.Vertices.Clear();
        }
    }
}