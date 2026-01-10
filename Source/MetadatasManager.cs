using System.Collections.Generic;

namespace Celeste.Mod.LocalizationHelper;

public class MetadatasManager {
    public static bool IsAliasPresent(Dictionary<string, Dictionary<string, Dictionary<string, string>>> parsedFile)
    {
        return parsedFile?.GetValueOrDefault("metadatas")?.GetValueOrDefault("aliases") != null;
    }

    public static void AssociateAliasWithPath(
        Dictionary<string, string> aliases,
        KeyValuePair<string, Dictionary<string, string>> languagesToAssetsKV,
        Dictionary<string, Dictionary<string, string>> textures
    ) {
        Dictionary<string, string> mappedTextures = [];
        foreach (var textureDict in languagesToAssetsKV.Value) {
            if (aliases.TryGetValue(textureDict.Key, out string aliasedPath)) {
                mappedTextures.Add(aliasedPath, textureDict.Value);
            } else {
                mappedTextures.Add(textureDict.Key, textureDict.Value);
            }
        }
        textures[languagesToAssetsKV.Key] = mappedTextures;
    }
}