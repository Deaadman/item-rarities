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

    /// <summary>The label in the menus, or <see langword="null"/> until one first shows an item.</summary>
    internal static UILabel? s_Label;

    /// <summary>The label in the inspect view, or <see langword="null"/> until the player first inspects an item.</summary>
    internal static UILabel? s_InspectLabel;

    /// <summary>Creates the inspect view's label.</summary>
    /// <param name="parent">The inspect view's details grid.</param>
    internal static void CreateInspectLabel(Transform parent)
    {
        GameObject gameObject = new("RarityLabelInspectGameObject");
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.localPosition = Vector3.zero;

        s_InspectLabel = UILabelExtensions.SetupGameObjectWithUILabel("RarityLabelInspect", gameObject.transform, false, true);
        s_InspectLabel.transform.SetSiblingIndex(0);
    }

    /// <summary>Lines the inspect view's label up with the left edge of the item's description.</summary>
    /// <param name="description">The inspect view's description label.</param>
    internal static void AlignInspectLabel(UILabel description)
    {
        if (s_InspectLabel == null || description == null)
            return;

        var position = s_InspectLabel.transform.position;
        position.x = description.worldCorners[0].x;
        s_InspectLabel.transform.position = position;
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
    /// Shows an item's rarity on a label, in its colour, glowing for <see cref="Rarities.Mythic"/>. Hides the label if
    /// the item has no rarity.
    /// </summary>
    /// <param name="gearItem">The item, or <see langword="null"/> to leave the label as it is.</param>
    /// <param name="inspectLabel">Whether to update the inspect view's label rather than the menus'.</param>
    internal static void Show(GearItem? gearItem, bool inspectLabel = false)
    {
        if (gearItem == null)
            return;

        var label = inspectLabel ? s_InspectLabel : s_Label;
        if (label == null)
            return;

        var rarity = RarityManager.GetRarity(gearItem.name);
        label.text = LocalizedName(rarity);

        if (rarity == Rarities.None)
        {
            label.gameObject.SetActive(false);
            return;
        }

        label.gameObject.SetActive(true);

        var glow = label.GetComponent<MythicGlowEffect>();
        if (glow != null)
            UnityEngine.Object.Destroy(glow);

        if (rarity == Rarities.Mythic)
            label.gameObject.AddComponent<MythicGlowEffect>();

        label.color = RarityColours.Get(rarity);
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
