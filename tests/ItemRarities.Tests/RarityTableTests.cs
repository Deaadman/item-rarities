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
    public void Json_gives_each_item_its_groups_rarity()
    {
        RarityTable table = new();
        table.LoadJson("""
            {
              "Common": ["GEAR_Stick", "GEAR_Stone"],
              "Mythic": ["GEAR_Flaregun"]
            }
            """);

        Assert.Equal(Rarities.Common, table.Get("GEAR_Stick"));
        Assert.Equal(Rarities.Common, table.Get("GEAR_Stone"));
        Assert.Equal(Rarities.Mythic, table.Get("GEAR_Flaregun"));
        Assert.Equal(3, table.Count);
    }

    [Fact]
    public void Json_skips_keys_that_arent_a_rarity()
    {
        RarityTable table = new();
        table.LoadJson("""{ "Shiny": ["GEAR_Stick"], "Rare": ["GEAR_Stone"] }""");

        Assert.Equal(Rarities.None, table.Get("GEAR_Stick"));
        Assert.Equal(Rarities.Rare, table.Get("GEAR_Stone"));
    }

    [Fact]
    public void Json_allows_comments_and_trailing_commas()
    {
        RarityTable table = new();
        table.LoadJson("""
            {
              // The rarest.
              "Legendary": ["GEAR_Stick",],
            }
            """);

        Assert.Equal(Rarities.Legendary, table.Get("GEAR_Stick"));
    }

    [Fact]
    public void The_embedded_rarities_file_loads()
    {
        RarityTable table = new();
        table.LoadJson(File.ReadAllText(Path.Combine(RepositoryRoot(), "src", "Resources", "ItemRarities.json")));

        Assert.True(table.Count > 0);
        Assert.Equal(Rarities.Mythic, table.Get("GEAR_CougarClaw"));
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

    /// <summary>The repository's folder, found by walking up from the test's output folder.</summary>
    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "ItemRarities.slnx")))
            directory = directory.Parent;

        return directory?.FullName ?? throw new DirectoryNotFoundException("ItemRarities.slnx wasn't found above the tests.");
    }
}
