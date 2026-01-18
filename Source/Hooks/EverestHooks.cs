using Celeste.Mod.LocalizationHelper.Formats;

namespace Celeste.Mod.LocalizationHelper.Hooks;

public static class EverestHooks {
    private static void Hook_OnUpdate(ModAsset prev, ModAsset next) {
        LocalizationFile asset;
        if (next.PathVirtual == "LocalizationTextures") {
            asset = new LocalizationFile(next);
        } else if (next.PathVirtual == "LocalizationTextures.json") {
            asset = new JsonLocalizationFile(next);
        } else {
            return;
        }

        AssetReloadHelper.Do(Dialog.Clean("ASSETRELOADHELPER_RELOADING_LOCALIZATIONTEXTURES"), () => {
            LocalizationHelperModule.Instance.textureTranslator.ReloadLocalizationTextures(asset);
        });
    }

    public static void Load() {
        Everest.Content.OnUpdate += Hook_OnUpdate;
    }

    public static void Unload() {
        Everest.Content.OnUpdate -= Hook_OnUpdate;
    }
}