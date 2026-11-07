using ItemRarities.Core;

namespace ItemRarities.Tests;

public class DecorationNamesTests
{
    [Theory]
    [InlineData("OBJ_Baseball_Prefab", "OBJ_Baseball_Prefab")]
    [InlineData("OBJ_Baseball_Prefab (2)", "OBJ_Baseball_Prefab")]
    [InlineData("OBJ_Baseball_Prefab(Clone)", "OBJ_Baseball_Prefab")]
    [InlineData("OBJ_GunRackDouble_Prefab (PLACED)", "OBJ_GunRackDouble_Prefab")]
    [InlineData("OBJ_Baseball_Prefab (12)(Clone) (PLACED)", "OBJ_Baseball_Prefab")]
    [InlineData("  OBJ_Baseball_Prefab  ", "OBJ_Baseball_Prefab")]
    public void Prefab_name_drops_the_tags_after_the_name(string name, string prefabName) =>
        Assert.Equal(prefabName, DecorationNames.PrefabName(name));

    [Theory]
    [InlineData("OBJ_Thomson_Foreclosure_A_Prefab Variant")]
    [InlineData("OBJ_Traffic_Cone_A_2")]
    [InlineData("OBJ_Sign ()")]
    [InlineData("(Odd)")]
    public void Prefab_name_keeps_names_without_a_tag(string name) =>
        Assert.Equal(name, DecorationNames.PrefabName(name));
}
