using System;
using System.Collections.Generic;
using System.Reflection;
using Monocle;
using MonoMod.RuntimeDetour;

namespace Celeste.Mod.LocalizationHelper;

public static class AtlasHooks {
    private static Hook hook_AtlasGetItem;

    private static List<MTexture> Hook_GetAtlasSubtextures(On.Monocle.Atlas.orig_GetAtlasSubtextures orig, Atlas self, string key) {
        return orig(self, LocalizationHelperModule.Instance.assetTranslator[key]);
    }
    
    private static MTexture Hook_GetItem(Func<Atlas, string, MTexture> orig, Atlas self, string key) {
        return orig(self, LocalizationHelperModule.Instance.assetTranslator[key]);
    }

    public static void Load() {
        On.Monocle.Atlas.GetAtlasSubtextures += Hook_GetAtlasSubtextures;
        CreateGetItemHook();
    }

    public static void Unload() {
        On.Monocle.Atlas.GetAtlasSubtextures -= Hook_GetAtlasSubtextures;
        hook_AtlasGetItem?.Dispose();
        hook_AtlasGetItem = null;
    }

    private static void CreateGetItemHook() {
        Type atlasType = typeof(Atlas);

        PropertyInfo target = atlasType.GetProperty(
            name: "Item",
            bindingAttr: BindingFlags.Public | BindingFlags.Instance,
            binder: null,
            returnType: typeof(MTexture),
            types: [typeof(string)],
            modifiers: null
        );

        hook_AtlasGetItem = new Hook(
            target.GetGetMethod(),
            typeof(AtlasHooks).GetMethod(
                nameof(Hook_GetItem),
                BindingFlags.NonPublic | BindingFlags.Static
            )
        );
    }
}