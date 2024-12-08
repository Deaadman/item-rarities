using ItemRarities.Managers;
using ItemRarities.Properties;
using ItemRarities.Utilities;
using LocalizationUtilities;

namespace ItemRarities;

internal sealed class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
        LoadLocalizations();
        Settings.OnLoad();
    }

    // Uncomment this when required, e.g. after a new update drops to see which items don't have a rarity.
    /*public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        Logging.LogItemsWithoutRarities();
        Logging.LogDecorationNames();
    }*/

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (RarityManager.isInitialized) return;
        if (!GameManager.IsMainMenuActive()) return;
        
        MelonCoroutines.Start(RarityManager.InitializeRarities());
    }
    
    private static void LoadLocalizations()
    {
        ParsingUtilities.ReadJSON("ItemRarities.Resources.Localization.json", out var jsonText);
        LocalizationManager.LoadJsonLocalization(jsonText);
    }
}