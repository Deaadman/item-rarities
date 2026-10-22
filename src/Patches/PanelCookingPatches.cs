using Il2CppTLD.Cooking;

using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="Panel_Cooking"/>, the cooking menu.</summary>
internal static class PanelCookingPatches
{
    /// <summary>Shows the selected food's rarity above its name.</summary>
    [HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.GetSelectedCookableItem))]
    private static class ShowRarity
    {
        private static void Postfix(Panel_Cooking __instance, CookableItem __result)
        {
            RarityLabels.PlaceAbove(__instance.m_Label_CookedItemName.transform);
            RarityLabels.Show(__result.m_GearItem);
        }
    }
}
