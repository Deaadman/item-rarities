using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;

/// <summary>
/// The Item Rarities repository, which the editor scripts read the mod's rarities from. Asked for the first time it's
/// needed, then remembered in the editor's preferences.
/// </summary>
public static class ItemRaritiesRepository
{
    private const string PathPreference = "ItemRarities.RepositoryPath";

    /// <summary>A file every copy of the repository has, to check the chosen folder is one.</summary>
    private static readonly string ItemsFile = Path.Combine("src", "Resources", "ItemRarities.ir");

    /// <summary>Gets the repository's folder, asking for it if it isn't known or has moved.</summary>
    /// <returns>The folder, or null if none was chosen.</returns>
    public static string Find()
    {
        var path = EditorPrefs.GetString(PathPreference, "");
        if (IsRepository(path))
            return path;

        path = EditorUtility.OpenFolderPanel("Choose the Item Rarities repository", "", "");
        if (!IsRepository(path))
        {
            Debug.LogWarning($"'{path}' isn't the Item Rarities repository: it has no {ItemsFile}.");
            return null;
        }

        EditorPrefs.SetString(PathPreference, path);
        return path;
    }

    [MenuItem("Tools/Item Rarities/Choose Repository Folder")]
    private static void Choose()
    {
        EditorPrefs.DeleteKey(PathPreference);
        var path = Find();
        if (path != null)
            Debug.Log($"Using the Item Rarities repository at {path}");
    }

    /// <summary>Reads an .ir file: [Rarity] lines, then the names with that rarity, one a line.</summary>
    /// <returns>Each name's rarity, or none if the file isn't there.</returns>
    public static Dictionary<string, string> ReadRarities(string path)
    {
        var rarities = new Dictionary<string, string>(System.StringComparer.Ordinal);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Couldn't find {path}, so no rarities are filled in.");
            return rarities;
        }

        var rarity = "";
        foreach (var rawLine in File.ReadAllLines(path))
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith("#"))
                continue;

            if (line.StartsWith("[") && line.EndsWith("]"))
                rarity = line.Substring(1, line.Length - 2);
            else
                rarities[line] = rarity;
        }

        return rarities;
    }

    /// <summary>Quotes a CSV field if it needs it.</summary>
    public static string Csv(string value) =>
        value.IndexOfAny(new[] { ',', '"', '\n' }) < 0 ? value : $"\"{value.Replace("\"", "\"\"")}\"";

    private static bool IsRepository(string path) =>
        !string.IsNullOrEmpty(path) && File.Exists(Path.Combine(path, ItemsFile));
}
