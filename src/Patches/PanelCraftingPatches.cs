using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="Panel_Crafting"/>, the crafting menu.</summary>
internal static class PanelCraftingPatches
{
    /// <summary>Shows the rarity of what the selected blueprint makes above its name. Decorations have none.</summary>
    [HarmonyPatch(typeof(Panel_Crafting), nameof(Panel_Crafting.RefreshSelectedBlueprint))]
    private static class ShowRarity
    {
        private static void Postfix(Panel_Crafting __instance)
        {
            var blueprint = __instance.SelectedBPI;
            if (blueprint == null)
                return;

            if (blueprint.m_CraftedResultDecoration)
            {
                RarityLabels.Hide();
                return;
            }

            RarityLabels.PlaceAbove(__instance.m_SelectedName.transform);
            RarityLabels.Show(blueprint.m_CraftedResultGear);
        }
    }
}
