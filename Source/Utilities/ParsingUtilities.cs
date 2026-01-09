using ItemRarities.Enums;
using ItemRarities.Managers;
using Newtonsoft.Json.Linq;

namespace ItemRarities.Utilities;

internal static class ParsingUtilities
{
    internal static void LoadRaritiesFromIR(StreamReader reader)
    {
        var currentRarity = Rarities.None;

        while (reader.ReadLine() is { } line)
        {
            line = line.Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue;

            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                var rarityString = line.Trim('[', ']');
                if (Enum.TryParse(rarityString, out Rarities rarity))
                {
                    currentRarity = rarity;
                }
            }
            else if (currentRarity != Rarities.None)
            {
                RarityManager.raritiesLookup[line] = currentRarity;
            }
        }
    }
    
    internal static void LoadRaritiesFromJson(string jsonText)
    {
        var jsonObject = JObject.Parse(jsonText);
        foreach (var rarityGroup in jsonObject)
        {
            if (!Enum.TryParse(rarityGroup.Key, out Rarities rarity) || rarityGroup.Value == null) continue;
            foreach (var item in rarityGroup.Value)
            {
                RarityManager.raritiesLookup[item.ToString()] = rarity;
            }
        }
    }
    
    internal static void ReadJSON(string jsonFilePath, out string jsonText)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(jsonFilePath);
        if (stream is null)
        {
            jsonText = string.Empty;
            return;
        }
        
        using StreamReader reader = new(stream);
        
        jsonText = reader.ReadToEnd();
    }
}