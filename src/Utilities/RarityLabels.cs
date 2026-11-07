using ItemRarities.Components;
using ItemRarities.Enums;
using ItemRarities.Managers;

namespace ItemRarities.Utilities;

/// <summary>
/// The label that names an item's rarity in its colour. One label moves between the menus, above the selected item's
/// name; the inspect view has its own, with the rest of the item's details.
/// </summary>
internal static class RarityLabels
{
    /// <summary>How far above the item's name the label sits.</summary>
    internal const float HeightAboveName = 35;

    /// <summary>
    /// How much bigger than the condition's icon the inspect view's star is drawn, since the star has more empty space
    /// around it, so the two look the same size.
    /// </summary>
    private const float InspectIconScale = 1.25f;

    /// <summary>The label in the menus, or <see langword="null"/> until one first shows an item.</summary>
    internal static UILabel? s_Label;

    /// <summary>The label in the inspect view, or <see langword="null"/> until the player first inspects an item.</summary>
    internal static UILabel? s_InspectLabel;

    /// <summary>
    /// The star to the left of the inspect view's label, like the icons beside the item's condition and weight, or
    /// <see langword="null"/> until the player first inspects an item, or if there was no condition icon to size it by.
    /// </summary>
    internal static UISprite? s_InspectIcon;

    /// <summary>Creates the inspect view's label, and its star the size of the condition's icon.</summary>
    /// <param name="parent">The inspect view's details grid.</param>
    /// <param name="condition">The inspect view's condition label.</param>
    internal static void CreateInspectLabel(Transform parent, UILabel condition)
    {
        GameObject gameObject = new("RarityLabelInspectGameObject");
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.localPosition = Vector3.zero;

        s_InspectLabel = UILabelExtensions.SetupGameObjectWithUILabel("RarityLabelInspect", gameObject.transform, false, true);
        s_InspectLabel.transform.SetSiblingIndex(0);

        var conditionIcon = FindIcon(condition);
        if (conditionIcon == null)
        {
            Mod.Logger.Log("Couldn't find the inspect view's condition icon, so the rarity has no star", FlaggedLoggingLevel.Warning);
            return;
        }

        GameObject iconObject = new("RarityIconInspect");
        iconObject.transform.SetParent(gameObject.transform, false);
        iconObject.transform.localPosition = Vector3.zero;

        s_InspectIcon = iconObject.AddComponent<UISprite>();
        s_InspectIcon.atlas = conditionIcon.atlas;
        if (s_InspectIcon.atlas == null || s_InspectIcon.atlas.GetSprite(RaritySorting.ButtonSprite) == null)
            s_InspectIcon.atlas = InterfaceManager.GetPanel<Panel_Inventory>().m_SortButtons[0].GetComponent<UISprite>().atlas;

        s_InspectIcon.spriteName = RaritySorting.ButtonSprite;
        s_InspectIcon.type = UIBasicSprite.Type.Simple;
        s_InspectIcon.depth = s_InspectLabel.depth;

        // The condition icon's height, scaled up to make up for the star's wider margins, in the star's own shape.
        var star = s_InspectIcon.atlas.GetSprite(RaritySorting.ButtonSprite);
        s_InspectIcon.height = Mathf.RoundToInt(conditionIcon.height * InspectIconScale);
        s_InspectIcon.width = star != null && star.height > 0
            ? Mathf.RoundToInt(s_InspectIcon.height * (float)star.width / star.height)
            : s_InspectIcon.height;
    }

    /// <summary>
    /// Lines the inspect view's star and label up with the condition's icon and text, or the label with the left edge
    /// of the item's description if there's no star.
    /// </summary>
    /// <param name="condition">The inspect view's condition label.</param>
    /// <param name="description">The inspect view's description label.</param>
    internal static void AlignInspectLabel(UILabel condition, UILabel description)
    {
        if (s_InspectLabel == null || condition == null || description == null)
            return;

        var conditionIcon = FindIcon(condition);
        if (s_InspectIcon == null || conditionIcon == null)
        {
            SetWorldX(s_InspectLabel.transform, description.worldCorners[0].x);
            return;
        }

        SetWorldX(s_InspectIcon.transform, conditionIcon.transform.position.x);
        SetWorldX(s_InspectLabel.transform, condition.worldCorners[0].x);

        // Sits as far above or below the label as the condition's icon does its text.
        var position = s_InspectIcon.transform.position;
        position.y = s_InspectLabel.transform.position.y + (conditionIcon.transform.position.y - condition.transform.position.y);
        s_InspectIcon.transform.position = position;
    }

