namespace ItemRarities.Utilities;

/// <summary>Reads files embedded in the mod's DLL.</summary>
internal static class EmbeddedResources
{
    /// <summary>Reads an embedded text file.</summary>
    /// <param name="name">The resource's manifest name, like <c>ItemRarities.Resources.Localization.json</c>.</param>
    /// <returns>The file's text, or an empty string if it's missing, which is logged.</returns>
    internal static string ReadText(string name)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        if (stream == null)
        {
            Mod.Logger.Log($"Embedded resource '{name}' was not found.", FlaggedLoggingLevel.Error);
            return string.Empty;
        }

        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
