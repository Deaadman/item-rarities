using LocalizationUtilities;

using ItemRarities.Managers;
using ItemRarities.Utilities;

namespace ItemRarities;

/// <summary>
/// The mod's entry point for MelonLoader. Most of what it does is in the Harmony patches in <see cref="Patches"/>,
/// which MelonLoader applies when it loads the mod. It loads the translations, sets up the settings and loads the
/// rarities once the main menu is up.
/// </summary>
internal sealed class Mod : MelonMod
{
    /// <summary>The manifest name of the embedded translations, read by ModComponent's localization.</summary>
    private const string LocalizationResource = "ItemRarities.Resources.Localization.json";

    /// <summary>
    /// The mod's logger. It writes to the MelonLoader console and <c>MelonLoader\Latest.log</c>, at the levels the
    /// player turns on in Complex Logger's settings.
    /// </summary>
    internal static readonly ComplexLogger.ComplexLogger<Mod> Logger = new();

    /// <inheritdoc />
    public override void OnInitializeMelon()
    {
        Logger.Log("Initializing Item Rarities.", FlaggedLoggingLevel.Debug);

        LoadLocalizations();
        Settings.Initialize();

        Logger.Log($"{Properties.BuildInfo.Name} {Properties.BuildInfo.Version} initialized.", FlaggedLoggingLevel.Debug);
    }

    /// <inheritdoc />
    /// <remarks>
    /// Loads the rarities the first time the main menu loads, by when other mods have added theirs with
    /// <see cref="RarityManager.AddGearItemAndRarity"/>.
    /// </remarks>
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (RarityManager.s_Initialized || !GameManager.IsMainMenuActive())
            return;

        MelonCoroutines.Start(RarityManager.InitializeRarities());
    }

    // Uncomment after a game update to log the new items that don't have a rarity yet.
    /*/// <inheritdoc />
    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        MissingRarities.LogItemsWithoutRarities();
        MissingRarities.LogDecorationNames();
    }*/

    /// <summary>Loads the mod's translations, like the rarities' names, from the embedded JSON file.</summary>
    private static void LoadLocalizations()
    {
        try
        {
            LocalizationManager.LoadJsonLocalization(EmbeddedResources.ReadText(LocalizationResource));
            Logger.Log("Loaded localizations.", FlaggedLoggingLevel.Debug);
        }
        catch (Exception ex)
        {
            Logger.Log("Failed to load localizations.", FlaggedLoggingLevel.Exception, ex);
        }
    }
}
