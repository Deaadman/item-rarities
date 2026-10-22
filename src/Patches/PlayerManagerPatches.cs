using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="PlayerManager"/>, which runs the player's interactions, like inspecting an item.</summary>
internal static class PlayerManagerPatches
{
    /// <summary>
    /// Shows the inspected item's rarity with the rest of its details, and adds the label to the details that fade in,
    /// so it fades in with them.
    /// </summary>
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.InitLabelsForGear))]
    private static class ShowRarityWhenInspecting
    {
        private static void Prefix(PlayerManager __instance)
        {
            var panelHUD = InterfaceManager.GetPanel<Panel_HUD>();

            if (RarityLabels.s_InspectLabel == null)
                RarityLabels.CreateInspectLabel(panelHUD.m_InspectModeDetailsGrid.transform);

            RarityLabels.Show(__instance.m_Gear, inspectLabel: true);

            var inspectFade = panelHUD.m_InspectFadeSequence[1];
            var fadeElements = new UIWidget[inspectFade.m_FadeElements.Length + 1];
            Array.Copy(inspectFade.m_FadeElements, fadeElements, inspectFade.m_FadeElements.Length);

            if (RarityLabels.s_InspectLabel == null)
                return;

            fadeElements[inspectFade.m_FadeElements.Length] = RarityLabels.s_InspectLabel;
            inspectFade.m_FadeElements = fadeElements;
            panelHUD.m_InspectFadeSequence[1] = inspectFade;
        }
    }
}
