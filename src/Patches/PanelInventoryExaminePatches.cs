using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="Panel_Inventory_Examine"/>, the panel for examining, repairing and harvesting an item.</summary>
internal static class PanelInventoryExaminePatches
{
    /// <summary>Shows the item's rarity above its name.</summary>
    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.RefreshMainWindow))]
    private static class ShowRarity
    {
        private static void Postfix(Panel_Inventory_Examine __instance)
        {
            RarityLabels.PlaceAbove(__instance.m_Item_Label.transform);
            RarityLabels.Show(__instance.m_GearItem);
        }
    }
}
