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

}
