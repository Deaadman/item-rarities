using ItemRarities.Core;
using ItemRarities.Enums;

namespace ItemRarities.Tests;

public class RaritySortTests
{
    private static readonly Rarities[] Unsorted =
        [Rarities.Rare, Rarities.None, Rarities.Mythic, Rarities.Common, Rarities.Legendary];

    [Fact]
    public void Descending_puts_the_rarest_first_and_items_without_a_rarity_last()
    {
        var sorted = Unsorted.ToList();
        sorted.Sort((a, b) => RaritySort.Compare(a, b, descending: true));

        Assert.Equal([Rarities.Mythic, Rarities.Legendary, Rarities.Rare, Rarities.Common, Rarities.None], sorted);
    }

    [Fact]
    public void Ascending_puts_items_without_a_rarity_first_and_the_rarest_last()
    {
        var sorted = Unsorted.ToList();
        sorted.Sort((a, b) => RaritySort.Compare(a, b, descending: false));

        Assert.Equal([Rarities.None, Rarities.Common, Rarities.Rare, Rarities.Legendary, Rarities.Mythic], sorted);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void The_same_rarity_compares_equal(bool descending) =>
        Assert.Equal(0, RaritySort.Compare(Rarities.Epic, Rarities.Epic, descending));
}
