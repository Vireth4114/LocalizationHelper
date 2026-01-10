using System.Collections.Generic;

namespace Celeste.Mod.LocalizationHelper.Utils;

public class MetadatasManager {
    /// <summary>
    /// This method return if the aliases key is present in the parsed file or not.
    /// </summary>
    /// <param name="parsedFile"></param>
    /// <returns>true if "aliases" is present under "metadatas", false otherwise</returns>
    public static bool IsAliasPresent(Dictionary<string, Dictionary<string, Dictionary<string, string>>> parsedFile)
    {
        return parsedFile?.GetValueOrDefault("metadatas")?.GetValueOrDefault("aliases") != null;
    }

    /// <summary>
    /// This method associate the path from an alias under metadatas/aliases and use it as the actual key
    /// in the textures to be able to retrieve it correctly.
    /// </summary>
    /// <param name="aliases">The Dictionnary mapping an alias to a texture path</param>
    /// <param name="languagesToAssetsKV">The key-value pair associating a language to a list of originalTexturePath to localizedTexturePath</param>
    /// <param name="textures">The stored textures in TextureTranslator associated with a language key</param>
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