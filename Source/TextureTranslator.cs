using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Monocle;

namespace Celeste.Mod.LocalizationHelper;

public class TextureTranslator {
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
            var parsedTextures = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
            foreach (var kv in parsedTextures) {
                textures[kv.Key] = kv.Value;
            }
        } catch (JsonException e) {
            Logger.Error("LocalizationHelper", $"Failed to parse {asset.PathVirtual}: {e}");
        }
    }

    public static string GetFullKey(string key, Atlas atlas) {
        string textureFolder = atlas.DataPath.Replace('\\', '/') + "/";
        return textureFolder + key;
    }

    public static string GetShortKey(string fullKey, Atlas atlas) {
        string textureFolder = atlas.DataPath.Replace('\\', '/') + "/";
        if (fullKey.StartsWith(textureFolder)) {
            return fullKey[textureFolder.Length..];
        }
        return fullKey;
    }

    public string GetOriginalTextureFromLocalized(string localizedKey, Atlas atlas) {
        foreach (var kv in textures) {
            if (kv.Value.GetValueOrDefault(Dialog.Language.Id) == GetFullKey(localizedKey, atlas)) {
                return GetShortKey(kv.Key, atlas);
            }
        }
        return null;
    }

    public string this[string key, Atlas atlas] {
        get {
            if (key == null) return null;

            string localizedKey = textures?.GetValueOrDefault(GetFullKey(key, atlas))?.GetValueOrDefault(Dialog.Language.Id);

            return localizedKey != null ? GetShortKey(localizedKey, atlas) : key;
        }
    }
}