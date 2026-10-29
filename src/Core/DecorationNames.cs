namespace ItemRarities.Core;

/// <summary>Turns a decoration's name in the scene into the prefab name <c>Decorations.ir</c> lists it under.</summary>
internal static class DecorationNames
{
    /// <summary>What Unity adds to the name of an object made from a prefab while the game runs.</summary>
    private const string CloneSuffix = "(Clone)";

    /// <summary>
    /// Gets a decoration's prefab name from its name in the scene, without the tags in brackets added after it: the
    /// <c>(Clone)</c> Unity adds to one made while the game runs, the <c> (2)</c> it adds to a copy placed in a level,
    /// and the <c> (PLACED)</c> the game adds to one the player has placed. No prefab name in <c>Decorations.ir</c>
    /// has brackets.
    /// </summary>
    /// <param name="name">The decoration's name, like <c>OBJ_Baseball_Prefab (PLACED)</c>.</param>
    /// <returns>Its prefab name, like <c>OBJ_Baseball_Prefab</c>.</returns>
    internal static string PrefabName(string name)
    {
        var prefabName = name.Trim();

        while (true)
        {
            if (prefabName.EndsWith(CloneSuffix, StringComparison.Ordinal))
                prefabName = prefabName[..^CloneSuffix.Length].TrimEnd();
            else if (TagStart(prefabName) is var start and > 0)
                prefabName = prefabName[..start].TrimEnd();
            else
                return prefabName;
        }
    }

    /// <summary>Finds where a tag in brackets after the name, like the <c> (2)</c> in <c>OBJ_Cup (2)</c>, starts.</summary>
    /// <param name="name">The name.</param>
    /// <returns>The index of the space before the bracket, or -1 if the name doesn't end in a tag.</returns>
    private static int TagStart(string name)
    {
        if (!name.EndsWith(')'))
            return -1;

        var open = name.LastIndexOf(" (", StringComparison.Ordinal);
        return open < 0 || open + 2 == name.Length - 1 ? -1 : open;
    }
}
