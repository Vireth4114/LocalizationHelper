using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using Monocle;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace Celeste.Mod.LocalizationHelper;

public static class LanguageHooks {
    private static void Hook_ApplyLanguage(On.Celeste.Settings.orig_ApplyLanguage orig, Settings self) {
        GFX.Game.ResetCaches();
        GFX.Gui.ResetCaches();
        GFX.Opening.ResetCaches();
        GFX.Misc.ResetCaches();
        GFX.Portraits.ResetCaches();
        GFX.ColorGrades.ResetCaches();
        orig(self);
    }

    public static void Load() {
        On.Celeste.Settings.ApplyLanguage += Hook_ApplyLanguage;
    }

    public static void Unload() {
        On.Celeste.Settings.ApplyLanguage -= Hook_ApplyLanguage;
    }
}