    /// <summary>Moves the menus' label above an item's name, creating it the first time.</summary>
    /// <param name="itemName">The item's name label, or the label that takes its place.</param>
    internal static void PlaceAbove(Transform itemName)
    {
        if (s_Label == null)
        {
            s_Label = UILabelExtensions.SetupGameObjectWithUILabel("RarityLabel", itemName, false, false, 0, HeightAboveName, 0);
        }
        else
        {
            s_Label.transform.SetParent(itemName, false);
            s_Label.transform.localPosition = new Vector3(0, HeightAboveName, 0);
        }
    }

    /// <summary>Hides the menus' label.</summary>
    internal static void Hide() => s_Label?.gameObject.SetActive(false);

    /// <summary>
    /// Shows an item's rarity on a label, in its colour, glowing for <see cref="Rarities.Mythic"/>, with the inspect
    /// view's star in the same colour. Hides them if the item has no rarity.
    /// </summary>
    /// <param name="gearItem">The item, or <see langword="null"/> to leave the label as it is.</param>
    /// <param name="inspectLabel">Whether to update the inspect view's label rather than the menus'.</param>
    internal static void Show(GearItem? gearItem, bool inspectLabel = false)
    {
        if (gearItem != null)
            Show(RarityManager.GetRarity(gearItem), inspectLabel);
    }

    /// <summary>
    /// Shows a decoration's rarity on the menus' label, in its colour, glowing for <see cref="Rarities.Mythic"/>. Hides
    /// it if the decoration has no rarity.
    /// </summary>
    /// <param name="decorationItem">The decoration, or <see langword="null"/> to leave the label as it is.</param>
    internal static void Show(DecorationItem? decorationItem)
    {
        if (decorationItem != null)
            Show(RarityManager.GetRarity(decorationItem), false);
    }

    /// <summary>Shows a rarity on a label, with the inspect view's star in the same colour, or hides them for none.</summary>
    /// <param name="rarity">The rarity.</param>
    /// <param name="inspectLabel">Whether to update the inspect view's label rather than the menus'.</param>
    private static void Show(Rarities rarity, bool inspectLabel)
    {
        var label = inspectLabel ? s_InspectLabel : s_Label;
        if (label == null)
            return;

        label.text = LocalizedName(rarity);

        var icon = inspectLabel ? s_InspectIcon : null;

        if (rarity == Rarities.None)
        {
            label.gameObject.SetActive(false);
            if (icon != null)
                icon.gameObject.SetActive(false);
            return;
        }

        Colour(label, rarity);
        if (icon != null)
            Colour(icon, rarity);
    }

    /// <summary>Shows a label or star in a rarity's colour, glowing for <see cref="Rarities.Mythic"/>.</summary>
    /// <param name="widget">The label or star.</param>
    /// <param name="rarity">The rarity.</param>
    private static void Colour(UIWidget widget, Rarities rarity)
    {
        widget.gameObject.SetActive(true);

        var glow = widget.GetComponent<MythicGlowEffect>();
        if (glow != null)
            UnityEngine.Object.Destroy(glow);

        if (rarity == Rarities.Mythic)
            widget.gameObject.AddComponent<MythicGlowEffect>();

        widget.color = RarityColours.Get(rarity);
    }

    /// <summary>Finds the icon beside the inspect view's condition: the sprite its label sits under.</summary>
    /// <param name="condition">The inspect view's condition label.</param>
    /// <returns>The icon, or <see langword="null"/> if there's none.</returns>
    private static UISprite? FindIcon(UILabel condition)
    {
        var parent = condition.transform.parent;
        return parent == null ? null : parent.GetComponent<UISprite>();
    }

    /// <summary>Moves a transform to a world x position, keeping its y and z.</summary>
    /// <param name="transform">The transform.</param>
    /// <param name="x">The world x position.</param>
    private static void SetWorldX(Transform transform, float x)
    {
        var position = transform.position;
        position.x = x;
        transform.position = position;
    }

    /// <summary>Gets a rarity's name in the player's language, from <c>Resources/Localization.json</c>.</summary>
    /// <param name="rarity">The rarity.</param>
    /// <returns>Its name, or an empty string for <see cref="Rarities.None"/>.</returns>
    private static string LocalizedName(Rarities rarity) => rarity switch
    {
        Rarities.Common => Localization.Get("GAMEPLAY_RarityCommon"),
        Rarities.Uncommon => Localization.Get("GAMEPLAY_RarityUncommon"),
        Rarities.Rare => Localization.Get("GAMEPLAY_RarityRare"),
        Rarities.Epic => Localization.Get("GAMEPLAY_RarityEpic"),
        Rarities.Legendary => Localization.Get("GAMEPLAY_RarityLegendary"),
        Rarities.Mythic => Localization.Get("GAMEPLAY_RarityMythic"),
        _ => string.Empty
    };
}
