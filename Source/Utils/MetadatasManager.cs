using System.Collections.Generic;

namespace Celeste.Mod.LocalizationHelper.Utils;

public class MetadatasManager {

    private static Dictionary<string, Dictionary<string, string>> metadatas = null;

    public static void SetMetadatas(Dictionary<string, Dictionary<string, string>> givenMetadata) {
        metadatas = givenMetadata;
    }

    /// <summary>
    /// This method associate the path from an alias under metadatas/aliases and return the actual path.
    /// If no alias found, return the given path.
    /// </summary>
    /// <param name="path">The path we want to check</param>
    public static string AssociateAliasWithPath(string path) {
        return metadatas?.GetValueOrDefault("aliases")?.GetValueOrDefault(path) ?? path;
    }

    /// <summary>
    /// Retrieve the path wanted for the given keyname.
    /// If no path found, return the given keyname.
    /// </summary>
    /// <param name="keyname">The keyname we want to check</param>
    public static string RetrievePathValue(string keyname) {
        // Note: we accept code duplication here to ensure separation of roles and the duplication isn't big enough to be meaningful
        return metadatas?.GetValueOrDefault("paths")?.GetValueOrDefault(keyname) ?? keyname;
    }
}
