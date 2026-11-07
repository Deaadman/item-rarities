using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using UnityEditor;

using UnityEngine;

/// <summary>
/// Finds every prefab in the project with a GearItem on its root and writes them to GearItems.csv in the project's
/// folder once each, with its rarity from the Item Rarities repository. Logs the ones that have no rarity and aren't on
/// the mod's list of items that don't need one, like the mod's in-game MissingRarities check. Prefabs the exporter saved
/// twice, like "GEAR_Hatchet_0", merge into the original.
/// </summary>
public static class FindGearItems
{
    private const string OutputFile = "GearItems.csv";

    private const string NoRarity = "None (no-rarity list)";

    /// <summary>The "_0" the exporter adds to a prefab saved twice.</summary>
    private static readonly Regex ExportSuffix = new Regex(@"_(0|[1-9]\d*)$");

    private sealed class GearPrefab
    {
        public int Copies;
        public string Path;
    }

    [MenuItem("Tools/Item Rarities/Find Gear Items")]
    private static void Find()
    {
        var repository = ItemRaritiesRepository.Find();
        if (repository == null)
            return;

        var rarities = ItemRaritiesRepository.ReadRarities(Path.Combine(repository, "src", "Resources", "ItemRarities.ir"));
        var withoutRarities = ReadItemsWithoutRarities(Path.Combine(repository, "src", "Utilities", "MissingRarities.cs"));

        var guids = AssetDatabase.FindAssets("t:Prefab");
        var gearItems = new Dictionary<string, GearPrefab>(System.StringComparer.Ordinal);

        try
        {
            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (i % 50 == 0 && EditorUtility.DisplayCancelableProgressBar(
                        "Finding gear items", $"{i} of {guids.Length}: {path}", (float)i / guids.Length))
                {
                    Debug.LogWarning("Stopped finding gear items: cancelled.");
                    return;
                }

                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null || prefab.GetComponent<GearItem>() == null)
                    continue;

                if (gearItems.TryGetValue(prefab.name, out var found))
                    found.Copies++;
                else
                    gearItems[prefab.name] = new GearPrefab { Copies = 1, Path = path };
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

        var prefabCount = 0;
        foreach (var gearItem in gearItems.Values)
            prefabCount += gearItem.Copies;

        MergeExportCopies(gearItems);

        var names = new List<string>(gearItems.Keys);
        names.Sort(System.StringComparer.Ordinal);

        var csv = new StringBuilder("Prefab,Rarity,Copies,Path\n");
        var missing = 0;
        foreach (var name in names)
        {
            string rarity;
            if (rarities.TryGetValue(name, out var found))
                rarity = found;
            else if (withoutRarities.Contains(name))
                rarity = NoRarity;
            else
            {
                rarity = "";
                missing++;
                Debug.Log($"Gear item without a rarity: {name}");
            }

            csv.Append($"{ItemRaritiesRepository.Csv(name)},{ItemRaritiesRepository.Csv(rarity)},{gearItems[name].Copies},{ItemRaritiesRepository.Csv(gearItems[name].Path)}\n");
        }

        var outputPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", OutputFile));
        File.WriteAllText(outputPath, csv.ToString());

        Debug.Log($"Checked {guids.Length} prefabs: {prefabCount} gear item prefabs, {names.Count} unique, " +
                  $"{missing} without a rarity. Wrote {outputPath}");
        EditorUtility.RevealInFinder(outputPath);
    }

    /// <summary>
    /// Merges each "Name_0" the exporter saved into "Name" when both were found. Only then, so a name that really ends
    /// in a number stays.
    /// </summary>
    private static void MergeExportCopies(Dictionary<string, GearPrefab> gearItems)
    {
        foreach (var name in new List<string>(gearItems.Keys))
        {
            var original = ExportSuffix.Replace(name, "");
            if (original == name || !gearItems.TryGetValue(original, out var kept))
                continue;

            kept.Copies += gearItems[name].Copies;
            gearItems.Remove(name);
        }
    }

    /// <summary>Reads the gear names quoted in MissingRarities.cs, the items that don't need a rarity.</summary>
    private static HashSet<string> ReadItemsWithoutRarities(string path)
    {
        var names = new HashSet<string>(System.StringComparer.Ordinal);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Couldn't find {path}, so every item without a rarity is logged.");
            return names;
        }

        foreach (Match match in Regex.Matches(File.ReadAllText(path), "\"(GEAR_[^\"]+)\"", RegexOptions.IgnoreCase))
            names.Add(match.Groups[1].Value);

        return names;
    }
}
