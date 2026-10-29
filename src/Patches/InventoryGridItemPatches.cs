using ItemRarities.Enums;
using ItemRarities.Managers;
using ItemRarities.Utilities;

namespace ItemRarities.Patches;

/// <summary>Patches on <see cref="InventoryGridItem"/>, an item's square in the inventory and container grids.</summary>
internal static class InventoryGridItemPatches
{
    /// <summary>Colours the selected item's highlight with its rarity when an item in the container panel is clicked.</summary>
    [HarmonyPatch(typeof(InventoryGridItem), nameof(InventoryGridItem.OnClick))]
    private static class ColourContainerSelectionOnClick
    {
        private static void Postfix()
        {
            var panelContainer = InterfaceManager.GetPanel<Panel_Container>();
            if (panelContainer == null)
                return;

            RarityColours.UpdateContainerSelection(panelContainer);
        }
    }

    /// <summary>
    /// Tints the square in its item or decoration's rarity's colour when it's filled, with
    /// <see cref="Settings.colourSquares"/> on. Squares without a rarity, and every square with it off, keep the game's
    /// own colour.
    /// </summary>
    [HarmonyPatch]
    private static class ColourSquares
    {
        /// <summary>How strong the tint is, so the item's picture stays easy to see.</summary>
        private const float TintAlpha = 0.35f;

        /// <summary>The game's own colour for a square, read before the first tint, to put back.</summary>
        private static Color? s_DefaultColour;

        private static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(InventoryGridItem), nameof(InventoryGridItem.Refresh));
            yield return AccessTools.Method(typeof(InventoryGridItem), nameof(InventoryGridItem.RefreshDecorationItem));
            yield return AccessTools.Method(typeof(InventoryGridItem), nameof(InventoryGridItem.RefreshEmptyItem));
        }

        private static void Postfix(InventoryGridItem __instance)
        {
            if (__instance.m_Button == null)
                return;

            s_DefaultColour ??= __instance.m_Button.defaultColor;

            var rarity = !Settings.Instance.colourSquares ? Rarities.None
                : __instance.m_GearItem != null ? RarityManager.GetRarity(__instance.m_GearItem)
                : RarityManager.GetRarity(__instance.m_DecorationItem);

            __instance.m_Button.defaultColor = rarity == Rarities.None ? s_DefaultColour.Value : RarityColours.Get(rarity, TintAlpha);
        }
    }

    /// <summary>Colours the square's hover and press highlights with its item or decoration's rarity.</summary>
    /// <remarks>
    /// The hover looks odd on items without a rarity, and flickers a little on decoration items.
    /// </remarks>
    [HarmonyPatch(typeof(InventoryGridItem), nameof(InventoryGridItem.OnHover))]
    private static class ColourOnHover
    {
        private static void Postfix(InventoryGridItem __instance)
        {
            if (__instance.m_Button == null)
                return;

            var rarity = __instance.m_GearItem != null
                ? RarityManager.GetRarity(__instance.m_GearItem)
                : RarityManager.GetRarity(__instance.m_DecorationItem);

            __instance.m_Button.hover = RarityColours.Get(rarity, 0.5f);
            __instance.m_Button.pressed = RarityColours.Get(rarity, 0.5f, 0.5f);
        }
    }
}
