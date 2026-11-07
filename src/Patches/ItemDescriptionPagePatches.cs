using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="ItemDescriptionPage"/>, the selected item's details beside the inventory.</summary>
internal static class ItemDescriptionPagePatches
{
    /// <summary>Shows the item's rarity above its name.</summary>
    [HarmonyPatch(typeof(ItemDescriptionPage), nameof(ItemDescriptionPage.UpdateGearItemDescription))]
    private static class ShowRarity
    {
        private static void Postfix(ItemDescriptionPage __instance, GearItem gi)
        {
            RarityLabels.PlaceAbove(__instance.m_ItemNameLabel.transform);
            RarityLabels.Show(gi);
        }
    }

    /// <summary>Shows a carried decoration's rarity above its name.</summary>
    [HarmonyPatch(typeof(ItemDescriptionPage), nameof(ItemDescriptionPage.UpdateDecorationItemDescription))]
    private static class ShowDecorationRarity
    {
        private static void Postfix(ItemDescriptionPage __instance, DecorationItem di)
        {
            RarityLabels.PlaceAbove(__instance.m_ItemNameLabel.transform);
            RarityLabels.Show(di);
        }
    }
}
