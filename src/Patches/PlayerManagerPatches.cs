using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="PlayerManager"/>, which runs the player's interactions, like inspecting an item.</summary>
internal static class PlayerManagerPatches
{
    /// <summary>
    /// Shows the inspected item's rarity with the rest of its details, with a star like their icons, lined up on the
    /// left with them, and adds the label and star to the details that fade in, so they fade in with them.
    /// </summary>
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.InitLabelsForGear))]
    private static class ShowRarityWhenInspecting
    {
        private static void Prefix(PlayerManager __instance)
        {
            var panelHUD = InterfaceManager.GetPanel<Panel_HUD>();

            if (RarityLabels.s_InspectLabel == null)
                RarityLabels.CreateInspectLabel(panelHUD.m_InspectModeDetailsGrid.transform, panelHUD.m_InspectMode_Conditon);

            RarityLabels.Show(__instance.m_Gear, inspectLabel: true);

            if (RarityLabels.s_InspectLabel == null)
                return;

            var inspectFade = panelHUD.m_InspectFadeSequence[1];
            List<UIWidget> fadeElements = new(inspectFade.m_FadeElements);
            if (fadeElements.Contains(RarityLabels.s_InspectLabel))
                return;

            fadeElements.Add(RarityLabels.s_InspectLabel);
            if (RarityLabels.s_InspectIcon != null)
                fadeElements.Add(RarityLabels.s_InspectIcon);

            inspectFade.m_FadeElements = fadeElements.ToArray();
            panelHUD.m_InspectFadeSequence[1] = inspectFade;
        }

        private static void Postfix()
        {
            var panelHUD = InterfaceManager.GetPanel<Panel_HUD>();
            RarityLabels.AlignInspectLabel(panelHUD.m_InspectMode_Conditon, panelHUD.m_InspectMode_Description);
        }
    }
}
