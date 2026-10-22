using ModSettings;

using ItemRarities.Core;
using ItemRarities.Utilities;

namespace ItemRarities;

/// <summary>The mod's page in the game's Mod Settings menu, saved to <c>Mods\ItemRarities.json</c>.</summary>
/// <remarks>
/// ModSettings shows each public field as a setting, so the fields here are public. Their names are what the settings
/// file saves, so they keep their names rather than the <c>m_</c> prefix, or players would lose their settings. They're
/// read where they're used, by <see cref="RarityColours"/>, so changes show straight away.
/// </remarks>
internal sealed class Settings : JsonModSettings
{
    /// <summary>
    /// The settings for each rarity's custom colour: its red, green and blue sliders, and the 0 to 1 channels Item
    /// Rarities 2.0.5 and earlier saved, with their defaults.
    /// </summary>
    /// <remarks>Declared before <see cref="Instance"/>, so it's set before the constructor could reach <see cref="RefreshFields"/>.</remarks>
    private static readonly ColourFields[] Colours =
    [
        new(nameof(CommonRed), nameof(CommonGreen), nameof(CommonBlue), "Common",
            nameof(commonRed), nameof(commonGreen), nameof(commonBlue), 0.6f, 0.6f, 0.6f),
        new(nameof(UncommonRed), nameof(UncommonGreen), nameof(UncommonBlue), "Uncommon",
            nameof(uncommonRed), nameof(uncommonGreen), nameof(uncommonBlue), 0.3f, 0.7f, 0.0f),
        new(nameof(RareRed), nameof(RareGreen), nameof(RareBlue), "Rare",
            nameof(rareRed), nameof(rareGreen), nameof(rareBlue), 0.0f, 0.6f, 0.9f),
        new(nameof(EpicRed), nameof(EpicGreen), nameof(EpicBlue), "Epic",
            nameof(epicRed), nameof(epicGreen), nameof(epicBlue), 0.7f, 0.3f, 0.9f),
        new(nameof(LegendaryRed), nameof(LegendaryGreen), nameof(LegendaryBlue), "Legendary",
            nameof(legendaryRed), nameof(legendaryGreen), nameof(legendaryBlue), 0.9f, 0.5f, 0.2f),
        new(nameof(MythicRed), nameof(MythicGreen), nameof(MythicBlue), "Mythic",
            nameof(mythicRed), nameof(mythicGreen), nameof(mythicBlue), 0.8f, 0.7f, 0.3f),
    ];

    /// <summary>The settings, loaded from the file when the mod starts.</summary>
    internal static readonly Settings Instance = new();

    /// <summary>Whether the rarities use the colours below instead of the mod's own.</summary>
    [Name("Custom Colours")]
    [Description("Enabling this allows you to customise the colours of each rarity.")]
    public bool customColours = false;

    #region Common

    /// <summary>The red of <see cref="Enums.Rarities.Common"/>, from 0 to 255.</summary>
    [Section("Common")]
    [Name("Red")]
    [Slider(0, 255)]
    public int CommonRed = 153;

    /// <summary>The green of <see cref="Enums.Rarities.Common"/>, from 0 to 255.</summary>
    [Name("Green")]
    [Slider(0, 255)]
    public int CommonGreen = 153;

    /// <summary>The blue of <see cref="Enums.Rarities.Common"/>, from 0 to 255.</summary>
    [Name("Blue")]
    [Slider(0, 255)]
    public int CommonBlue = 153;


    #endregion

    #region Uncommon

    /// <summary>The red of <see cref="Enums.Rarities.Uncommon"/>, from 0 to 255.</summary>
    [Section("Uncommon")]
    [Name("Red")]
    [Slider(0, 255)]
    public int UncommonRed = 77;

    /// <summary>The green of <see cref="Enums.Rarities.Uncommon"/>, from 0 to 255.</summary>
    [Name("Green")]
    [Slider(0, 255)]
    public int UncommonGreen = 179;

    /// <summary>The blue of <see cref="Enums.Rarities.Uncommon"/>, from 0 to 255.</summary>
    [Name("Blue")]
    [Slider(0, 255)]
    public int UncommonBlue = 0;


    #endregion

    #region Rare

    /// <summary>The red of <see cref="Enums.Rarities.Rare"/>, from 0 to 255.</summary>
    [Section("Rare")]
    [Name("Red")]
    [Slider(0, 255)]
    public int RareRed = 0;

    /// <summary>The green of <see cref="Enums.Rarities.Rare"/>, from 0 to 255.</summary>
    [Name("Green")]
    [Slider(0, 255)]
    public int RareGreen = 153;

    /// <summary>The blue of <see cref="Enums.Rarities.Rare"/>, from 0 to 255.</summary>
    [Name("Blue")]
    [Slider(0, 255)]
    public int RareBlue = 230;


    #endregion

    #region Epic

