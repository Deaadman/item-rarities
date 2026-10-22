namespace ItemRarities.Core;

/// <summary>
/// Converts the custom colour settings between their 0 to 255 channels and the 0 to 1 channels that Item Rarities 2.0.5
/// and earlier saved.
/// </summary>
internal static class ColourCodes
{
    /// <summary>The largest value of a channel.</summary>
    internal const int MaxChannel = 255;

    /// <summary>Converts a 0 to 1 channel, as saved by Item Rarities 2.0.5 and earlier, to 0 to 255.</summary>
    /// <param name="unit">The channel, from 0 to 1. Values outside that are clamped.</param>
    /// <returns>The channel, from 0 to 255, rounded to the nearest whole number with halves rounded up.</returns>
    internal static int FromUnit(float unit) =>
        Math.Clamp((int)Math.Round(unit * MaxChannel, MidpointRounding.AwayFromZero), 0, MaxChannel);

}
