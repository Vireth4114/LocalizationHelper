namespace Celeste.Mod.LocalizationHelper.Hooks;

public static class EverestHooks {
    private static void Hook_OnUpdate(ModAsset prev, ModAsset next) {
        AssetReloadHelper.Do(Dialog.Clean("ASSETRELOADHELPER_RELOADING_LOCALIZATIONTEXTURES"), () => {
            LocalizationHelperModule.Instance.textureTranslator.ReloadLocalizationTextures(next);
        });
    }

    public static void Load() {
        Everest.Content.OnUpdate += Hook_OnUpdate;
    }

    public static void Unload() {
        Everest.Content.OnUpdate -= Hook_OnUpdate;
    }
}