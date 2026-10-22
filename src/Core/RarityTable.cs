using ItemRarities.Enums;

namespace ItemRarities.Core;

/// <summary>The rarity of every item that has one, by gear item name.</summary>
internal sealed class RarityTable
{
    /// <summary>Each item's rarity, by gear item name.</summary>
    private readonly Dictionary<string, Rarities> m_Rarities = new();

    /// <summary>How many items have a rarity.</summary>
    internal int Count => m_Rarities.Count;

    /// <summary>Gets an item's rarity.</summary>
    /// <param name="gearName">The gear item's name, like <c>GEAR_Hatchet</c>.</param>
    /// <returns>Its rarity, or <see cref="Rarities.None"/> if it has none.</returns>
    internal Rarities Get(string gearName) => m_Rarities.GetValueOrDefault(gearName, Rarities.None);

    /// <summary>Gives an item a rarity, replacing any it had.</summary>
    /// <param name="gearName">The gear item's name, like <c>GEAR_Hatchet</c>.</param>
    /// <param name="rarity">Its rarity.</param>
    internal void Set(string gearName, Rarities rarity) => m_Rarities[gearName] = rarity;

    /// <summary>
    /// Reads rarities in the <c>.ir</c> format of the mod's own <c>ItemRarities.ir</c> and a ModComponent mod's
    /// <c>rarities.ir</c>: a <c>[Rarity]</c> line, then the gear item names with that rarity, one a line. Blank lines and lines starting with <c>#</c> are skipped, as are names before the
    /// first rarity or under <c>[None]</c>. A line in brackets that isn't a rarity leaves the rarity as it was.
    /// </summary>
    /// <param name="reader">The file's text.</param>
    internal void LoadIr(TextReader reader)
    {
        var currentRarity = Rarities.None;

        while (reader.ReadLine() is { } line)
        {
            line = line.Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith('#'))
                continue;

            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                if (Enum.TryParse(line.Trim('[', ']'), out Rarities rarity))
                    currentRarity = rarity;
            }
            else if (currentRarity != Rarities.None)
            {
                Set(line, currentRarity);
            }
        }
    }
}
