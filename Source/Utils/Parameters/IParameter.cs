using System.Collections.Generic;

namespace Celeste.Mod.LocalizationHelper.Utils.Parameters;

interface IParameter {
    /// <summary>
    /// Verify if the given parameter is present in the given strings.
    /// </summary>
    /// <param name="key">The key to test against</param>
    /// <param name="value">If the value is a string, the value to test against, else doesn't test anything</param>
    /// <returns></returns>
    public bool IsParameterPresent(string key, object value);

    /// <summary>
    /// Process the given key and value to apply the parameter. If you want more context about a specific parameter, check its dedicated class.
    /// This method changes the Dictionary.
    /// </summary>
    /// <param name="textures">The dictionary to update the parameter with</param>
    /// <param name="key">The key to apply the parameter to</param>
    /// <param name="value">If the value is a string to apply the parameter to, if it is a string, else doesn't apply anything</param>
    public void ApplyParameter(Dictionary<string, object> textures, string key, object value);
}
