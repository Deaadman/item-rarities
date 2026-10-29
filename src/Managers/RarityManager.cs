using System.Collections;
using System.IO.Compression;

using ItemRarities.Core;
using ItemRarities.Enums;
using ItemRarities.Utilities;

namespace ItemRarities.Managers;

/// <summary>Which rarity each item has, from the mod's own list, ModComponent mods and other mods' code.</summary>
/// <remarks>
/// Part of the mod's public API, through the ItemRarities NuGet package, so it keeps its name and namespace. Other mods
/// call <see cref="AddGearItemAndRarity"/>; the rest is the mod's own.
/// </remarks>
public static class RarityManager
{
    /// <summary>The manifest name of the embedded rarities of the game's own items.</summary>
    private const string RaritiesResource = "ItemRarities.Resources.ItemRarities.ir";

    /// <summary>The manifest name of the embedded rarities of the decorations the player can carry.</summary>
    private const string DecorationRaritiesResource = "ItemRarities.Resources.Decorations.ir";

    /// <summary>The file a ModComponent mod lists its items' rarities in, inside its <c>.modcomponent</c> archive.</summary>
    private const string ModComponentRaritiesEntry = "rarities.ir";

    /// <summary>Every item's rarity.</summary>
    internal static readonly RarityTable Table = new();

    /// <summary>Every decoration's rarity, by prefab name.</summary>
    internal static readonly RarityTable DecorationTable = new();

    /// <summary>Whether <see cref="InitializeRarities"/> has loaded the rarities.</summary>
    internal static bool s_Initialized;

    /// <summary>Gives a custom item a rarity.</summary>
    /// <param name="gearItem">The item's gear item name, like <c>GEAR_ExampleItem</c>.</param>
    /// <param name="rarity">Its rarity.</param>
    /// <example>
    /// <code>
    /// RarityManager.AddGearItemAndRarity("GEAR_ExampleItem", Rarities.Mythic);
    /// </code>
    /// </example>
    public static void AddGearItemAndRarity(string gearItem, Rarities rarity) => Table.Set(gearItem, rarity);

    /// <summary>Gets an item's rarity.</summary>
    /// <param name="itemName">The gear item's name, like <c>GEAR_Hatchet</c>.</param>
    /// <returns>Its rarity, or <see cref="Rarities.None"/> if it has none.</returns>
    internal static Rarities GetRarity(string itemName) => Table.Get(itemName);

    /// <summary>Gets an item's rarity.</summary>
    /// <param name="gearItem">The item, or <see langword="null"/>, which has none.</param>
    /// <returns>Its rarity, or <see cref="Rarities.None"/> if it has none.</returns>
    internal static Rarities GetRarity(GearItem? gearItem) =>
        gearItem == null ? Rarities.None : Table.Get(gearItem.name);

    /// <summary>Gets a decoration's rarity, by the name of the prefab it was made from.</summary>
    /// <param name="decorationItem">The decoration, or <see langword="null"/>, which has none.</param>
    /// <returns>Its rarity, or <see cref="Rarities.None"/> if it has none.</returns>
    internal static Rarities GetRarity(DecorationItem? decorationItem) =>
        decorationItem == null ? Rarities.None : DecorationTable.Get(DecorationNames.PrefabName(decorationItem.name));

    /// <summary>Gets the rarity of what a square in the inventory or container grid holds: an item or a decoration.</summary>
    /// <param name="item">The square's contents, or <see langword="null"/>, which has none.</param>
    /// <returns>Its rarity, or <see cref="Rarities.None"/> if it has none.</returns>
    internal static Rarities GetRarity(InventoryGridDataItem? item)
    {
        if (item == null)
            return Rarities.None;

        return item.m_GearItem != null ? GetRarity(item.m_GearItem) : GetRarity(item.m_DecorationItem);
    }

    /// <summary>
    /// Loads the rarities of the game's items and decorations, then those of the ModComponent mods' items, which can
    /// replace them. Runs as a coroutine once the main menu first loads.
    /// </summary>
    /// <returns>The coroutine.</returns>
    internal static IEnumerator InitializeRarities()
    {
        yield return LoadRaritiesFromLocalFile();
        LoadRaritiesFromModComponents();
        s_Initialized = true;

        Mod.Logger.Log($"Loaded the rarities of {Table.Count} items and {DecorationTable.Count} decorations.", FlaggedLoggingLevel.Debug);
    }

    /// <summary>Loads the rarities of the game's own items and decorations, embedded in the mod.</summary>
    /// <returns>The coroutine.</returns>
    private static IEnumerator LoadRaritiesFromLocalFile()
    {
        using (StringReader reader = new(EmbeddedResources.ReadText(RaritiesResource)))
            Table.LoadIr(reader);

        using (StringReader reader = new(EmbeddedResources.ReadText(DecorationRaritiesResource)))
            DecorationTable.LoadIr(reader);

        yield break;
    }

    /// <summary>
    /// Loads the rarities in the <see cref="ModComponentRaritiesEntry"/> of every <c>.modcomponent</c> file in the Mods
    /// folder, which the <see href="https://github.com/Deaadman/mod-component-sdk">ModComponent SDK</see> writes.
    /// </summary>
    private static void LoadRaritiesFromModComponents()
    {
        var modDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (modDirectory == null)
            return;

        foreach (var modComponentFile in Directory.GetFiles(modDirectory, "*.modcomponent"))
        {
            using var archive = ZipFile.OpenRead(modComponentFile);
            var entry = archive.GetEntry(ModComponentRaritiesEntry);
            if (entry == null)
                continue;

            using StreamReader reader = new(entry.Open());
            Table.LoadIr(reader);

            Mod.Logger.Log($"Loaded rarities from '{Path.GetFileName(modComponentFile)}'.", FlaggedLoggingLevel.Debug);
        }
    }
}
