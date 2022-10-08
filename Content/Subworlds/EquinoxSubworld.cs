using System.Collections.Generic;
using SubworldLibrary;
using Terraria.WorldBuilding;

namespace DarknessFallenMod.Content.Subworlds;

public sealed class EquinoxSubworld : Subworld
{
    public override int Width => 600;

    public override int Height => 400;

    public override List<GenPass> Tasks => new();
}