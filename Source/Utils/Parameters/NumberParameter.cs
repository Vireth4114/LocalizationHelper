using System.Text.RegularExpressions;

namespace Celeste.Mod.LocalizationHelper.Utils.Parameters;

/// <summary>
/// Inherits from FrameParameter, exist to allow the use of the parameter {NUMBER:NUM-NUM} that acts the same as {FRAME:FRAME-FRAME}.
/// For details about implementation, check the FrameParameter class.
/// </summary>
public partial class NumberParameter: FrameParameter {

    protected override Regex FramePattern => NumberRegex();
    
    [GeneratedRegex(@"\{NUMBER:(?<STARTING_NUMBER>\d+)\-(?<ENDING_NUMBER>\d+)\}")]
    private static partial Regex NumberRegex();
}
