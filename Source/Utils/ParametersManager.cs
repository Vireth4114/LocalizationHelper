using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Celeste.Mod.LocalizationHelper.Utils;

public partial class ParametersManager {

    private static readonly Regex framePattern = FrameRegex();
    
    [GeneratedRegex(@"\{FRAME:(?<STARTING_FRAME>\d*)\-(?<ENDING_FRAME>\d*)\}")]
    private static partial Regex FrameRegex();
    
    private static bool IsParameterPresent(string parameter, string key, string value) {
        Regex pattern = null;
        if (parameter.Equals("FRAME")) {
            pattern = framePattern;
        }
        return pattern.IsMatch(key) && pattern.IsMatch(value);
    }

    private static void ApplyFrameParameter(Dictionary<string, string> textures, string key, string value) {
        Match keyMatch = framePattern.Match(key);
        Match valueMatch = framePattern.Match(value);
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
            textures.Add(framePattern.Replace(key, number), framePattern.Replace(value, number));
        }
    }

    public static void ApplyParameters(Dictionary<string, string> textures, string key, string value) {
        if (IsParameterPresent("FRAME", key, value)) {
            ApplyFrameParameter(textures, key, value);
        } else {
            textures.Add(key, value);
        }
    }
}