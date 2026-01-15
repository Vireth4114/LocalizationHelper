using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Celeste.Mod.LocalizationHelper.Utils;

public partial class ParametersManager {

    private static readonly Regex numberPattern = NumberRegex();
    private static readonly Regex framePattern = FrameRegex();
    
    [GeneratedRegex(@"\{FRAME:(?<STARTING_FRAME>\d+)\-(?<ENDING_FRAME>\d+)\}")]
    private static partial Regex FrameRegex();
    
    [GeneratedRegex(@"\{NUMBER:(?<STARTING_NUMBER>\d+)\-(?<ENDING_NUMBER>\d+)\}")]
    private static partial Regex NumberRegex();
    
    /// <summary>
    /// Verify if the given parameter is present in the given strings.
    /// The parameter has to be present in the key AND the value to be valid.
    /// </summary>
    /// <param name="parameter">The parameter to check</param>
    /// <param name="key">The key to test against</param>
    /// <param name="value">The value to test against</param>
    /// <returns></returns>
    private static bool IsParameterPresent(string parameter, string key, string value) {
        Regex pattern = null;
        if (parameter.Equals("FRAME")) {
            pattern = framePattern;
        } else if (parameter.Equals("NUMBER"))
        {
            pattern = numberPattern;
        }
        return pattern.IsMatch(key) && pattern.IsMatch(value);
    }

    /// <summary>
    /// Process the given key and value to apply the FRAME parameter. This modify the textures Dictionnary by adding as much key as there is
    /// frame needed. For example, if the parameter if FRAME:00-08, it will add 9 keys ranging from frame 00 to 08.
    /// The method will do nothing and add the basic pair (key, value) if there is a parameter mismatch between the key and the value.
    /// That means, for example, that the key has FRAME:00-08 while the value has FRAME:01-07.
    /// </summary>
    /// <param name="textures">The textures dictionnary to update the parameter with</param>
    /// <param name="key">The key to apply the parameter to</param>
    /// <param name="value">The value to apply the parameter to</param>
    private static void ApplyFrameParameter(Dictionary<string, string> textures, string key, string value, Regex pattern) {
        Match keyMatch = pattern.Match(key);
        Match valueMatch = pattern.Match(value);
        int keyBeginningFrame = int.Parse(keyMatch.Groups[1].Value);
        int keyEndingFrame = int.Parse(keyMatch.Groups[2].Value);
        int keyNumberDigits = keyMatch.Groups[2].Value.Length;
        int valueBeginningFrame = int.Parse(valueMatch.Groups[1].Value);
        int valueEndingFrame = int.Parse(valueMatch.Groups[2].Value);
        int valueNumberDigits = valueMatch.Groups[2].Value.Length;
        if (keyBeginningFrame != valueBeginningFrame || keyEndingFrame != valueEndingFrame || keyNumberDigits != valueNumberDigits) {
            Logger.Error("LocalizationHelper",
                "It seems some values doesn't match. " +
                "Key: " + keyBeginningFrame + " -> " + keyEndingFrame +
                " | Value: " + valueBeginningFrame + " -> " + valueEndingFrame +
                " | leading zeros, key: " + keyNumberDigits + " value: " + valueNumberDigits
            );
            textures.Add(key, value);
            return;
        }
        for (int i = keyBeginningFrame; i <= keyEndingFrame; i++) {
            string number = i.ToString($"D{keyNumberDigits}");
            textures.Add(pattern.Replace(key, number), pattern.Replace(value, number));
        }
    }

    /// <summary>
    /// This method apply all available parameters to the given key and value. This modify the textures dictionnary.
    /// </summary>
    /// <param name="textures">The textures dictionnary to modify</param>
    /// <param name="key">The key to apply the parameters to</param>
    /// <param name="value">The value to apply the parameters to</param>
    public static void ApplyParameters(Dictionary<string, string> textures, string key, string value) {
        if (IsParameterPresent("FRAME", key, value)) {
            ApplyFrameParameter(textures, key, value, framePattern);
        } else if (IsParameterPresent("NUMBER", key, value)) {
            ApplyFrameParameter(textures, key, value, numberPattern);
        } else {
            textures.Add(key, value);
        }
    }
}
