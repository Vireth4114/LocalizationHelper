using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Celeste.Mod.LocalizationHelper.Utils.Parameters;

public partial class PathParameter: IParameter {

    private static readonly Regex pathPattern = PathRegex();

    [GeneratedRegex(@"\{PATH:(?<PATH>\w+)\}")]
    private static partial Regex PathRegex();

    public bool IsParameterPresent(string key, string value) {
        return pathPattern.IsMatch(key) || pathPattern.IsMatch(value);
    }

    /// <summary>
    /// Process the given key and value to apply the FRAME parameter. This modify the textures Dictionary by adding as much key as there is
    /// frame needed. For example, if the parameter if FRAME:00-08, it will add 9 keys ranging from frame 00 to 08.
    /// The method will do nothing and add the basic pair (key, value) if there is a parameter mismatch between the key and the value.
    /// That means, for example, that the key has FRAME:00-08 while the value has FRAME:01-07.
    /// </summary>
    /// <param name="textures">The textures dictionary to update the parameter with</param>
    /// <param name="key">The key to apply the parameter to</param>
    /// <param name="value">The value to apply the parameter to</param>
    public void ApplyParameter(Dictionary<string, string> textures, string key, string value) {
        Match keyMatch = pathPattern.Match(key);
        Match valueMatch = pathPattern.Match(value);
        string keyPath = MetadatasManager.RetrievePathValue(keyMatch.Groups[1].Value);
        string valuePath = MetadatasManager.RetrievePathValue(valueMatch.Groups[1].Value);
        textures.Add(pathPattern.Replace(key, keyPath), pathPattern.Replace(value, valuePath));
    }
}