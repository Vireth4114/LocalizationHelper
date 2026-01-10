namespace Celeste.Mod.LocalizationHelper.Formats;
public class LocalizationFile(ModAsset asset) {
    public ModAsset modAsset = asset;

    public virtual bool TryDeserialize<T>(out T result) {
        return modAsset.TryDeserialize(out result);
    }
}
