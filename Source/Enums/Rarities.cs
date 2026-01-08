namespace ItemRarities.Enums;

/// <summary>
/// Represents different rarity levels for GearItems.
/// </summary>
public enum Rarities
{
    /// <summary>
    /// No rarity assigned, the default value.
    /// </summary>
    None,
    /// <summary>
    /// Items that are easily found.
    /// </summary>
    Common,
    /// <summary>
    /// Items that are easily found, but with more value.
    /// </summary>
    Uncommon,
    /// <summary>
    /// Items that are harder to come across.
    /// </summary>
    Rare,
    /// <summary>
    /// Items that are harder to come across, but with more value.
    /// </summary>
    Epic,
    /// <summary>
    /// Items with incredible value, and which are really to obtain.
    /// </summary>
    Legendary,
    /// <summary>
    /// Extremely rare items, nearly un-obtainable.
    /// </summary>
    Mythic
}