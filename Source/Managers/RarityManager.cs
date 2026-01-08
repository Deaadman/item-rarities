using ItemRarities.Enums;
using ItemRarities.Utilities;
using System.Collections;
using System.IO.Compression;

namespace ItemRarities.Managers;

public static class RarityManager
{
    internal static bool isInitialized;
    internal static Dictionary<string, Rarities> raritiesLookup = new();

    // These XML Documentation isn't included in the build unless specified.
    // If so, then it'll generate an XML document which the modders will need.
    // To prevent this, I might just package this mod as a NuGet package, so they don't have to worry about it.
    
    /// <summary>
    /// Adds a Rarity to your custom GearItem.
    /// </summary>
    /// <param name="gearItem">The name of the GearItem to add.</param>
    /// <param name="rarity">The rarity of the GearItem, represented by the Rarities enum.</param>
    public static void AddGearItemAndRarity(string gearItem, Rarities rarity)
    {
        raritiesLookup[gearItem] = rarity;
    }
    
    private static IEnumerator AssignRarities()
    {
        var gearNames = new SortedSet<string>();
        var enumerator = ConsoleManager.m_SearchStringToGearNames._values.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var name = enumerator._currentValue;
            if (name != null && name.StartsWith("GEAR_"))
            {
                gearNames.Add(name);
            }
        }
        
        yield return null;
    }
    
    internal static Rarities GetRarity(string itemName) => raritiesLookup.GetValueOrDefault(itemName, Rarities.None);
    
    internal static IEnumerator InitializeRarities()
    {
        yield return LoadRaritiesFromLocalFile();
        LoadRaritiesFromModComponents();
        yield return AssignRarities();
        isInitialized = true;
    }
    
    private static IEnumerator LoadRaritiesFromLocalFile()
    {
        ParsingUtilities.ReadJSON("ItemRarities.Resources.ItemRarities.json", out var jsonText);
        ParsingUtilities.LoadRaritiesFromJson(jsonText);
        yield break;
    }
    
    private static void LoadRaritiesFromModComponents()
    {
        var modDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (modDirectory == null) return;

        var modComponentFiles = Directory.GetFiles(modDirectory, "*.modcomponent");

        foreach (var modComponentFile in modComponentFiles)
        {
            using var archive = ZipFile.OpenRead(modComponentFile);
            var itemRaritiesEntry = archive.GetEntry("rarities.ir");
            if (itemRaritiesEntry == null) continue;
            using var reader = new StreamReader(itemRaritiesEntry.Open());
            ParsingUtilities.LoadRaritiesFromIR(reader);
        }
    }
}