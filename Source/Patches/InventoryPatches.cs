using ItemRarities.Managers;
using ItemRarities.Utilities;

namespace ItemRarities.Patches;

internal static class InventoryPatches
{
    // This needs a bit work, the hover affect looks strange when an item doesn't have a rarity assigned.
    [HarmonyPatch(typeof(InventoryGridItem), nameof(InventoryGridItem.OnHover))]
    private static class InventoryGridItemHoverPatch
    {
        private static void Postfix(InventoryGridItem __instance, bool isOver)
        {
            if (__instance.m_Button == null || __instance.m_GearItem == null) return;
            __instance.m_Button.hover = RarityUIManager.GetRarityAndColour(__instance.m_GearItem, 0.5f);
            __instance.m_Button.pressed = RarityUIManager.GetRarityAndColour(__instance.m_GearItem, 0.5f, 0.5f);
        }
    }
    
    [HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.Initialize))]
    private static class PanelInventoryInitializePatch
    {
        private static void Postfix(Panel_Inventory __instance)
        {
            UIButtonExtensions.NewSortButton(__instance.m_SortButtons, "Button_SortRarity", "ico_Star", __instance.OnSortChange, 85, 1.8f);
            __instance.m_SortFlipDictionary.Add("GAMEPLAY_SortRarity", false);
        }
    }
    
    [HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.OnSortChange))]
    private static class PanelInventorySortPatch
    {
        private static void Prefix(Panel_Inventory __instance, UIButton sortButtonClicked)
        {
            if (sortButtonClicked.name == "Button_SortRarity")
            {
                if (__instance.m_SortName == "GAMEPLAY_SortRarity")
                {
                    RarityUIManager.ToggleRaritySort();
                }
            }
        }
    }

    [HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.UpdateFilteredInventoryList))]
    private static class PanelInventorySortRarityPatch
    {
        private static void Postfix(Panel_Inventory __instance)
        {
            if (__instance.m_SortName == "GAMEPLAY_SortRarity")
            {
                RarityUIManager.CompareGearByRarity(__instance);
            }
        }
    }
    
    [HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.UpdateGearStatsBlock))]
    private static class PanelInventoryUpdateGearPatch
    {
        private static void Postfix(Panel_Inventory __instance)
        {
            if (__instance.m_SelectedSpriteObj == null || __instance.m_SelectedSpriteTweenScale == null || __instance.GetCurrentlySelectedItem().m_GearItem == null) return;
            __instance.m_SelectedSpriteObj.GetComponentInChildren<UISprite>().color = RarityUIManager.GetRarityAndColour(__instance.GetCurrentlySelectedItem().m_GearItem, 1, 0.5f);
            __instance.m_SelectedSpriteTweenScale.GetComponent<UISprite>().color = RarityUIManager.GetRarityAndColour(__instance.GetCurrentlySelectedItem().m_GearItem, 1, 1);
        }
    }
}