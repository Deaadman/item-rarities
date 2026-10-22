using System.Text.Json;

using ItemRarities.Enums;

namespace ItemRarities.Core;

/// <summary>The rarity of every item that has one, by gear item name.</summary>
internal sealed class RarityTable
{
    /// <summary>
    /// How lenient the JSON reader is: the rarities file may have comments and trailing commas, as it could when it was
    /// read with Newtonsoft.Json.
    /// </summary>
    private static readonly JsonDocumentOptions JsonOptions = new()
    {
        AllowTrailingCommas = true,
        CommentHandling = JsonCommentHandling.Skip
    };

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
    /// Reads rarities from JSON with an array of gear item names for each rarity, like
    /// <c>{ "Common": ["GEAR_Stick"], "Mythic": ["GEAR_Flaregun"] }</c>. Keys that aren't a rarity are skipped.
    /// </summary>
    /// <param name="json">The JSON text.</param>
    /// <exception cref="JsonException">The text isn't valid JSON.</exception>
    internal void LoadJson(string json)
    {
        using var document = JsonDocument.Parse(json, JsonOptions);

        foreach (var group in document.RootElement.EnumerateObject())
        {
            if (!Enum.TryParse(group.Name, out Rarities rarity) || group.Value.ValueKind != JsonValueKind.Array)
                continue;

            foreach (var item in group.Value.EnumerateArray())
                Set(item.ToString(), rarity);
        }
    }

    /// <summary>
    /// Reads rarities from a ModComponent mod's <c>rarities.ir</c>: a <c>[Rarity]</c> line, then the gear item names
    /// with that rarity, one a line. Blank lines and lines starting with <c>#</c> are skipped, as are names before the
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
