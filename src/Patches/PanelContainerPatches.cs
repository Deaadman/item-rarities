using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="Panel_Container"/>, the panel for moving items between a container and the inventory.</summary>
internal static class PanelContainerPatches
{
    /// <summary>Adds the sort-by-rarity button.</summary>
    [HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.Initialize))]
    private static class AddSortButton
    {
        private static void Postfix(Panel_Container __instance) =>
            RaritySorting.AddButton(__instance.m_SortButtons, __instance.m_SortFlipDictionary, __instance.OnSortInventoryChange, 100);
    }

    /// <summary>Flips the rarity order when the button is pressed while already sorting by rarity.</summary>
    [HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.OnSortInventoryChange))]
    private static class FlipSortOrder
    {
        private static void Prefix(Panel_Container __instance, UIButton sortButtonClicked) =>
            RaritySorting.OnSortButton(sortButtonClicked, __instance.m_InventorySortName);
    }

    /// <summary>Sorts the container's side by rarity.</summary>
    [HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.UpdateFilteredContainerList))]
    private static class SortContainer
    {
        private static void Postfix(Panel_Container __instance)
        {
            if (__instance.m_InventorySortName == RaritySorting.SortName)
                RaritySorting.Sort(__instance.m_FilteredContainerList);
        }
    }

    /// <summary>Sorts the inventory's side by rarity.</summary>
    [HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.UpdateFilteredInventoryList))]
    private static class SortInventory
    {
        private static void Postfix(Panel_Container __instance)
        {
            if (__instance.m_InventorySortName == RaritySorting.SortName)
                RaritySorting.Sort(__instance.m_FilteredInventoryList);
        }
    }

    /// <summary>
    /// Keeps the selected item's highlight in its rarity's colour, including after moving the last item off one side.
    /// </summary>
    [HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.Update))]
    private static class ColourSelection
    {
        private static void Postfix(Panel_Container __instance)
        {
            // The game throws getting the selected item while the selected side is empty.
            var selectedSide = __instance.m_SelectedTable == SelectedTableEnum.ContainerTable
                ? __instance.m_FilteredContainerList
                : __instance.m_FilteredInventoryList;

            if (selectedSide.Count == 0)
                return;

            RarityColours.UpdateContainerSelection(__instance);
        }
    }
}
