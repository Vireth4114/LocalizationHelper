using System.Collections.Generic;
using Monocle;
using Celeste.Mod.LocalizationHelper.Formats;
using Celeste.Mod.LocalizationHelper.Utils;

namespace Celeste.Mod.LocalizationHelper;

public class TextureTranslator {
    private readonly Dictionary<string, Dictionary<string, string>> textures = [];

    public void AddToTextureMap(LocalizationFile file) {
        if (file.TryDeserialize(out Dictionary<string, Dictionary<string, Dictionary<string, string>>> parsedLanguagesMetadatas)) {
            // If the file contains 3 levels of deepness, we consider they have adopted the metadatas/languages structure and handle the file accordingly
            if (!parsedLanguagesMetadatas.TryGetValue("languages", out Dictionary<string, Dictionary<string, string>> languages)) {
                Logger.Error("LocalizationHelper", 
                    "The \"languages\" key is missing. Probably written with a typo. " + 
                    "Make sure your translated languages are under this key if you're using the format with the metadatas key."
                );
                return;
            }
            foreach (var kv in languages) {
                textures[kv.Key] = ApplyTexturesModifiers(kv.Value, parsedLanguagesMetadatas?.GetValueOrDefault("metadatas"));
            }
        } else if (file.TryDeserialize(out Dictionary<string, Dictionary<string, string>> parsedTextures)) {
            foreach (var kv in parsedTextures) {
                textures[kv.Key] = ApplyTexturesModifiers(kv.Value);
            }
        } else {
            Logger.Error("LocalizationHelper", $"Failed to parse {file.modAsset.PathVirtual}");
        }
    }

    public static Dictionary<string, string> ApplyTexturesModifiers(
        Dictionary<string, string> textures,
        Dictionary<string, Dictionary<string, string>> metadatas = null
    ) {
        Dictionary<string, string> mappedTextures = [];
        foreach (var key in textures.Keys)
        {
            string keyAliased = MetadatasManager.AssociateAliasWithPath(metadatas?.GetValueOrDefault("aliases"), key);
            ParametersManager.ApplyParameters(mappedTextures, keyAliased, textures[key]);
        }
        return mappedTextures;
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

    public string GetLocalizedTexture(string key, Atlas atlas) {
        if (key == null) return null;

        Language lang = Dialog.Language;
        if (lang == null) return key;
        
        string localizedKey = textures?.GetValueOrDefault(lang.Id)?.GetValueOrDefault(GetFullKey(key, atlas));

        return localizedKey != null ? GetShortKey(localizedKey, atlas) : key;
    }

    public string GetOriginalTextureFromLocalized(string localizedKey, Atlas atlas) {
        foreach (var kv in textures.GetValueOrDefault(Dialog.Language.Id) ?? []) {
            if (kv.Value == GetFullKey(localizedKey, atlas)) {
                return GetShortKey(kv.Key, atlas);
            }
        }
        return null;
    }

    public string this[string key, Atlas atlas] {
        get {
            return GetLocalizedTexture(key, atlas);
        }
    }
}