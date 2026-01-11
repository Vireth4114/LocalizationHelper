using Celeste.Mod.LocalizationHelper.Formats;

namespace Celeste.Mod.LocalizationHelper;

public class LocalizationHelperModule : EverestModule {
    public static LocalizationHelperModule Instance { get; private set; }
    public TextureTranslator textureTranslator = new();

    public LocalizationHelperModule() {
        Instance = this;
    }

    public override void Load() {
        AtlasHooks.Load();
        LanguageHooks.Load();
    }

    public override void Unload() {
        AtlasHooks.Unload();
        LanguageHooks.Unload();
    }

    public override void Initialize() {
        base.Initialize();

        foreach (ModContent mod in Everest.Content.Mods) {
            if (mod.Map.TryGetValue("LocalizationTextures", out ModAsset asset)) {
                textureTranslator.AddToTextureMap(new LocalizationFile(asset));
            } else if (mod.Map.TryGetValue("LocalizationTextures.json", out ModAsset jsonAsset)) {
                textureTranslator.AddToTextureMap(new JsonLocalizationFile(jsonAsset));
            }
        }
    }
}