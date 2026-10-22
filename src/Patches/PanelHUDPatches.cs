using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="Panel_HUD"/>, the in-game HUD.</summary>
internal static class PanelHUDPatches
{
    /// <summary>Colours the name of the item under the crosshair with its rarity.</summary>
    [HarmonyPatch(typeof(Panel_HUD), nameof(Panel_HUD.SetHoverText))]
    private static class ColourHoverText
    {
        private static void Postfix(Panel_HUD __instance, GameObject itemUnderCrosshairs)
        {
            if (itemUnderCrosshairs == null)
                return;

            var gearItem = itemUnderCrosshairs.GetComponent<GearItem>();
            if (gearItem != null)
                __instance.m_Label_ObjectName.color = RarityColours.Get(gearItem, 1f, 1f);
        }
    }
}
