using Il2CppTLD.Cooking;
using ItemRarities.Managers;

namespace ItemRarities.Patches;

internal static class OtherPatches
{
    [HarmonyPatch(typeof(ItemDescriptionPage), nameof(ItemDescriptionPage.UpdateGearItemDescription))]
    private static class UpdateItemDescriptionPageRarity
    {
        private static void Postfix(ItemDescriptionPage __instance, GearItem gi)
        {
            RarityUIManager.InstantiateOrMoveRarityLabel(__instance.m_ItemNameLabel.gameObject.transform, 0, 35, 0);
            RarityUIManager.UpdateRarityLabelProperties(gi);
        }
    }
    
    [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.UpdateDisplayText))]
    private static class UpdatePanelActionsRadialRarity
    {
        private static void Postfix(Panel_ActionsRadial __instance)
        {
            if (RarityUIManager.m_RarityLabel == null)
            {
                RarityUIManager.InstantiateOrMoveRarityLabel(__instance.m_SegmentLabel.gameObject.transform, 0, 35, 0);
            }

            foreach (var t in __instance.m_RadialArms)
            {
                if (t.IsHoveredOver())
                {
                    RarityUIManager.InstantiateOrMoveRarityLabel(__instance.m_SegmentLabel.gameObject.transform, 0, 35, 0);
                    RarityUIManager.UpdateRarityLabelProperties(t.m_GearItem);
                    if (t.m_GearItem == null) break;
                    t.m_BG.color = RarityUIManager.GetRarityAndColour(t.m_GearItem, 1, 0.3f);
                    break;
                }
                else
                {
                    RarityUIManager.m_RarityLabel.gameObject.SetActive(false);
                }
            }
        }
    }
    
    [HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.GetSelectedCookableItem))]
    private static class UpdatePanelCookingRarity
    {
        private static void Postfix(Panel_Cooking __instance, ref CookableItem __result)
        {
            RarityUIManager.InstantiateOrMoveRarityLabel(__instance.m_Label_CookedItemName.gameObject.transform, 0, 35, 0);
            RarityUIManager.UpdateRarityLabelProperties(__result.m_GearItem);
        }
    }
    
    [HarmonyPatch(typeof(Panel_Crafting), nameof(Panel_Crafting.RefreshSelectedBlueprint))]
    private static class UpdatePanelCraftingRarity
    {
        private static void Postfix(Panel_Crafting __instance)
        {
            if (__instance.SelectedBPI == null) return;
            RarityUIManager.InstantiateOrMoveRarityLabel(__instance.m_SelectedName.gameObject.transform, 0, 35, 0);
            RarityUIManager.UpdateRarityLabelProperties(__instance.SelectedBPI.m_CraftedResultGear);
        }
    }
    
    [HarmonyPatch(typeof(Panel_HUD), nameof(Panel_HUD.SetHoverText))]
    private static class UpdateHoverLabelRarity
    {
        private static void Postfix(Panel_HUD __instance, string hoverText, GameObject itemUnderCrosshairs, HoverTextState textState)
        {
            if (itemUnderCrosshairs == null) return;
            if (itemUnderCrosshairs.GetComponent<GearItem>() != null)
            {
                __instance.m_Label_ObjectName.color = RarityUIManager.GetRarityAndColour(itemUnderCrosshairs.GetComponent<GearItem>(), 1f, 1f);   
            }
        }
    }
    
    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.RefreshMainWindow))]
    private static class UpdatePanelInventoryExamineRarity
    {
        private static void Postfix(Panel_Inventory_Examine __instance)
        {
            RarityUIManager.InstantiateOrMoveRarityLabel(__instance.m_Item_Label.gameObject.transform, 0, 35, 0);
            RarityUIManager.UpdateRarityLabelProperties(__instance.m_GearItem);
        }
    }

    [HarmonyPatch(typeof(Panel_Milling), nameof(Panel_Milling.GetSelected))]
    private static class UpdatePanelMillingRarity
    {
        private static void Postfix(Panel_Milling __instance, ref GearItem __result)
        {
            RarityUIManager.InstantiateOrMoveRarityLabel(__instance.m_NameLabel.gameObject.transform, 0, 35, 0);
            RarityUIManager.UpdateRarityLabelProperties(__result);
        }
    }
    
    // Might be a good idea to revisit later and use the same technique for the UIButton for the sorting.
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.InitLabelsForGear))]
    private static class UpdatePlayerManagerInspectModeRarity
    {
        private static void Prefix(PlayerManager __instance)
        {
            var panelHUD = InterfaceManager.GetPanel<Panel_HUD>();
            
            if (RarityUIManager.m_RarityLabelInspect == null) RarityUIManager.InstantiateInspectRarityLabel(panelHUD.m_InspectModeDetailsGrid.gameObject.transform);
            
            RarityUIManager.UpdateRarityLabelProperties(__instance.m_Gear, true);

            var inspectFade = panelHUD.m_InspectFadeSequence[1];
            var newSize = inspectFade.m_FadeElements.Length + 1;
            var newFadeElements = new UIWidget[newSize];

            Array.Copy(inspectFade.m_FadeElements, newFadeElements, inspectFade.m_FadeElements.Length);
            newFadeElements[inspectFade.m_FadeElements.Length] = RarityUIManager.m_RarityLabelInspect;
            inspectFade.m_FadeElements = newFadeElements;
            panelHUD.m_InspectFadeSequence[1] = inspectFade;
        }
    }
}