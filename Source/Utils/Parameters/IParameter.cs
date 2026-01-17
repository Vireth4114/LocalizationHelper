using System;
using System.Collections.Generic;

namespace Celeste.Mod.LocalizationHelper.Utils.Parameters;

interface IParameter {
    /// <summary>
    /// Verify if the given parameter is present in the given strings.
    /// The parameter has to be present in the key AND the value to be valid.
    /// </summary>
    /// <param name="parameter">The parameter to check</param>
    /// <param name="key">The key to test against</param>
    /// <param name="value">The value to test against</param>
    /// <returns></returns>
    public bool IsParameterPresent(string key, string value);

    /// <summary>
    /// Process the given key and value to apply the parameter. If you want more context about a specific parameter, check its dedicated class.
    /// This method changes the textures Dictionary.
    /// </summary>
    /// <param name="textures">The textures dictionary to update the parameter with</param>
    /// <param name="key">The key to apply the parameter to</param>
    /// <param name="value">The value to apply the parameter to</param>
    public void ApplyParameter(Dictionary<string, string> textures, string key, string value);
}
