using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
                positions[language.Key] = new Dictionary<string, Vector2>(StringComparer.OrdinalIgnoreCase);
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
            positions[language.Key] = TextureTranslator.ApplyTexturesModifiers(positions[language.Key]);
        }
    }

    /// <summary>
    /// Clear all saved positions.
    /// </summary>
    public static void ClearPositions() {
        positions.Clear();
    }

    /// <summary>
    /// Retrieve the position wanted for the given texture.
    /// If no position found, return a Vector2.Zero.
    /// </summary>
    /// <param name="texture">The texture we want to check, must be a full path</param>
    public static Vector2 RetrievePosition(string texture) {
        if (string.IsNullOrEmpty(texture) || Dialog.Language is not { } lang)
            return Vector2.Zero;
        if (texture.Contains("idle"))
        {
            Logger.Info("l", texture);
        }
        return positions?.GetValueOrDefault(lang.Id)?.TryGetValue(texture, out Vector2 position) == true
            ? position
            : Vector2.Zero;
    }
}
