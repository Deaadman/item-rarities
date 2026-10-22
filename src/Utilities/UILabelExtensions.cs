namespace ItemRarities.Utilities;

/// <summary>Helpers for the game's NGUI labels.</summary>
internal static class UILabelExtensions
{
    /// <summary>Creates a game object with an empty label in the game's font, for a rarity.</summary>
    /// <param name="gameObjectName">The game object's name.</param>
    /// <param name="parent">Its parent.</param>
    /// <param name="worldPositionStays">Whether it keeps its world position when parented.</param>
    /// <param name="inspectLabel">
    /// Whether it's for the inspect view: left-aligned, growing to the right of its position, and smaller, instead of
    /// centred.
    /// </param>
    /// <param name="posX">Its x position under the parent.</param>
    /// <param name="posY">Its y position under the parent.</param>
    /// <param name="posZ">Its z position under the parent.</param>
    /// <returns>The label.</returns>
    internal static UILabel SetupGameObjectWithUILabel(string gameObjectName, Transform parent, bool worldPositionStays, bool inspectLabel = false, float posX = 0f, float posY = 0f, float posZ = 0f)
    {
        GameObject gameObject = new(gameObjectName);
        var uiLabel = gameObject.AddComponent<UILabel>();
        gameObject.transform.SetParent(parent, worldPositionStays);
        gameObject.transform.localPosition = new Vector3(posX, posY, posZ);

        SetupUILabel(
            uiLabel,
            string.Empty,
            FontStyle.Normal,
            UILabel.Crispness.Always,
            inspectLabel ? NGUIText.Alignment.Left : NGUIText.Alignment.Center,
            UILabel.Overflow.ResizeFreely,
            true,
            inspectLabel ? 2 : 0,
            inspectLabel ? 14 : 18,
            Color.clear,
            true);

        // Grows to the right from its position, so it can line up with the inspect view's other details.
        if (inspectLabel)
            uiLabel.pivot = UIWidget.Pivot.Left;

        return uiLabel;
    }

    /// <summary>Sets up a label in the font for the player's language.</summary>
    /// <param name="label">The label.</param>
    /// <param name="text">Its text.</param>
    /// <param name="fontStyle">Its font style.</param>
    /// <param name="crispness">When it stays crisp as it shrinks.</param>
    /// <param name="alignment">Its alignment.</param>
    /// <param name="overflow">What it does when the text doesn't fit.</param>
    /// <param name="multiLine">Whether the text can wrap.</param>
    /// <param name="depth">Its depth, drawing over lower ones.</param>
    /// <param name="fontSize">Its font size.</param>
    /// <param name="color">Its colour.</param>
    /// <param name="capsLock">Whether it's in capitals.</param>
    private static void SetupUILabel(
        UILabel label,
        string text,
        FontStyle fontStyle,
        UILabel.Crispness crispness,
        NGUIText.Alignment alignment,
        UILabel.Overflow overflow,
        bool multiLine,
        int depth,
        int fontSize,
        Color color,
        bool capsLock)
    {
        var font = GameManager.GetFontManager().GetUIFontForCharacterSet(FontManager.m_CurrentCharacterSet);

        label.text = text;
        label.ambigiousFont = font;
        label.bitmapFont = font;
        label.font = font;

        label.fontStyle = fontStyle;
        label.keepCrispWhenShrunk = crispness;
        label.alignment = alignment;
        label.overflowMethod = overflow;
        label.multiLine = multiLine;
        label.depth = depth;
        label.fontSize = fontSize;
        label.color = color;
        label.capsLock = capsLock;
    }
}
