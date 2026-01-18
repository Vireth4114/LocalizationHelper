namespace Celeste.Mod.LocalizationHelper.Hooks;

public static class LanguageHooks {
    private static void Hook_ApplyLanguage(On.Celeste.Settings.orig_ApplyLanguage orig, Settings self) {
        TextureTranslator.ResetAllAtlasCaches();
        orig(self);
    }

    public static void Load() {
        On.Celeste.Settings.ApplyLanguage += Hook_ApplyLanguage;
    }

    public static void Unload() {
        On.Celeste.Settings.ApplyLanguage -= Hook_ApplyLanguage;
    }
}