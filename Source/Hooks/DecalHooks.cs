using System.IO;
using Celeste.Mod.LocalizationHelper.Utils;
using IL.Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.LocalizationHelper.Hooks;

public static class DecalHooks {

    /// <summary>
    /// Hook to change the position of a decal when applicable for the localization
    /// </summary>
    private static void Hook_ctor(On.Celeste.Decal.orig_ctor_string_Vector2_Vector2_int orig, Decal self, string texture, Vector2 position, Vector2 scale, int depth) {
        string extension = Path.GetExtension(texture);
        string input = TextureTranslator.GetFullKey(Path.Combine("decals/",texture.Replace(extension, "")), GFX.Game);
        Vector2 posDelta = PositionsManager.RetrievePosition(input);
        Vector2 newPosition = position + posDelta;
        orig(self, texture, newPosition, scale, depth);
    }
    public static void Load() {
        On.Celeste.Decal.ctor_string_Vector2_Vector2_int += Hook_ctor;
    }

    public static void Unload() {
        On.Celeste.Decal.ctor_string_Vector2_Vector2_int -= Hook_ctor;
    }
}
