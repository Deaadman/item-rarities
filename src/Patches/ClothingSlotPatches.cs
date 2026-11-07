using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="ClothingSlot"/>, a slot on the clothing panel.</summary>
internal static class ClothingSlotPatches
{
    /// <summary>Colours the slot with its item's rarity when the mouse moves over it.</summary>
    [HarmonyPatch(typeof(ClothingSlot), nameof(ClothingSlot.ActivateMouseHoverHighlight))]
    private static class ColourOnHover
    {
        private static void Postfix(ClothingSlot __instance) => RarityColours.UpdateClothingSlot(__instance);
    }

    /// <summary>Colours the slot with its item's rarity when it's selected or deselected.</summary>
    [HarmonyPatch(typeof(ClothingSlot), nameof(ClothingSlot.SetSelected))]
    private static class ColourOnSelect
    {
        private static void Postfix(ClothingSlot __instance) => RarityColours.UpdateClothingSlot(__instance);
    }
}
