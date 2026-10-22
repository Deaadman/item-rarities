using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="Panel_ActionsRadial"/>, the radial menu.</summary>
internal static class PanelActionsRadialPatches
{
    /// <summary>
    /// Shows the hovered item's rarity above the radial menu's label, and colours its segment with it. Hides the label
    /// when no item is hovered.
    /// </summary>
    [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.UpdateDisplayText))]
    private static class ShowRarity
    {
        private static void Postfix(Panel_ActionsRadial __instance)
        {
            if (RarityLabels.s_Label == null)
                RarityLabels.PlaceAbove(__instance.m_SegmentLabel.transform);

            foreach (var arm in __instance.m_RadialArms)
            {
                if (!arm.IsHoveredOver())
                {
                    RarityLabels.Hide();
                    continue;
                }

                RarityLabels.PlaceAbove(__instance.m_SegmentLabel.transform);
                RarityLabels.Show(arm.m_GearItem);
                if (arm.m_GearItem != null)
                    arm.m_BG.color = RarityColours.Get(arm.m_GearItem, 1, 0.3f);

                break;
            }
        }
    }
}
