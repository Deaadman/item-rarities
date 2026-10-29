using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using UnityEditor;

using UnityEngine;

/// <summary>
/// Finds every decoration the player can carry, a DecorationItem with m_AllowInInventory ticked, and writes each one
/// once to CarryableDecorations.csv in the project's folder, with its rarity from the Item Rarities repository's
/// Decorations.ir. Logs the ones that have no rarity.
/// </summary>
/// <remarks>
/// The export flattened nested prefabs, so a decoration placed in a building is a plain copy inside that building's
/// prefab, named like "OBJ_Baseball_Prefab (2)", and the exporter saved some prefabs twice, like "DECO_FramedPhoto_B_0".
/// Each decoration is named for its own object without the tags in brackets after the name, the way the mod looks them
/// up, so the copies merge into one row. A decoration inside a gear item, like an ID card's picture, is named for the
/// gear item instead, since that's what the player carries, and its rarity belongs in ItemRarities.ir.
/// </remarks>
public static class FindCarryableDecorations
{
    private const string OutputFile = "CarryableDecorations.csv";

    private const string GearItem = "Gear item (ItemRarities.ir)";

    /// <summary>The tags in brackets after a name, like " (2)", "(Clone)" and " (PLACED)".</summary>
    private static readonly Regex Tags = new Regex(@"(\s*\([^()]+\))+$");

    /// <summary>The "_0" the exporter adds to a prefab saved twice.</summary>
    private static readonly Regex ExportSuffix = new Regex(@"_(0|[1-9]\d*)$");

    private sealed class Decoration
    {
        public int Copies;
        public string Path;
        public bool IsOwnPrefab;
    }

    [MenuItem("Tools/Item Rarities/Find Carryable Decorations")]
    private static void Find()
    {
        var repository = ItemRaritiesRepository.Find();
        if (repository == null)
            return;

        var rarities = ItemRaritiesRepository.ReadRarities(Path.Combine(repository, "src", "Resources", "Decorations.ir"));

        var guids = AssetDatabase.FindAssets("t:Prefab");
        var decorations = new Dictionary<string, Decoration>(System.StringComparer.Ordinal);
        var withDecorationItem = 0;
        var carryable = 0;

        try
        {
            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (i % 50 == 0 && EditorUtility.DisplayCancelableProgressBar(
                        "Finding carryable decorations", $"{i} of {guids.Length}: {path}", (float)i / guids.Length))
                {
                    Debug.LogWarning("Stopped finding carryable decorations: cancelled.");
                    return;
                }

                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                    continue;

                var isGearItem = prefab.GetComponent<GearItem>() != null;
                foreach (var decoration in prefab.GetComponentsInChildren<DecorationItem>(true))
                {
                    withDecorationItem++;

                    // The decompiled AllowInInventory property is a stub that always returns false, so read the saved field.
                    var allowInInventory = new SerializedObject(decoration).FindProperty("m_AllowInInventory");
                    if (allowInInventory == null || !allowInInventory.boolValue)
                        continue;

                    carryable++;
                    var isOwnPrefab = isGearItem || decoration.transform == prefab.transform;
                    var name = Tags.Replace(isOwnPrefab ? prefab.name : decoration.name, "").Trim();

                    if (!decorations.TryGetValue(name, out var found))
                        decorations[name] = found = new Decoration { Path = path, IsOwnPrefab = isOwnPrefab };
                    else if (isOwnPrefab && !found.IsOwnPrefab)
                    {
                        // Point at the decoration's own prefab rather than a building it was placed in.
                        found.Path = path;
                        found.IsOwnPrefab = true;
                    }

                    found.Copies++;
                }
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

        MergeExportCopies(decorations);

        var names = new List<string>(decorations.Keys);
        names.Sort(System.StringComparer.Ordinal);

        var csv = new StringBuilder("Decoration,Rarity,Copies,Path\n");
        var missing = 0;
        foreach (var name in names)
        {
            string rarity;
            if (name.StartsWith("GEAR_", System.StringComparison.OrdinalIgnoreCase))
                rarity = GearItem;
            else if (!rarities.TryGetValue(name, out rarity))
            {
                rarity = "";
                missing++;
                Debug.Log($"Decoration without a rarity: {name}");
            }

            csv.Append($"{ItemRaritiesRepository.Csv(name)},{ItemRaritiesRepository.Csv(rarity)},{decorations[name].Copies},{ItemRaritiesRepository.Csv(decorations[name].Path)}\n");
        }

        var outputPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", OutputFile));
        File.WriteAllText(outputPath, csv.ToString());

        Debug.Log($"Checked {guids.Length} prefabs: {withDecorationItem} DecorationItems, {carryable} carryable, " +
                  $"{names.Count} unique, {missing} without a rarity. Wrote {outputPath}");
        EditorUtility.RevealInFinder(outputPath);
    }

    /// <summary>
    /// Merges each "Name_0" into "Name" when both were found. Only then, so a name that really ends in a number stays.
    /// </summary>
    private static void MergeExportCopies(Dictionary<string, Decoration> decorations)
    {
        foreach (var name in new List<string>(decorations.Keys))
        {
            var original = ExportSuffix.Replace(name, "");
            if (original == name || !decorations.TryGetValue(original, out var kept))
                continue;

            kept.Copies += decorations[name].Copies;
            decorations.Remove(name);
        }
    }
}
