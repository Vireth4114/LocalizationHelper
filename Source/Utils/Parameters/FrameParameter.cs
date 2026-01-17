using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Celeste.Mod.LocalizationHelper.Utils.Parameters;

public partial class FrameParameter: IParameter {

    protected virtual Regex FramePattern => FrameRegex();
    
    [GeneratedRegex(@"\{FRAME:(?<STARTING_FRAME>\d+)\-(?<ENDING_FRAME>\d+)\}")]
    private static partial Regex FrameRegex();

    public bool IsParameterPresent(string key, string value) {
        return FramePattern.IsMatch(key) && FramePattern.IsMatch(value);
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
        Match keyMatch = FramePattern.Match(key);
        Match valueMatch = FramePattern.Match(value);
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
        }
        for (int i = keyBeginningFrame; i <= keyEndingFrame; i++) {
            string number = i.ToString($"D{keyNumberDigits}");
            textures.Add(FramePattern.Replace(key, number), FramePattern.Replace(value, number));
        }
    }
}
