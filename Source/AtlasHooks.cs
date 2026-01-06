using System;
using System.Collections.Generic;
using System.Reflection;
using Monocle;
using MonoMod.RuntimeDetour;

namespace Celeste.Mod.LocalizationHelper;

public static class AtlasHooks {
    private static Hook hook_AtlasGetItem;

    private static List<MTexture> Hook_GetAtlasSubtextures(On.Monocle.Atlas.orig_GetAtlasSubtextures orig, Atlas self, string key) {
        return orig(self, LocalizationHelperModule.Instance.textureTranslator[key, self]);
    }

    private static MTexture Hook_GetOrDefault(On.Monocle.Atlas.orig_GetOrDefault orig, Atlas self, string key, MTexture defaultTexture) {
        return orig(self, LocalizationHelperModule.Instance.textureTranslator[key, self], defaultTexture);
    }

    private static MTexture Hook_GetAtlasSubtextureFromAtlasAt(On.Monocle.Atlas.orig_GetAtlasSubtextureFromAtlasAt orig, Atlas self, string key, int index) {
        // Probably better way to do with IL Hooks but went with basically copy-pasting the code for now, surely no one is hooking that
        TextureTranslator translator = LocalizationHelperModule.Instance.textureTranslator;
        string localizedKey = translator[key, self];
        if (index == 0 && self.textures.TryGetValue(localizedKey, out MTexture value)) {
            return value;
        }
        string text = index.ToString();
        int length = text.Length;
        while (text.Length < length + 6) {
            localizedKey = translator[key + text, self];
            if (self.textures.TryGetValue(localizedKey, out MTexture result)) {
                return result;
            }
            text = "0" + text;
        }
        return null;
    }

    private static MTexture Hook_GetLinkedTexture(On.Monocle.Atlas.orig_GetLinkedTexture orig, Atlas self, string key) {
        string origKey = LocalizationHelperModule.Instance.textureTranslator.GetOriginalTextureFromLocalized(key, self);
        return orig(self, origKey ?? key);
    }
    
    private static MTexture Hook_GetItem(Func<Atlas, string, MTexture> orig, Atlas self, string key) {
        return orig(self, LocalizationHelperModule.Instance.textureTranslator[key, self]);
    }

    public static void Load() {
        On.Monocle.Atlas.GetAtlasSubtextures += Hook_GetAtlasSubtextures;
        On.Monocle.Atlas.GetOrDefault += Hook_GetOrDefault;
        On.Monocle.Atlas.GetAtlasSubtextureFromAtlasAt += Hook_GetAtlasSubtextureFromAtlasAt;
        On.Monocle.Atlas.GetLinkedTexture += Hook_GetLinkedTexture;
        CreateGetItemHook();
    }

    public static void Unload() {
        On.Monocle.Atlas.GetAtlasSubtextures -= Hook_GetAtlasSubtextures;
        On.Monocle.Atlas.GetOrDefault -= Hook_GetOrDefault;
        On.Monocle.Atlas.GetAtlasSubtextureFromAtlasAt -= Hook_GetAtlasSubtextureFromAtlasAt;
        On.Monocle.Atlas.GetLinkedTexture -= Hook_GetLinkedTexture;
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