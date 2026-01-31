using System;
using System.Collections.Generic;
using Celeste.Mod.LocalizationHelper.Utils.Parameters;

namespace Celeste.Mod.LocalizationHelper.Utils;

public partial class ParametersManager {
    /// <summary>
    /// Retrieve a dictionary associating the name of a parameter with an instance of its class.
    /// </summary>
    /// <returns>A dictionary mapping a parameter's name with an instance of its class.</returns>
    private static Dictionary<string, IParameter> GetAllParameters() {
        Dictionary<string, IParameter> parameters = [];
        parameters.Add("PATH", new PathParameter());
        parameters.Add("FRAME", new FrameParameter());
        parameters.Add("NUMBER", new NumberParameter());
        return parameters;
    }

    /// <summary>
    /// This method apply all available parameters to the given key and value.
    /// </summary>
    /// <param name="key">The key to apply the parameters to</param>
    /// <param name="value">The value to apply the parameters to</param>
    public static Dictionary<string, string> ApplyParameters(string key, string value) {
        Dictionary<string, string> textures = new(StringComparer.OrdinalIgnoreCase){
            { key, value }
        };
        foreach (var parameter in GetAllParameters()) {
            IParameter parameterInstance = parameter.Value;
            Dictionary<string, string> newlyGeneratedTextures = new(StringComparer.OrdinalIgnoreCase);            
            foreach (var texture in textures) {
                if (parameterInstance.IsParameterPresent(texture.Key, texture.Value)) {
                    parameterInstance.ApplyParameter(newlyGeneratedTextures, texture.Key, texture.Value);
                } else {
                    newlyGeneratedTextures.Add(texture.Key, texture.Value);
                }
            }
            textures = newlyGeneratedTextures;
        }
        return textures;
    }
}
