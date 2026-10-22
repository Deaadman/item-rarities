using System.Globalization;

namespace ItemRarities.Core;

/// <summary>
/// Converts the custom colour settings between their 0 to 255 channels, hex codes like <c>#FF8800</c>, and the 0 to 1
/// channels that Item Rarities 2.0.5 and earlier saved.
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

    /// <summary>Gets a colour's hex code.</summary>
    /// <param name="red">The red, from 0 to 255.</param>
    /// <param name="green">The green, from 0 to 255.</param>
    /// <param name="blue">The blue, from 0 to 255.</param>
    /// <returns>The hex code, like <c>#FF8800</c>.</returns>
    internal static string ToHex(int red, int green, int blue) =>
        $"#{Clamp(red):X2}{Clamp(green):X2}{Clamp(blue):X2}";

    /// <summary>
    /// Reads a hex code a player typed: six hex digits like <c>#FF8800</c>, or three like <c>#F80</c>, in either case,
    /// with or without the <c>#</c> and spaces around it.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="red">The red, from 0 to 255, or 0 if the text isn't a hex code.</param>
    /// <param name="green">The green, from 0 to 255, or 0 if the text isn't a hex code.</param>
    /// <param name="blue">The blue, from 0 to 255, or 0 if the text isn't a hex code.</param>
    /// <returns>Whether the text is a hex code.</returns>
    internal static bool TryParseHex(string? text, out int red, out int green, out int blue)
    {
        red = green = blue = 0;

        var digits = text?.Trim().TrimStart('#') ?? string.Empty;
        if (digits.Length == 3)
            digits = string.Concat(digits.Select(digit => new string(digit, 2)));

        if (digits.Length != 6 || !int.TryParse(digits, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var value))
            return false;

        red = (value >> 16) & 0xFF;
        green = (value >> 8) & 0xFF;
        blue = value & 0xFF;
        return true;
    }

    /// <summary>Keeps a channel from 0 to 255.</summary>
    /// <param name="channel">The channel.</param>
    /// <returns>The channel, clamped.</returns>
    private static int Clamp(int channel) => Math.Clamp(channel, 0, MaxChannel);
}
