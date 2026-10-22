using Il2CppInterop.Runtime.InteropTypes.Arrays;

using ItemRarities.Core;
using ItemRarities.Enums;
using ItemRarities.Managers;

namespace ItemRarities.Utilities;

/// <summary>
/// The sort-by-rarity button in the inventory and container panels. Pressing it again flips the order, like the
/// game's own sort buttons.
/// </summary>
internal static class RaritySorting
{
    /// <summary>The sort's name, which the panels keep as the current sort and look up in their flip dictionaries.</summary>
    internal const string SortName = "GAMEPLAY_SortRarity";

    /// <summary>The name of the button's game object.</summary>
    internal const string ButtonName = "Button_SortRarity";

    /// <summary>The button's icon, a star from the game's atlas.</summary>
    private const string ButtonSprite = "ico_Star";

    /// <summary>Whether the rarest items come first.</summary>
    private static bool s_Descending = true;

    /// <summary>Adds the button after a panel's sort buttons.</summary>
    /// <param name="sortButtons">The panel's sort buttons. The new one copies the last.</param>
    /// <param name="sortFlipDictionary">The panel's record of which sorts are flipped.</param>
    /// <param name="onSort">The panel's handler for its sort buttons.</param>
    /// <param name="x">The button's x position.</param>
    internal static void AddButton(Il2CppReferenceArray<UIButton> sortButtons, Il2CppSystem.Collections.Generic.Dictionary<string, bool> sortFlipDictionary, Action<UIButton> onSort, float x)
    {
        UIButtonExtensions.NewSortButton(sortButtons, ButtonName, ButtonSprite, onSort, x, 1.8f);
        sortFlipDictionary.Add(SortName, false);
    }

    /// <summary>Flips the order if the button was pressed while already sorting by rarity.</summary>
    /// <param name="button">The sort button that was pressed.</param>
    /// <param name="currentSort">The panel's sort before the press.</param>
    internal static void OnSortButton(UIButton button, string currentSort)
    {
        if (button.name == ButtonName && currentSort == SortName)
            s_Descending = !s_Descending;
    }

    /// <summary>Sorts a panel's items by rarity, in the current order.</summary>
    /// <param name="items">The panel's filtered list of items, sorted in place.</param>
    internal static void Sort(Il2CppSystem.Collections.Generic.List<InventoryGridDataItem> items)
    {
        List<InventoryGridDataItem> sorted = new(items.Count);
        for (var i = 0; i < items.Count; i++)
            sorted.Add(items[i]);

        sorted.Sort((a, b) => RaritySort.Compare(RarityOf(a), RarityOf(b), s_Descending));

        items.Clear();
        foreach (var item in sorted)
            items.Add(item);
    }

    /// <summary>Gets a grid item's rarity.</summary>
    /// <param name="item">The grid item, which may be empty or hold no gear item.</param>
    /// <returns>Its gear item's rarity, or <see cref="Rarities.None"/>.</returns>
    private static Rarities RarityOf(InventoryGridDataItem? item) =>
        item?.m_GearItem != null ? RarityManager.GetRarity(item.m_GearItem.name) : Rarities.None;
}
