using ItemRarities.Core;
using ItemRarities.Enums;

namespace ItemRarities.Tests;

public class RarityTableTests
{
    [Fact]
    public void Items_without_a_rarity_have_none() =>
        Assert.Equal(Rarities.None, new RarityTable().Get("GEAR_Unknown"));

    [Fact]
    public void Setting_a_rarity_replaces_the_old_one()
    {
        RarityTable table = new();
        table.Set("GEAR_Hatchet", Rarities.Common);
        table.Set("GEAR_Hatchet", Rarities.Epic);

        Assert.Equal(Rarities.Epic, table.Get("GEAR_Hatchet"));
        Assert.Equal(1, table.Count);
    }

    [Fact]
    public void The_mods_rarities_file_loads()
    {
        RarityTable table = new();
        using (var reader = File.OpenText(ItemRaritiesFile()))
            table.LoadIr(reader);

        Assert.True(table.Count > 0);
        Assert.Equal(Rarities.Mythic, table.Get("GEAR_CougarClaw"));
    }

    [Fact]
    public void The_mods_decorations_file_loads()
    {
        RarityTable table = new();
        using (var reader = File.OpenText(RaritiesFile(DecorationsFileName)))
            table.LoadIr(reader);

        Assert.True(table.Count > 0);
        Assert.Equal(Rarities.Mythic, table.Get("OBJ_Wood_Statue_Cougar_Prefab"));
    }

    [Fact]
    public void The_mods_decorations_file_leaves_gear_items_to_the_items_file()
    {
        var gearItems = ItemNames(File.ReadAllLines(RaritiesFile(DecorationsFileName)))
            .Where(name => name.StartsWith("GEAR_", StringComparison.OrdinalIgnoreCase));

        Assert.Empty(gearItems);
    }

    [Fact]
    public void The_mods_decorations_file_has_no_names_the_lookup_would_cut_short()
    {
        var changed = ItemNames(File.ReadAllLines(RaritiesFile(DecorationsFileName)))
            .Where(name => DecorationNames.PrefabName(name) != name);

        Assert.Empty(changed);
    }

    [Theory]
    [InlineData(ItemsFileName)]
    [InlineData(DecorationsFileName)]
    public void The_mods_rarities_files_list_each_name_once(string fileName)
    {
        var duplicates = ItemNames(File.ReadAllLines(RaritiesFile(fileName)))
            .GroupBy(name => name)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key);

        Assert.Empty(duplicates);
    }

    [Theory]
    [InlineData(ItemsFileName)]
    [InlineData(DecorationsFileName)]
    public void The_mods_rarities_files_only_have_real_rarities_as_headings(string fileName)
    {
        var headings = File.ReadAllLines(RaritiesFile(fileName))
            .Select(line => line.Trim())
            .Where(line => line.StartsWith('[') && line.EndsWith(']'))
            .Select(line => line.Trim('[', ']'));

        Assert.All(headings, heading =>
            Assert.True(Enum.TryParse(heading, out Rarities rarity) && rarity != Rarities.None,
                $"[{heading}] isn't a rarity"));
    }

    [Fact]
    public void Ir_gives_each_name_the_rarity_above_it()
    {
        RarityTable table = new();
        table.LoadIr(new StringReader("""
            # My items
            [Rare]
            GEAR_MyKnife

              GEAR_MyAxe
            [Mythic]
            GEAR_MyGun
            """));

        Assert.Equal(Rarities.Rare, table.Get("GEAR_MyKnife"));
        Assert.Equal(Rarities.Rare, table.Get("GEAR_MyAxe"));
        Assert.Equal(Rarities.Mythic, table.Get("GEAR_MyGun"));
        Assert.Equal(3, table.Count);
    }

    [Fact]
    public void Ir_skips_names_without_a_rarity()
    {
        RarityTable table = new();
        table.LoadIr(new StringReader("""
            GEAR_BeforeAnyRarity
            [None]
            GEAR_UnderNone
            """));

        Assert.Equal(0, table.Count);
    }

    [Fact]
    public void Ir_keeps_the_rarity_through_a_heading_that_isnt_one()
    {
        RarityTable table = new();
        table.LoadIr(new StringReader("""
            [Epic]
            GEAR_First
            [Shiny]
            GEAR_Second
            """));

        Assert.Equal(Rarities.Epic, table.Get("GEAR_Second"));
    }

    /// <summary>The file of the game's items' rarities.</summary>
    private const string ItemsFileName = "ItemRarities.ir";

    /// <summary>The file of the rarities of the decorations the player can carry.</summary>
    private const string DecorationsFileName = "Decorations.ir";

    /// <summary>The path of <c>src/Resources/ItemRarities.ir</c>, the rarities of the game's items.</summary>
    private static string ItemRaritiesFile() => RaritiesFile(ItemsFileName);

    /// <summary>The path of one of the mod's rarities files in <c>src/Resources</c>.</summary>
    private static string RaritiesFile(string fileName) => Path.Combine(RepositoryRoot(), "src", "Resources", fileName);

    /// <summary>The names in an <c>.ir</c> file's lines, skipping headings, comments and blank lines.</summary>
    private static IEnumerable<string> ItemNames(IEnumerable<string> lines) => lines
        .Select(line => line.Trim())
        .Where(line => line.Length > 0 && !line.StartsWith('#') && !line.StartsWith('['));

    /// <summary>The repository's folder, found by walking up from the test's output folder.</summary>
    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "ItemRarities.slnx")))
            directory = directory.Parent;

        return directory?.FullName ?? throw new DirectoryNotFoundException("ItemRarities.slnx wasn't found above the tests.");
    }
}
