using ItemRarities.Utilities;
using ModSettings;

namespace ItemRarities.Properties;

internal class Settings : JsonModSettings
{
    internal static Settings Instance { get; } = new();

    [Name("Custom Colours")] [Description("Enabling this allows you to customise the colours of each rarity.")]
    public bool customColours = false;

    #region Common
    [Section("Common")]
    [Name("Red")]
    public float commonRed = 0.6f;

    [Name("Green")]
    public float commonGreen = 0.6f;

    [Name("Blue")]
    public float commonBlue = 0.6f;
    #endregion

    #region Uncommon
    [Section("Uncommon")]
    [Name("Red")]
    public float uncommonRed = 0.3f;

    [Name("Green")]
    public float uncommonGreen = 0.7f;

    [Name("Blue")]
    public float uncommonBlue = 0.0f;
    #endregion
    
    #region Rare
    [Section("Rare")]
    [Name("Red")]
    public float rareRed = 0.0f;

    [Name("Green")]
    public float rareGreen = 0.6f;

    [Name("Blue")]
    public float rareBlue = 0.9f;
    #endregion
    
    #region Epic
    [Section("Epic")]
    [Name("Red")]
    public float epicRed = 0.7f;

    [Name("Green")]
    public float epicGreen = 0.3f;

    [Name("Blue")]
    public float epicBlue = 0.9f;
    #endregion
    
    #region Legendary
    [Section("Legendary")]
    [Name("Red")]
    public float legendaryRed = 0.9f;

    [Name("Green")]
    public float legendaryGreen = 0.5f;

    [Name("Blue")]
    public float legendaryBlue = 0.2f;
    #endregion
    
    #region Mythic
    [Section("Mythic")]
    [Name("Red")]
    public float mythicRed = 0.8f;

    [Name("Green")]
    public float mythicGreen = 0.7f;

    [Name("Blue")]
    public float mythicBlue = 0.3f;
    #endregion
    
    protected override void OnChange(FieldInfo field, object? oldValue, object? newValue) => RefreshFields();
    
    internal static void OnLoad()
    {
        Instance.AddToModSettings(BuildInfo.Name);
        Instance.RefreshFields();
        Instance.RefreshGUI();
    }

    private void RefreshFields() => SettingsUtilities.UpdateFieldVisibilities(customColours);
}