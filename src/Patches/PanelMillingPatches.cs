using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="Panel_Milling"/>, the milling machine's menu.</summary>
internal static class PanelMillingPatches
{
    /// <summary>Shows the selected item's rarity above its name.</summary>
    [HarmonyPatch(typeof(Panel_Milling), nameof(Panel_Milling.GetSelected))]
    private static class ShowRarity
    {
        private static void Postfix(Panel_Milling __instance, GearItem __result)
        {
            RarityLabels.PlaceAbove(__instance.m_NameLabel.transform);
            RarityLabels.Show(__result);
        }
    }
}
