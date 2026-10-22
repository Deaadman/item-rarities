using ItemRarities.Enums;

namespace ItemRarities.Core;

/// <summary>The order the inventory and containers list items in when sorted by rarity.</summary>
internal static class RaritySort
{
    /// <summary>Compares two items' rarities for sorting.</summary>
    /// <param name="a">The first item's rarity, <see cref="Rarities.None"/> if it has none.</param>
    /// <param name="b">The second item's rarity, <see cref="Rarities.None"/> if it has none.</param>
    /// <param name="descending">Whether the rarest items come first. Items without a rarity come last then, and first otherwise.</param>
    /// <returns>Less than zero if <paramref name="a"/> comes first, more than zero if <paramref name="b"/> does, or zero.</returns>
    internal static int Compare(Rarities a, Rarities b, bool descending) => descending ? b.CompareTo(a) : a.CompareTo(b);
}
