using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Celeste.Mod.LocalizationHelper.Utils.Parameters;

public partial class FrameParameter: IParameter {

    protected virtual Regex FramePattern => FrameRegex();
    
    [GeneratedRegex(@"\{FRAME:(?<STARTING_FRAME>\d+)\-(?<ENDING_FRAME>\d+)\}")]
    private static partial Regex FrameRegex();

    public record FrameRange(int BeginningFrame, int EndingFrame, int NumberDigits);
    public FrameRange GetFrameRange(string value) {
        Match keyMatch = FramePattern.Match(value);
        return new FrameRange(
            int.Parse(keyMatch.Groups[1].Value),
            int.Parse(keyMatch.Groups[2].Value),
            keyMatch.Groups[2].Value.Length
        );
    }

    public bool IsParameterPresent(string key, object value) {
        return FramePattern.IsMatch(key) && (value is not string valueString || FramePattern.IsMatch(valueString));
    }

    /// <summary>
    /// Process the given key and value to apply the FRAME parameter. This modifies the textures Dictionary by adding as much key as there is
    /// frame needed. For example, if the parameter is FRAME:00-08, it will add 9 keys ranging from frame 00 to 08.
    /// The method will do nothing and add the basic pair (key, value) if the value is a string and there is a parameter mismatch between the key and the value.
    /// That means, for example, that the key has FRAME:00-08 while the value has FRAME:01-07.
    /// </summary>
    /// <param name="textures">The textures dictionary to update the parameter with</param>
    /// <param name="key">The key to apply the parameter to</param>
    /// <param name="value">If value is a string, the value to apply the parameter to</param>
    public void ApplyParameter(Dictionary<string, object> textures, string key, object value) {
        if (value is string valueString) {
            ApplyParameterWithStringValue(textures, key, valueString);
        } else {
            ApplyParameterWithNonStringValue(textures, key, value);
        }
    }

    private void ApplyParameterWithStringValue(Dictionary<string, object> textures, string key, string value) {
        FrameRange keyRange = GetFrameRange(key);
        FrameRange valueRange = GetFrameRange(value);
        if (keyRange != valueRange) {
            Logger.Error("LocalizationHelper",
                "It seems some values doesn't match. " +
                "Key: " + keyRange.BeginningFrame + " -> " + keyRange.EndingFrame +
                " | Value: " + valueRange.BeginningFrame + " -> " + valueRange.EndingFrame +
                " | leading zeros, key: " + keyRange.NumberDigits + " value: " + valueRange.NumberDigits
            );
            textures.Add(key, value);
        }
        for (int i = keyRange.BeginningFrame; i <= keyRange.EndingFrame; i++) {
            string number = i.ToString($"D{keyRange.NumberDigits}");
            textures.Add(FramePattern.Replace(key, number), FramePattern.Replace(value, number));
        }
    }

    private void ApplyParameterWithNonStringValue(Dictionary<string, object> textures, string key, object value) {
        FrameRange keyRange = GetFrameRange(key);
        for (int i = keyRange.BeginningFrame; i <= keyRange.EndingFrame; i++) {
            string number = i.ToString($"D{keyRange.NumberDigits}");
            textures.Add(FramePattern.Replace(key, number), value);
        }
    }
}