    /// <summary>The red of <see cref="Enums.Rarities.Epic"/>, from 0 to 255.</summary>
    [Section("Epic")]
    [Name("Red")]
    [Slider(0, 255)]
    public int EpicRed = 179;

    /// <summary>The green of <see cref="Enums.Rarities.Epic"/>, from 0 to 255.</summary>
    [Name("Green")]
    [Slider(0, 255)]
    public int EpicGreen = 77;

    /// <summary>The blue of <see cref="Enums.Rarities.Epic"/>, from 0 to 255.</summary>
    [Name("Blue")]
    [Slider(0, 255)]
    public int EpicBlue = 230;


    #endregion

    #region Legendary

    /// <summary>The red of <see cref="Enums.Rarities.Legendary"/>, from 0 to 255.</summary>
    [Section("Legendary")]
    [Name("Red")]
    [Slider(0, 255)]
    public int LegendaryRed = 230;

    /// <summary>The green of <see cref="Enums.Rarities.Legendary"/>, from 0 to 255.</summary>
    [Name("Green")]
    [Slider(0, 255)]
    public int LegendaryGreen = 128;

    /// <summary>The blue of <see cref="Enums.Rarities.Legendary"/>, from 0 to 255.</summary>
    [Name("Blue")]
    [Slider(0, 255)]
    public int LegendaryBlue = 51;


    #endregion

    #region Mythic

    /// <summary>The red of <see cref="Enums.Rarities.Mythic"/>, from 0 to 255.</summary>
    [Section("Mythic")]
    [Name("Red")]
    [Slider(0, 255)]
    public int MythicRed = 204;

    /// <summary>The green of <see cref="Enums.Rarities.Mythic"/>, from 0 to 255.</summary>
    [Name("Green")]
    [Slider(0, 255)]
    public int MythicGreen = 179;

    /// <summary>The blue of <see cref="Enums.Rarities.Mythic"/>, from 0 to 255.</summary>
    [Name("Blue")]
    [Slider(0, 255)]
    public int MythicBlue = 77;


    #endregion

    #region Saved by 2.0.5 and earlier

    // The 0 to 1 colours of Item Rarities 2.0.5 and earlier, always hidden. They're kept so the settings file can still
    // be read, and so CarryOverOldColours can move a player's own colours to the sliders above, after which they go back
    // to their defaults. ModSettings needs a name on every field, so they keep theirs.

    /// <summary>The red of <see cref="Enums.Rarities.Common"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Red")]
    public float commonRed = 0.6f;

    /// <summary>The green of <see cref="Enums.Rarities.Common"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Green")]
    public float commonGreen = 0.6f;

    /// <summary>The blue of <see cref="Enums.Rarities.Common"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Blue")]
    public float commonBlue = 0.6f;

    /// <summary>The red of <see cref="Enums.Rarities.Uncommon"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Red")]
    public float uncommonRed = 0.3f;

    /// <summary>The green of <see cref="Enums.Rarities.Uncommon"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Green")]
    public float uncommonGreen = 0.7f;

    /// <summary>The blue of <see cref="Enums.Rarities.Uncommon"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Blue")]
    public float uncommonBlue = 0.0f;

    /// <summary>The red of <see cref="Enums.Rarities.Rare"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Red")]
    public float rareRed = 0.0f;

    /// <summary>The green of <see cref="Enums.Rarities.Rare"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Green")]
    public float rareGreen = 0.6f;

    /// <summary>The blue of <see cref="Enums.Rarities.Rare"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Blue")]
    public float rareBlue = 0.9f;

    /// <summary>The red of <see cref="Enums.Rarities.Epic"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Red")]
    public float epicRed = 0.7f;

    /// <summary>The green of <see cref="Enums.Rarities.Epic"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Green")]
    public float epicGreen = 0.3f;

    /// <summary>The blue of <see cref="Enums.Rarities.Epic"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Blue")]
    public float epicBlue = 0.9f;

    /// <summary>The red of <see cref="Enums.Rarities.Legendary"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Red")]
    public float legendaryRed = 0.9f;

    /// <summary>The green of <see cref="Enums.Rarities.Legendary"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Green")]
    public float legendaryGreen = 0.5f;

    /// <summary>The blue of <see cref="Enums.Rarities.Legendary"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Blue")]
    public float legendaryBlue = 0.2f;

    /// <summary>The red of <see cref="Enums.Rarities.Mythic"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Red")]
    public float mythicRed = 0.8f;

    /// <summary>The green of <see cref="Enums.Rarities.Mythic"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Green")]
    public float mythicGreen = 0.7f;

    /// <summary>The blue of <see cref="Enums.Rarities.Mythic"/> as 2.0.5 and earlier saved it, from 0 to 1.</summary>
    [Name("Blue")]
    public float mythicBlue = 0.3f;

    #endregion

