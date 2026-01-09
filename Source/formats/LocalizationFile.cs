using System.Collections.Generic;

namespace Celeste.Mod.LocalizationHelper.Format;
public class LocalizationFile(ModAsset asset) {
    public ModAsset modAsset = asset;

    public virtual bool TryDeserialize(out Dictionary<string, Dictionary<string, string>> result) {
        return modAsset.TryDeserialize(out result);
    }
}