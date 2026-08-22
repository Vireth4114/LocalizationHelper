using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Celeste.Mod.LocalizationHelper.Utils;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace Celeste.Mod.LocalizationHelper.Hooks;

public static class ImageHooks {
    private static readonly List<ILHook> Hooks = [];

    private static void HookDraw(ILContext il) {
        ILCursor cursor = new(il);
        
        if (cursor.TryGotoNext(MoveType.Before, i => i.MatchLdarg(1))) {
            cursor.EmitLdarg(0);
            cursor.EmitLdarg(1);
            cursor.EmitDelegate(GetNewPosition);
            cursor.EmitStarg(1);
        }
    }

    private static Vector2 GetNewPosition(MTexture self, Vector2 position) {
        return position + PositionsManager.RetrievePosition(self.Texture.Name);
    }

    public static void Load() {
        MethodInfo[] methods = typeof(MTexture).GetMethods(BindingFlags.Public | BindingFlags.Instance);

        foreach (MethodInfo method in methods.Where(m => m.Name.StartsWith("Draw", StringComparison.Ordinal)))
            Hooks.Add(new ILHook(method, HookDraw));
    }

    public static void Unload() {
        foreach (ILHook hook in Hooks)
            hook.Dispose();
        Hooks.Clear();
    }
}