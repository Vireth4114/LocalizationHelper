using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using Monocle;
using MonoMod.Cil;
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

    private static void Hook_GetAtlasSubtextureFromAtlasAt(ILContext il) {
        ILCursor cursor = new(il);
        
        while (cursor.TryGotoNext(
            MoveType.Before,
            instr => instr.MatchCallvirt<Dictionary<string, MTexture>>("ContainsKey")
                    || instr.MatchCallvirt<Dictionary<string, MTexture>>("get_Item")
        )) {
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate(LocalizationHelperModule.Instance.textureTranslator.GetLocalizedTexture);
            cursor.Index++;
        }

        cursor.Index = 0;
        while (cursor.TryGotoNext(
            MoveType.After,
            instr => instr.MatchCall<string>("Concat")
        )) {
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate(LocalizationHelperModule.Instance.textureTranslator.GetLocalizedTexture);
        }
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
        IL.Monocle.Atlas.GetAtlasSubtextureFromAtlasAt += Hook_GetAtlasSubtextureFromAtlasAt;
        On.Monocle.Atlas.GetLinkedTexture += Hook_GetLinkedTexture;
        CreateGetItemHook();
    }

    public static void Unload() {
        On.Monocle.Atlas.GetAtlasSubtextures -= Hook_GetAtlasSubtextures;
        On.Monocle.Atlas.GetOrDefault -= Hook_GetOrDefault;
        IL.Monocle.Atlas.GetAtlasSubtextureFromAtlasAt -= Hook_GetAtlasSubtextureFromAtlasAt;
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