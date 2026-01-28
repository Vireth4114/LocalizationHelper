using System.Collections.Generic;
using Celeste.Mod.LocalizationHelper.Formats;

namespace Celeste.Mod.LocalizationHelper.Hooks;

public static class EverestHooks {
    private static void Hook_OnUpdate(ModAsset prev, ModAsset next) {
        AssetReloadHelper.Do(Dialog.Clean("ASSETRELOADHELPER_RELOADING_LOCALIZATIONTEXTURES"), () => {
            List<LocalizationFile> assets = [];
            foreach (ModContent mod in Everest.Content.Mods) {
                if (mod.Map.TryGetValue("LocalizationTextures", out ModAsset asset)) {
                    assets.Add(new LocalizationFile(asset));
                } else if (mod.Map.TryGetValue("LocalizationTextures.json", out ModAsset jsonAsset)) {
                    assets.Add(new JsonLocalizationFile(jsonAsset));
                }
            }
            LocalizationHelperModule.Instance.textureTranslator.ReloadLocalizationTextures(assets);
        });
    }

    public static void Load() {
        Everest.Content.OnUpdate += Hook_OnUpdate;
    }

    public static void Unload() {
        Everest.Content.OnUpdate -= Hook_OnUpdate;
    }
}