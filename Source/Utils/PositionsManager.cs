using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.LocalizationHelper.Utils;

public class PositionsManager {

    private readonly static Dictionary<string, Dictionary<string, Vector2>> positions = [];

    /// <summary>
    /// Method to update the association asset => position.
    /// </summary>
    /// <param name="givenPosition">The positions to save</param>
    public static void SetPositions(Dictionary<string, Dictionary<string, string>> givenPosition) {
        foreach (var language in givenPosition) {
            if (!positions.TryGetValue(language.Key, out Dictionary<string, Vector2> value)) {
                positions[language.Key] = [];
            }
            foreach (var positionsMapping in language.Value)
            {
                string[] values = positionsMapping.Value.Split(",");
                try {
                    int x = int.Parse(values[0]);
                    int y = int.Parse(values[1]);
                    positions[language.Key][positionsMapping.Key] = new Vector2(x, y);
                } catch {
                    Logger.Error("LocalizationHelper", $"Could not process position for asset {positionsMapping.Key}. Was expecting asset: x,y format.");
                }
            }
        }
    }

    /// <summary>
    /// Retrieve the position wanted for the given keyname.
    /// If no position found, return a Vector2.Zero.
    /// </summary>
    /// <param name="keyname">The keyname we want to check</param>
    public static Vector2 RetrievePosition(string keyname) {
        Language lang = Dialog.Language;
        if (lang == null) return Vector2.Zero;
        return positions?.GetValueOrDefault(lang.Id)?.GetValueOrDefault(keyname) ?? Vector2.Zero;
    }
}
