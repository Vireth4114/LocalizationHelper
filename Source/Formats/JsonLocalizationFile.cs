using System.Text.Json;

namespace Celeste.Mod.LocalizationHelper.Formats;
public class JsonLocalizationFile(ModAsset modAsset) : LocalizationFile(modAsset) {
    public override bool TryDeserialize<T>(out T result) {
        try {
            result = JsonSerializer.Deserialize<T>(modAsset.Stream);
        } catch (JsonException) {
            result = default;
            return false;
        }
        return true;
    }
}