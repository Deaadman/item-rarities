using ItemRarities.Core;

namespace ItemRarities.Tests;

public class ColourCodesTests
{
    [Theory]
    [InlineData(0f, 0)]
    [InlineData(1f, 255)]
    [InlineData(0.6f, 153)]
    [InlineData(0.3f, 77)]
    [InlineData(0.5f, 128)]
    [InlineData(0.9f, 230)]
    public void Old_channels_scale_to_0_to_255_rounding_halves_up(float unit, int expected) =>
        Assert.Equal(expected, ColourCodes.FromUnit(unit));

    [Theory]
    [InlineData(-0.5f, 0)]
    [InlineData(1.5f, 255)]
    public void Old_channels_outside_0_to_1_are_clamped(float unit, int expected) =>
        Assert.Equal(expected, ColourCodes.FromUnit(unit));

    [Theory]
    [InlineData(255, 136, 0, "#FF8800")]
    [InlineData(0, 0, 0, "#000000")]
    [InlineData(153, 153, 153, "#999999")]
    [InlineData(300, -5, 10, "#FF000A")]
    public void Colours_become_upper_case_hex_codes(int red, int green, int blue, string expected) =>
        Assert.Equal(expected, ColourCodes.ToHex(red, green, blue));

    [Theory]
    [InlineData("#FF8800")]
    [InlineData("ff8800")]
    [InlineData("  #ff8800 ")]
    [InlineData("#F80")]
    [InlineData("f80")]
    public void Hex_codes_are_read_in_any_form(string text)
    {
        Assert.True(ColourCodes.TryParseHex(text, out var red, out var green, out var blue));
        Assert.Equal((255, 136, 0), (red, green, blue));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("#FF88")]
    [InlineData("#FF88001")]
    [InlineData("#GG8800")]
    [InlineData("red")]
    [InlineData("#-F8800")]
    public void Text_that_isnt_a_hex_code_is_rejected(string? text)
    {
        Assert.False(ColourCodes.TryParseHex(text, out var red, out var green, out var blue));
        Assert.Equal((0, 0, 0), (red, green, blue));
    }

    [Theory]
    [InlineData(153, 77, 230)]
    [InlineData(0, 255, 51)]
    public void A_colour_survives_a_round_trip_through_its_hex_code(int red, int green, int blue)
    {
        Assert.True(ColourCodes.TryParseHex(ColourCodes.ToHex(red, green, blue), out var r, out var g, out var b));
        Assert.Equal((red, green, blue), (r, g, b));
    }
}
