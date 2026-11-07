using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="Panel_Clothing"/>, the clothing panel.</summary>
internal static class PanelClothingPatches
{
    /// <summary>Colours every slot with its item's rarity when the panel opens.</summary>
    [HarmonyPatch(typeof(Panel_Clothing), nameof(Panel_Clothing.Enable))]
    private static class ColourSlotsOnOpen
    {
        private static void Postfix(Panel_Clothing __instance)
        {
            foreach (var slot in __instance.m_ClothingSlots)
                RarityColours.UpdateClothingSlot(slot);
        }
    }

    /// <summary>
    /// Colours every slot again after the player puts on or takes off clothing. The game updates the slots later in
    /// the frame, so this waits for the end of it.
    /// </summary>
    [HarmonyPatch(typeof(Panel_Clothing), nameof(Panel_Clothing.OnUseClothingItem))]
    private static class ColourSlotsOnUse
    {
        private static void Postfix(Panel_Clothing __instance) =>
            MelonCoroutines.Start(RarityColours.UpdateClothingSlotsAtEndOfFrame(__instance));
    }
}
