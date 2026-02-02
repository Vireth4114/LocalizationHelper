using System;
using System.Reflection;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace Celeste.Mod.LocalizationHelper.Hooks;

public static class EmojiHooks {
    private static ILHook hook_EmojiApply;

    /// <summary>
    /// Hook to change the emoji texture id when applicable for the localization
    /// existing code: Emoji._IDs.TryGetValue(key, out int num)
    /// replaced by: Emoji._IDs.TryGetValue(GetLocalizedEmoji(key), out int num)
    /// </summary>
    /// <param name="ctx">IL context</param>
    private static void Hook_EmojiApply(ILContext ctx) {
        var cursor = new ILCursor(ctx);

        FieldInfo idsField = typeof(Emoji)
            .GetField("_IDs", BindingFlags.Static | BindingFlags.NonPublic);

        if (cursor.TryGotoNext(
            MoveType.After,
            i => i.MatchLdsfld(idsField),
            i => i.MatchLdloc(out int keyIndex)
        )) {
            cursor.EmitDelegate(LocalizationHelperModule.Instance.textureTranslator.GetLocalizedEmoji);
        }
    }

    public static void Load() {
        Type emojiType = typeof(Emoji);

        Type cacheType = emojiType.GetNestedType(
            "CachedApply",
            BindingFlags.NonPublic
        );

        MethodInfo target = cacheType?.GetMethod(
            "Compute",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (target == null) {
            Logger.Error("LocalizationHelper", "Could not find Emoji.CachedApply.Compute method for hooking.");
            return;
        }

        hook_EmojiApply = new ILHook(
            target,
            Hook_EmojiApply
        );
    }

    public static void Unload() {
        hook_EmojiApply?.Dispose();
        hook_EmojiApply = null;
    }
}
