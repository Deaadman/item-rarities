using ItemRarities.Core;
using ItemRarities.Enums;
using ItemRarities.Managers;

namespace ItemRarities.Utilities;

/// <summary>The colour of each rarity, and colouring the inventory, container and clothing grids with it.</summary>
internal static class RarityColours
{
    /// <summary>Gets the colour of an item's rarity.</summary>
    /// <param name="gearItem">The item, or <see langword="null"/>, which counts as having no rarity.</param>
    /// <param name="alpha">The colour's alpha if the item has a rarity.</param>
    /// <param name="noRarityAlpha">The alpha of the white it gets if it has none.</param>
    /// <returns>The colour.</returns>
    internal static Color Get(GearItem? gearItem, float alpha = 1f, float noRarityAlpha = 0f) =>
        Get(RarityManager.GetRarity(gearItem), alpha, noRarityAlpha);

    /// <summary>Gets the colour of the rarity of what a square in the inventory or container grid holds.</summary>
    /// <param name="item">The square's item or decoration, or <see langword="null"/>, which counts as having no rarity.</param>
    /// <param name="alpha">The colour's alpha if it has a rarity.</param>
    /// <param name="noRarityAlpha">The alpha of the white it gets if it has none.</param>
    /// <returns>The colour.</returns>
    internal static Color Get(InventoryGridDataItem? item, float alpha = 1f, float noRarityAlpha = 0f) =>
        Get(RarityManager.GetRarity(item), alpha, noRarityAlpha);

    /// <summary>
    /// Gets the colour of a rarity: the player's own with <see cref="Settings.customColours"/> on, otherwise the mod's.
    /// </summary>
    /// <param name="rarity">The rarity.</param>
    /// <param name="alpha">The colour's alpha.</param>
    /// <param name="noRarityAlpha">The alpha of the white <see cref="Rarities.None"/> gets.</param>
    /// <returns>The colour.</returns>
    internal static Color Get(Rarities rarity, float alpha = 1f, float noRarityAlpha = 0f)
    {
        var settings = Settings.Instance;
        if (settings.customColours)
        {
            return rarity switch
            {
                Rarities.Common => FromChannels(settings.CommonRed, settings.CommonGreen, settings.CommonBlue, alpha),
                Rarities.Uncommon => FromChannels(settings.UncommonRed, settings.UncommonGreen, settings.UncommonBlue, alpha),
                Rarities.Rare => FromChannels(settings.RareRed, settings.RareGreen, settings.RareBlue, alpha),
                Rarities.Epic => FromChannels(settings.EpicRed, settings.EpicGreen, settings.EpicBlue, alpha),
                Rarities.Legendary => FromChannels(settings.LegendaryRed, settings.LegendaryGreen, settings.LegendaryBlue, alpha),
                Rarities.Mythic => FromChannels(settings.MythicRed, settings.MythicGreen, settings.MythicBlue, alpha),
                _ => new Color(1, 1, 1, noRarityAlpha)
            };
        }

        return rarity switch
        {
            Rarities.Common => new Color(0.6f, 0.6f, 0.6f, alpha),
            Rarities.Uncommon => new Color(0.3f, 0.7f, 0f, alpha),
            Rarities.Rare => new Color(0f, 0.6f, 0.9f, alpha),
            Rarities.Epic => new Color(0.7f, 0.3f, 0.9f, alpha),
            Rarities.Legendary => new Color(0.9f, 0.5f, 0.2f, alpha),
            Rarities.Mythic => new Color(0.8f, 0.7f, 0.3f, alpha),
            _ => new Color(1, 1, 1, noRarityAlpha)
        };
    }

    /// <summary>Makes a colour from the custom colour settings' 0 to 255 channels.</summary>
    /// <param name="red">The red, from 0 to 255.</param>
    /// <param name="green">The green, from 0 to 255.</param>
    /// <param name="blue">The blue, from 0 to 255.</param>
    /// <param name="alpha">The alpha, from 0 to 1.</param>
    /// <returns>The colour.</returns>
    private static Color FromChannels(int red, int green, int blue, float alpha) =>
        new(red / (float)ColourCodes.MaxChannel, green / (float)ColourCodes.MaxChannel, blue / (float)ColourCodes.MaxChannel, alpha);

    /// <summary>Colours a clothing slot's selection, hover and button with its item's rarity.</summary>
    /// <param name="slot">The clothing slot.</param>
    internal static void UpdateClothingSlot(ClothingSlot slot)
    {
        var color = slot.m_GearItem != null ? Get(slot.m_GearItem, 1, 0.5f) : new Color(1f, 1f, 1f, 0.5f);
        var hoverColor = new Color(color.r, color.g, color.b, 0.5f);
        var pressedColor = new Color(color.r, color.g, color.b, 0.6f);

        if (slot.m_Selected != null)
        {
            var children = new Il2CppSystem.Collections.Generic.List<Transform>();
            slot.m_Selected.GetComponentsInChildren(true, children);
            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child.gameObject.name is "InnerGlow" or "TweenedContent")
                    child.GetComponent<UISprite>().color = color;
            }
        }

        if (slot.m_SpriteBoxHover != null)
            slot.m_SpriteBoxHover.GetComponent<UISprite>().color = hoverColor;

        var buttonTransforms = new Il2CppSystem.Collections.Generic.List<Transform>();
        slot.GetComponentsInChildren(true, buttonTransforms);
        for (var i = 0; i < buttonTransforms.Count; i++)
        {
            var child = buttonTransforms[i];
            if (child.gameObject.name != "Button")
                continue;

            var widget = child.GetComponent<UIWidget>();
            if (widget != null)
                widget.color = color;

            var button = child.GetComponent<UIButton>();
            if (button == null)
                continue;

            button.hover = hoverColor;
            button.pressed = pressedColor;
        }
    }

    /// <summary>
    /// Colours every clothing slot at the end of the frame, once the game has finished updating them. Run as a coroutine.
    /// </summary>
    /// <param name="panel">The clothing panel.</param>
    /// <returns>The coroutine.</returns>
    internal static System.Collections.IEnumerator UpdateClothingSlotsAtEndOfFrame(Panel_Clothing panel)
    {
        yield return new WaitForEndOfFrame();

        foreach (var slot in panel.m_ClothingSlots)
            UpdateClothingSlot(slot);
    }

    /// <summary>Colours the selected item or decoration's highlight in the container panel with its rarity.</summary>
    /// <param name="panelContainer">The container panel.</param>
    internal static void UpdateContainerSelection(Panel_Container panelContainer)
    {
        var selected = panelContainer.GetCurrentlySelectedItem();
        if (panelContainer.m_SelectedSpriteObj == null || selected == null)
            return;

        panelContainer.m_SelectedSpriteObj.GetComponentInChildren<UISprite>().color = Get(selected, 1, 0.5f);

        var children = new Il2CppSystem.Collections.Generic.List<Transform>();
        panelContainer.m_SelectedSpriteObj.GetComponentsInChildren(true, children);
        for (var i = 0; i < children.Count; i++)
        {
            var child = children[i];
            if (child.gameObject.name is "TweenedContent")
                child.GetComponent<UISprite>().color = Get(selected, 1, 1);
        }
    }
}
