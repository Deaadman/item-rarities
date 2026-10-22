namespace ItemRarities.Enums;

/// <summary>How rare an item is, from <see cref="Common"/> to <see cref="Mythic"/>.</summary>
/// <remarks>
/// Part of the mod's public API: other mods pass it to <see cref="Managers.RarityManager.AddGearItemAndRarity"/>. The
/// order is the sort order, and the names are what <c>ItemRarities.ir</c> and <c>rarities.ir</c> files use, so
/// don't reorder or rename them.
/// </remarks>
public enum Rarities
{
    /// <summary>No rarity: the item shows no label or colour.</summary>
    None,

    /// <summary>Items that are easy to find.</summary>
    Common,

    /// <summary>Items that are easy to find, but worth more.</summary>
    Uncommon,

    /// <summary>Items that are harder to come across.</summary>
    Rare,

    /// <summary>Items that are harder to come across, and worth more.</summary>
    Epic,

    /// <summary>Items that are worth a lot and hard to find.</summary>
    Legendary,

    /// <summary>The rarest items, nearly impossible to find. Their label glows.</summary>
    Mythic
}