    /// <summary>
    /// Adds the settings to the Mod Settings menu, after moving any colours saved by 2.0.5 and earlier to the sliders.
    /// </summary>
    internal static void Initialize()
    {
        if (Instance.CarryOverOldColours())
        {
            // Saved and read back, so ModSettings treats the new values as confirmed and keeps them when the menu closes.
            Instance.Save();
            Instance.Reload();
        }

        Instance.AddToModSettings(Properties.BuildInfo.Name);
        Instance.RefreshFields();
        Instance.RefreshGUI();

        Mod.Logger.Log("Settings loaded.", FlaggedLoggingLevel.Debug);
    }

    /// <inheritdoc />
    /// <remarks>Shows or hides the colour sliders when <see cref="customColours"/> changes.</remarks>
    protected override void OnChange(FieldInfo field, object? oldValue, object? newValue) => RefreshFields();

    /// <summary>Shows the colour settings only with <see cref="customColours"/> on, and never the old 0 to 1 ones.</summary>
    private void RefreshFields()
    {
        foreach (var colour in Colours)
        {
            foreach (var field in new[] { colour.Red, colour.Green, colour.Blue })
                SetFieldVisible(field, customColours);

            foreach (var field in new[] { colour.OldRed, colour.OldGreen, colour.OldBlue })
                SetFieldVisible(field, false);
        }
    }

    /// <summary>
    /// Moves each rarity's colour saved by 2.0.5 and earlier, on a 0 to 1 scale, to the 0 to 255 sliders, if the player
    /// changed it from its default. The old values then go back to their defaults, so it only happens once.
    /// </summary>
    /// <returns>Whether any colour was moved.</returns>
    private bool CarryOverOldColours()
    {
        var carriedOver = false;

        foreach (var colour in Colours)
        {
            var red = GetFloat(colour.OldRed);
            var green = GetFloat(colour.OldGreen);
            var blue = GetFloat(colour.OldBlue);
            if (IsDefault(red, colour.DefaultRed) && IsDefault(green, colour.DefaultGreen) && IsDefault(blue, colour.DefaultBlue))
                continue;

            SetInt(colour.Red, ColourCodes.FromUnit(red));
            SetInt(colour.Green, ColourCodes.FromUnit(green));
            SetInt(colour.Blue, ColourCodes.FromUnit(blue));

            SetFloat(colour.OldRed, colour.DefaultRed);
            SetFloat(colour.OldGreen, colour.DefaultGreen);
            SetFloat(colour.OldBlue, colour.DefaultBlue);

            Mod.Logger.Log($"Moved the {colour.Rarity} colour from the old 0 to 1 settings.", FlaggedLoggingLevel.Debug);
            carriedOver = true;
        }

        return carriedOver;

        static bool IsDefault(float value, float defaultValue) => Math.Abs(value - defaultValue) < 0.0005f;
    }

    /// <summary>Gets one of the settings' fields by name.</summary>
    /// <param name="name">The field's name.</param>
    /// <returns>The field.</returns>
    private static FieldInfo Field(string name) => typeof(Settings).GetField(name)!;

    /// <summary>Writes a whole-number setting, kept from 0 to 255.</summary>
    /// <param name="name">The field's name.</param>
    /// <param name="value">Its value.</param>
    private void SetInt(string name, int value) => Field(name).SetValue(this, Math.Clamp(value, 0, ColourCodes.MaxChannel));

    /// <summary>Reads a decimal setting.</summary>
    /// <param name="name">The field's name.</param>
    /// <returns>Its value.</returns>
    private float GetFloat(string name) => (float)Field(name).GetValue(this)!;

    /// <summary>Writes a decimal setting.</summary>
    /// <param name="name">The field's name.</param>
    /// <param name="value">Its value.</param>
    private void SetFloat(string name, float value) => Field(name).SetValue(this, value);

    /// <summary>The names of one rarity's colour settings, and the defaults of its old 0 to 1 ones.</summary>
    /// <param name="Red">The red slider.</param>
    /// <param name="Green">The green slider.</param>
    /// <param name="Blue">The blue slider.</param>
    /// <param name="Rarity">The rarity's name, for the log.</param>
    /// <param name="OldRed">The red saved by 2.0.5 and earlier.</param>
    /// <param name="OldGreen">The green saved by 2.0.5 and earlier.</param>
    /// <param name="OldBlue">The blue saved by 2.0.5 and earlier.</param>
    /// <param name="DefaultRed">The default of <paramref name="OldRed"/>.</param>
    /// <param name="DefaultGreen">The default of <paramref name="OldGreen"/>.</param>
    /// <param name="DefaultBlue">The default of <paramref name="OldBlue"/>.</param>
    private sealed record ColourFields(
        string Red, string Green, string Blue, string Rarity,
        string OldRed, string OldGreen, string OldBlue,
        float DefaultRed, float DefaultGreen, float DefaultBlue);
}
