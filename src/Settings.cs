using ModSettings;

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
    /// <summary>The colour settings, only shown with <see cref="customColours"/> on.</summary>
    /// <remarks>Declared before <see cref="Instance"/>, so it's set before the constructor could reach <see cref="RefreshFields"/>.</remarks>
    private static readonly string[] ColourFields =
    [
        nameof(commonRed), nameof(commonGreen), nameof(commonBlue),
        nameof(uncommonRed), nameof(uncommonGreen), nameof(uncommonBlue),
        nameof(rareRed), nameof(rareGreen), nameof(rareBlue),
        nameof(epicRed), nameof(epicGreen), nameof(epicBlue),
        nameof(legendaryRed), nameof(legendaryGreen), nameof(legendaryBlue),
        nameof(mythicRed), nameof(mythicGreen), nameof(mythicBlue),
    ];

    /// <summary>The settings, loaded from the file when the mod starts.</summary>
    internal static readonly Settings Instance = new();

    /// <summary>Whether the rarities use the colours below instead of the mod's own.</summary>
    [Name("Custom Colours")]
    [Description("Enabling this allows you to customise the colours of each rarity.")]
    public bool customColours = false;

    #region Common

    /// <summary>The red of <see cref="Enums.Rarities.Common"/>, from 0 to 1.</summary>
    [Section("Common")]
    [Name("Red")]
    public float commonRed = 0.6f;

    /// <summary>The green of <see cref="Enums.Rarities.Common"/>, from 0 to 1.</summary>
    [Name("Green")]
    public float commonGreen = 0.6f;

    /// <summary>The blue of <see cref="Enums.Rarities.Common"/>, from 0 to 1.</summary>
    [Name("Blue")]
    public float commonBlue = 0.6f;

    #endregion

    #region Uncommon

    /// <summary>The red of <see cref="Enums.Rarities.Uncommon"/>, from 0 to 1.</summary>
    [Section("Uncommon")]
    [Name("Red")]
    public float uncommonRed = 0.3f;

    /// <summary>The green of <see cref="Enums.Rarities.Uncommon"/>, from 0 to 1.</summary>
    [Name("Green")]
    public float uncommonGreen = 0.7f;

    /// <summary>The blue of <see cref="Enums.Rarities.Uncommon"/>, from 0 to 1.</summary>
    [Name("Blue")]
    public float uncommonBlue = 0.0f;

    #endregion

    #region Rare

    /// <summary>The red of <see cref="Enums.Rarities.Rare"/>, from 0 to 1.</summary>
    [Section("Rare")]
    [Name("Red")]
    public float rareRed = 0.0f;

    /// <summary>The green of <see cref="Enums.Rarities.Rare"/>, from 0 to 1.</summary>
    [Name("Green")]
    public float rareGreen = 0.6f;

    /// <summary>The blue of <see cref="Enums.Rarities.Rare"/>, from 0 to 1.</summary>
    [Name("Blue")]
    public float rareBlue = 0.9f;

    #endregion

    #region Epic

    /// <summary>The red of <see cref="Enums.Rarities.Epic"/>, from 0 to 1.</summary>
    [Section("Epic")]
    [Name("Red")]
    public float epicRed = 0.7f;

    /// <summary>The green of <see cref="Enums.Rarities.Epic"/>, from 0 to 1.</summary>
    [Name("Green")]
    public float epicGreen = 0.3f;

    /// <summary>The blue of <see cref="Enums.Rarities.Epic"/>, from 0 to 1.</summary>
    [Name("Blue")]
    public float epicBlue = 0.9f;

    #endregion

    #region Legendary

    /// <summary>The red of <see cref="Enums.Rarities.Legendary"/>, from 0 to 1.</summary>
    [Section("Legendary")]
    [Name("Red")]
    public float legendaryRed = 0.9f;

    /// <summary>The green of <see cref="Enums.Rarities.Legendary"/>, from 0 to 1.</summary>
    [Name("Green")]
    public float legendaryGreen = 0.5f;

    /// <summary>The blue of <see cref="Enums.Rarities.Legendary"/>, from 0 to 1.</summary>
    [Name("Blue")]
    public float legendaryBlue = 0.2f;

    #endregion

    #region Mythic

    /// <summary>The red of <see cref="Enums.Rarities.Mythic"/>, from 0 to 1.</summary>
    [Section("Mythic")]
    [Name("Red")]
    public float mythicRed = 0.8f;

    /// <summary>The green of <see cref="Enums.Rarities.Mythic"/>, from 0 to 1.</summary>
    [Name("Green")]
    public float mythicGreen = 0.7f;

    /// <summary>The blue of <see cref="Enums.Rarities.Mythic"/>, from 0 to 1.</summary>
    [Name("Blue")]
    public float mythicBlue = 0.3f;

    #endregion

    /// <summary>Adds the settings to the Mod Settings menu.</summary>
    internal static void Initialize()
    {
        Instance.AddToModSettings(Properties.BuildInfo.Name);
        Instance.RefreshFields();
        Instance.RefreshGUI();

        Mod.Logger.Log("Settings loaded.", FlaggedLoggingLevel.Debug);
    }

    /// <inheritdoc />
    protected override void OnChange(FieldInfo field, object? oldValue, object? newValue) => RefreshFields();

    /// <summary>Shows the colour settings only with <see cref="customColours"/> on.</summary>
    private void RefreshFields()
    {
        foreach (var field in ColourFields)
            SetFieldVisible(field, customColours);
    }
}
