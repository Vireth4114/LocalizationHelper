using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Celeste.Mod.LocalizationHelper.Utils.Parameters;

public partial class PathParameter: IParameter {

    private static readonly Regex pathPattern = PathRegex();

    [GeneratedRegex(@"\{PATH:(?<PATH>\w+)\}")]
    private static partial Regex PathRegex();

    public bool IsParameterPresent(string key, object value) {
        return pathPattern.IsMatch(key) || (value is string valueString && pathPattern.IsMatch(valueString));
    }

    /// <summary>
    /// Process the given key and value to apply the PATH parameter. This modifies textures Dictionary by replacing the PATH of the pair from the metadata
    /// The method will do nothing and add the basic pair (key, value) if there is a parameter mismatch between the key and the value.
    /// That means, for example, that the key has FRAME:00-08 while the value has FRAME:01-07.
    /// </summary>
    /// <param name="textures">The dictionary to update the parameter with</param>
    /// <param name="key">The key to apply the parameter to</param>
    /// <param name="value">The value to apply the parameter to</param>
    public void ApplyParameter(Dictionary<string, object> textures, string key, object value) {
        string newKey = key;
        object newValue = value;

        if (pathPattern.IsMatch(key)) {
            Match keyMatch = pathPattern.Match(key);
            string keyPath = MetadatasManager.RetrievePathValue(keyMatch.Groups[1].Value);
            newKey = pathPattern.Replace(key, keyPath);
        }

        if (value is string valueString && pathPattern.IsMatch(valueString)) {
            Match valueMatch = pathPattern.Match(valueString);
            string valuePath = MetadatasManager.RetrievePathValue(valueMatch.Groups[1].Value);
            newValue = pathPattern.Replace(valueString, valuePath);
        }

        textures.Add(newKey, newValue);
    }
}