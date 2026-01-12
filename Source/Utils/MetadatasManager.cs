using System.Collections.Generic;

namespace Celeste.Mod.LocalizationHelper.Utils;

public class MetadatasManager {

    /// <summary>
    /// This method associate the path from an alias under metadatas/aliases and return the actual path.
    /// If no alias found, return the given path.
    /// </summary>
    /// <param name="aliases">The Dictionnary mapping an alias to a texture path. Can be null</param>
    /// <param name="path">The path we want to check</param>
    public static string AssociateAliasWithPath(
        Dictionary<string, string> aliases,
        string path
    ) {
        return aliases?.GetValueOrDefault(path) ?? path;
    }
}
