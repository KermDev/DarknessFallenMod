using DarknessFallenMod.Content.Subworlds;
using Microsoft.Xna.Framework.Input;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;

namespace DarknessFallenMod.Common.Systems;

public sealed class TestSystem : ModSystem
{
    public override void PostUpdateInput() {
        if (Main.keyState.IsKeyDown(Keys.F) && !Main.oldKeyState.IsKeyDown(Keys.F)) {
            if (SubworldSystem.IsActive<EquinoxSubworld>()) {
                SubworldSystem.Exit();
            }
            else {
                SubworldSystem.Enter<EquinoxSubworld>();
            }
        }
    }
}