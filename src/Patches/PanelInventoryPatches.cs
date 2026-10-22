using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="Panel_Inventory"/>, the inventory.</summary>
internal static class PanelInventoryPatches
{
    /// <summary>Adds the sort-by-rarity button.</summary>
    [HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.Initialize))]
    private static class AddSortButton
    {
        private static void Postfix(Panel_Inventory __instance) =>
            RaritySorting.AddButton(__instance.m_SortButtons, __instance.m_SortFlipDictionary, __instance.OnSortChange, 85);
    }

    /// <summary>Flips the rarity order when the button is pressed while already sorting by rarity.</summary>
    [HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.OnSortChange))]
    private static class FlipSortOrder
    {
        private static void Prefix(Panel_Inventory __instance, UIButton sortButtonClicked) =>
            RaritySorting.OnSortButton(sortButtonClicked, __instance.m_SortName);
    }

    /// <summary>Sorts the inventory by rarity.</summary>
    [HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.UpdateFilteredInventoryList))]
    private static class SortInventory
    {
        private static void Postfix(Panel_Inventory __instance)
        {
            if (__instance.m_SortName == RaritySorting.SortName)
                RaritySorting.Sort(__instance.m_FilteredInventoryList);
        }
    }

    /// <summary>
    /// Colours the selected item's highlight with its rarity, and hides the rarity label when the selection has no
    /// item, like a decoration.
    /// </summary>
    [HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.UpdateGearStatsBlock))]
    private static class ColourSelection
    {
        private static void Postfix(Panel_Inventory __instance)
        {
            var selected = __instance.GetCurrentlySelectedItem();
            if (__instance.m_SelectedSpriteObj == null || __instance.m_SelectedSpriteTweenScale == null || selected == null)
                return;

            if (selected.m_GearItem == null)
                RarityLabels.Hide();

            __instance.m_SelectedSpriteObj.GetComponentInChildren<UISprite>().color = RarityColours.Get(selected.m_GearItem, 1, 0.5f);
            __instance.m_SelectedSpriteTweenScale.GetComponent<UISprite>().color = RarityColours.Get(selected.m_GearItem, 1, 1);
        }
    }
}
