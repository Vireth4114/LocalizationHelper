using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using Monocle;
using MonoMod.RuntimeDetour;

namespace Celeste.Mod.LocalizationHelper;

public class LocalizationHelperModule : EverestModule {
    public static LocalizationHelperModule Instance { get; private set; }
    public AssetTranslator assetTranslator = new();

    public LocalizationHelperModule() {
        Instance = this;
    }

    public override void Load() {
        AtlasHooks.Load();
    }

    public override void Unload() {
        AtlasHooks.Unload();
    }

    public override void Initialize() {
        base.Initialize();

        foreach (ModContent mod in Everest.Content.Mods) {
            if (mod.Map.TryGetValue("LocalizationAssets.json", out ModAsset asset)) {
                assetTranslator.AddJsonToTextureMap(asset);
            }
        }
    }
}