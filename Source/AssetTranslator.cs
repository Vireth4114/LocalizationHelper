using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Celeste.Mod.LocalizationHelper;

public class AssetTranslator {
    private readonly Dictionary<string, Dictionary<string, string>> textures = [];

    public void AddJsonToTextureMap(ModAsset asset) {
        if (asset == null) {
            return;
        }

        string json;
        using (var reader = new StreamReader(asset.Stream)) {
            json = reader.ReadToEnd();
        }

        try {
            var parsedAssets = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
            foreach (var kv in parsedAssets) {
                textures[kv.Key] = kv.Value;
            }
        } catch (JsonException e) {
            Logger.Error("LocalizationHelper", $"Failed to parse {asset.PathVirtual}: {e}");
        }
    }

    public string this[string key] {
        get {
            return textures?.GetValueOrDefault(key)?.GetValueOrDefault(Dialog.Language.Id) ?? key;
        }
    }
}