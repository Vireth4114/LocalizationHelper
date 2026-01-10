
using System.Collections.Generic;
using System.Text.Json;

namespace Celeste.Mod.LocalizationHelper.Format;
public class JsonLocalizationFile(ModAsset modAsset) : LocalizationFile(modAsset) {
    public override bool TryDeserialize(out Dictionary<string, Dictionary<string, string>> result) {
        try {
            result = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(modAsset.Stream);
        } catch (JsonException) {
            result = null;
            return false;
        }
        return true;
    }
}