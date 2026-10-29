using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;

using ItemRarities.Enums;
using ItemRarities.Managers;

namespace ItemRarities.Utilities;

/// <summary>
/// Developer checks for after a game update: which new items still need a rarity in
/// <c>Resources/ItemRarities.ir</c>, and how the game names its decorations. Called once the rarities load, from the call in
/// <see cref="Mod"/> that's commented out when not in use.
/// </summary>
internal static class MissingRarities
{
    /// <summary>Items that don't need a rarity: notes, keys, postcards, story items and placeholders.</summary>
    private static readonly HashSet<string> ItemsWithoutRarities =
    [
        "GEAR_WolfCarcass",
        "GEAR_NewsprintBootStuffing",
        "GEAR_NewsprintInsulation",
        "GEAR_TechnicalBackpack",
        "GEAR_BoltCutters",
        "GEAR_CookingPotDummy",
        "GEAR_SpearHead",
        "GEAR_BearSpear",
        "GEAR_BearSpearBroken",
        "GEAR_BearSpearBrokenStory",
        "GEAR_BearSpearStory",
        "GEAR_GoldNugget",
        "Gear_Travois_Dummy",
        "GEAR_AccelerantGunpowder",
        "GEAR_CattailPlant",
        "GEAR_MountainTownFarmKey",
        "GEAR_PostCard_AC_CentralSpire",
        "GEAR_PostCard_AC_TopShelf",
        "GEAR_PostCard_BI_EchoOne-RadioTower",
        "GEAR_PostCard_BR_Canyon",
        "GEAR_PostCard_BR_Prison",
        "GEAR_PostCard_CR_AbandonedLookout",
        "GEAR_PostCard_FM_MuskegOverlook",
        "GEAR_PostCard_FM_ShortwaveTower",
        "GEAR_PostCard_ML_ForestryLookout",
        "GEAR_PostCard_ML_LakeOverlook",
        "GEAR_PostCard_MT_RadioTower",
        "GEAR_PostCard_PV_SignalHill",
        "GEAR_PostCard_RV_Pensive",
        "GEAR_PostCard_TM_AndresPeak",
        "GEAR_PostCard_TM_TailSection",
        "GEAR_BackerNote1A",
        "GEAR_BackerNote1B",
        "GEAR_BackerNote1C",
        "GEAR_BackerNote2A",
        "GEAR_BackerNote2B",
        "GEAR_BackerNote2C",
        "GEAR_BackerNote3A",
        "GEAR_BackerNote3B",
        "GEAR_BackerNote3C",
        "GEAR_BackerNote4A",
        "GEAR_BackerNote4B",
        "GEAR_BackerNote4C",
        "GEAR_BlackrockAdminNote",
        "GEAR_BlackrockAmmoRoomNote",
        "GEAR_BlackrockCodeNote",
        "GEAR_BlackrockSecurityNote",
        "GEAR_BlackrockTowerNote",
        "GEAR_CanneryCodeNote",
        "GEAR_CanneryMemo",
        "GEAR_CannerySurvivalPath",
        "GEAR_CanyonClimbersCaveNote",
        "GEAR_CanyonDeadClimberNote",
        "GEAR_CanyonFishingHutJournal",
        "GEAR_CanyonMinersNote",
        "GEAR_ClimbersJournal",
        "GEAR_DarkwalkerDiary1",
        "GEAR_DarkwalkerDiary2",
        "GEAR_DarkwalkerDiary3",
        "GEAR_DarkwalkerDiary4",
        "GEAR_DarkwalkerDiary5",
        "GEAR_DarkwalkerDiary6",
        "GEAR_DarkwalkerDiary7",
        "GEAR_DarkwalkerDiary8",
        "GEAR_DarkwalkerDiary9",
        "GEAR_DarkwalkerDiary10",
        "GEAR_DarkwalkerDiary11",
        "GEAR_DarkwalkerID",
        "GEAR_DeadmanNote1",
        "GEAR_DeadmanNote2",
        "GEAR_DeadmanNote3",
        "GEAR_DeadmanNote4",
        "GEAR_DeadmanNote5",
        "GEAR_VisorNoteML1",
        "GEAR_VisorNoteFM1",
        "GEAR_VisorNoteFM2",
        "GEAR_VisorNoteFM3",
        "GEAR_VisorNoteBR1",
        "GEAR_VisorNoteBR2",
        "GEAR_VisorNoteBR3",
        "GEAR_BRKey1",
        "GEAR_BRKey2",
        "GEAR_BIKey1",
        "GEAR_BIKey2",
        "GEAR_VisorNoteBI1",
        "GEAR_VisorNoteBI2",
        "GEAR_VisorNoteBI3",
        "GEAR_VisorNoteMT1",
        "GEAR_VisorNoteMT2",
        "GEAR_VisorNoteMT3",
        "GEAR_VisorNoteMTKey1",
        "GEAR_VisorNoteHRV1",
        "GEAR_VisorNoteHRV2",
        "GEAR_VisorNoteHRV3",
        "GEAR_VisorNoteHRVKey1",
        "GEAR_VisorNoteBlackrock1",
        "GEAR_VisorNoteBlackrock2",
        "GEAR_VisorNoteBlackrock3",
        "GEAR_VisorNoteBlackrockKey3",
        "GEAR_VisorNoteAC1",
        "GEAR_VisorNoteAC2",
        "GEAR_VisorNoteAC3",
        "GEAR_VisorNoteACKey1",
        "GEAR_VisorNoteDP1",
        "GEAR_VisorNoteDP2",
        "GEAR_VisorNoteDP3",
        "GEAR_VisorNoteDPKey1",
        "GEAR_VisorNoteTWM1",
        "GEAR_VisorNoteTWM2",
        "GEAR_VisorNotePV1",
        "GEAR_VisorNotePV2",
        "GEAR_VisorNotePV3",
        "GEAR_VisorNoteML2",
        "GEAR_VisorNoteML3",
        "GEAR_VisorNoteMLKey2",
        "GEAR_VisorNoteMLKey3",
        "GEAR_VisorNoteCH1",
        "GEAR_VisorNoteCH2",
        "GEAR_AirfieldCargomasterNote",
        "GEAR_AirfieldHangarNote",
        "GEAR_AirfieldJunkerNote",
        "GEAR_AirfieldTowerNote",
        "GEAR_AirfieldCabinNote",
        "GEAR_AirfieldControlNote",
        "GEAR_AirfieldGeologistNote1",
        "GEAR_AirfieldSecChiefNote1",
        "GEAR_Tale1ChiefNote1",
        "GEAR_Tale1ChiefNote2",
        "GEAR_Tale1ChiefNote3",
        "GEAR_Tale1ChiefNote4",
        "GEAR_Tale1ChiefNote5",
        "GEAR_Tale1Transcript1",
        "GEAR_Tale1Transcript2",
        "GEAR_Tale1Transcript3",
        "GEAR_Tale1Transcript4",
        "GEAR_Logbook_A01",
        "GEAR_Logbook_A02",
        "GEAR_Logbook_A03",
        "GEAR_Logbook_A04",
        "GEAR_RecipeCardFishcakes",
        "GEAR_RecipeCardPancakePeach",
        "GEAR_RecipeCardPieFishermans",
        "GEAR_RecipeCardPieForagers",
        "GEAR_RecipeCardPieMeat",
        "GEAR_RecipeCardPiePredator",
        "GEAR_RecipeCardPorridgeFruit",
        "GEAR_RecipeCardStewMeat",
        "GEAR_RecipeCardStewVegetables",
        "GEAR_LogbookTale2_A01",
        "GEAR_LogbookTale2_A02",
        "GEAR_LogbookTale2_A03",
        "GEAR_LogbookTale2_A04",
        "GEAR_ClipBoardTale2_A",
        "GEAR_ClipBoardTale2_B",
        "GEAR_Tale2GeologistNote1",
        "GEAR_Tale2_CorpseKey",
        "GEAR_Tale2RudigerNote1",
        "GEAR_Tale2SecurityChiefBones",
        "GEAR_LangstonMineKey1",
        "GEAR_Tale2_LockerKey",
        "GEAR_LangstonMineLockboxKey1",
        "GEAR_ClipBoardTale2_C",
        "GEAR_ClipBoardTale2_D",
        "GEAR_MineRegionBunkhouseNote",
        "GEAR_MineRegionPumphouseNote",
        "GEAR_PostCard_Tale02",
        "GEAR_PostCard_MR_Settlement",
        "GEAR_PostCard_MR_Window",
        "GEAR_Tale3ForemanLog1",
        "GEAR_Tale3ForemanLog2",
        "GEAR_Tale3ForemanLog3",
        "GEAR_Tale3RudigerNote1",
        "GEAR_Tale3RudigerNote2",
        "GEAR_Tale3RudigerNote3",
        "GEAR_Tale3ForemanLog4",
        "GEAR_Tale3_ForemanRefugeKey",
        "GEAR_Tale_03_PostCard_01",
        "GEAR_Tale_03_PostCard_02",
        "GEAR_Tale_03_PostCard_03",
        "GEAR_Tale_03_PostCard_04",
        "GEAR_Tale_03_PostCard_05",
        "GEAR_SecurityChiefID",
        "GEAR_MountainPassAvalancheNote",
        "GEAR_MountainPassWeatherStationNote",
        "GEAR_Rudiger_Watch",
        "GEAR_PostCard_MP_OgresTeardrop",
        "GEAR_PostCard_MP_BrokenRoad",
        "GEAR_RecipeCardSoupPotato",
        "GEAR_RecipeCardSoupRabbit",
        "GEAR_RecipeCardBarPemmican",
        "GEAR_BunkerPapers1",
        "GEAR_BunkerPapers2",
        "GEAR_BunkerPapers3",
        "GEAR_BunkerPapers4",
        "GEAR_BunkerSchematic_A",
        "GEAR_BunkerSchematic_B",
        "GEAR_BunkerSchematic_C",
        "GEAR_CougarPoster1",
        "GEAR_CougarPoster2",
        "GEAR_NaturalistNote1",
        "GEAR_NaturalistNote2",
        "GEAR_TraderWyattBonesNote",
        "GEAR_Trader_Pamphlet",
        "GEAR_BlueprintCard_BushcraftBow",
        "GEAR_BlueprintCard_WolfSkinHat",
        "GEAR_BlueprintCard_WolfSkinPants",
        "GEAR_ClipBoardBunker_A",
        "GEAR_ClipBoardBunker_B",
        "GEAR_ClipBoardBunker_C",
        "GEAR_ClipBoardBunker_D",
        "GEAR_PostCard_AF_Cabin",
        "GEAR_PostCard_AF_Plateau",
        "GEAR_RecipeCardSoupPtarmigan",
        "GEAR_IronRoadAnnaIslandMap1",
        "GEAR_IronRoadAnnaIslandMap2",
        "GEAR_IronRoadBoatMessagePlay",
        "GEAR_IronRoadChurchMap",
        "GEAR_IronRoadHIddenTrainLoreMcLeod",
        "GEAR_IronRoadTrainKey",
        "GEAR_IronRoadTrainMap",
        "GEAR_WaxCylinder1",
        "GEAR_WaxCylinder2",
        "GEAR_WaxCylinder3",
        "GEAR_WaxCylinder4",
        "GEAR_WaxCylinder5",
        "GEAR_WaxCylinder6",
        "GEAR_WaxCylinder7",
        "GEAR_WaxCylinder8",
        "GEAR_WaxCylinder9",
        "GEAR_WaxCylinder10",
        "GEAR_WaxCylinder11",
        "GEAR_WaxCylinder12",
        "GEAR_WaxCylinder13",
        "GEAR_WaxCylinder14",
        "GEAR_WaxCylinder15",
        "GEAR_WaxCylinder16",
        "GEAR_NULL",
        "Gear_Postcard",
    ];

    /// <summary>The start of every gear item's name and address, like <c>GEAR_Hatchet</c>.</summary>
    private const string GearPrefix = "GEAR_";

    /// <summary>The text that marks a catalogue key as possibly a decoration's, while finding how they're named.</summary>
    private const string DecorationHint = "decor";

    /// <summary>The most keys that look like decorations to log, so the log stays readable.</summary>
    private const int MaxDecorationKeys = 200;

    /// <summary>The full name of the type of an item's prefab, as the catalogue records it.</summary>
    private const string PrefabType = "UnityEngine.GameObject";

    /// <summary>
    /// Logs every item in the game that has no rarity and isn't in <see cref="ItemsWithoutRarities"/>, then the catalogue
    /// keys that look like decorations, which can't have a rarity yet. Items come from the game's catalogue of prefabs
    /// it can load, and the console's list of items it can add, so the whole game is checked rather than the current
    /// scene.
    /// </summary>
    internal static void Log()
    {
        try
        {
            LogMissing();
        }
        catch (Exception ex)
        {
            Mod.Logger.Log("Couldn't check for items without a rarity.", FlaggedLoggingLevel.Exception, ex);
        }
    }

    /// <summary>Does <see cref="Log"/>'s work, logging each step first so a crash shows where it stopped.</summary>
    private static void LogMissing()
    {
        var gearNames = new SortedSet<string>(StringComparer.Ordinal);
        var decorationKeys = new SortedSet<string>(StringComparer.Ordinal);
        var catalogueKeys = 0;
        var skippedAssets = 0;

        Mod.Logger.Log("Reading the game's catalogue.", FlaggedLoggingLevel.Debug);
        foreach (var (key, isPrefab) in CatalogueKeys())
        {
            catalogueKeys++;
            if (key.Contains(DecorationHint, StringComparison.OrdinalIgnoreCase))
                decorationKeys.Add(isPrefab ? key : $"{key} (not a prefab)");

            if (!key.StartsWith(GearPrefix, StringComparison.OrdinalIgnoreCase))
                continue;

            if (isPrefab)
                gearNames.Add(key);
            else
                skippedAssets++;
        }

        var fromCatalogue = gearNames.Count;
        Mod.Logger.Log($"Read {catalogueKeys} catalogue keys. Reading the console's items.", FlaggedLoggingLevel.Debug);
        gearNames.UnionWith(ConsoleGearNames());

        var missing = 0;
        foreach (var gearName in gearNames)
        {
            if (ItemsWithoutRarities.Contains(gearName) || RarityManager.GetRarity(gearName) != Rarities.None)
                continue;

            Mod.Logger.Log($"Item '{gearName}' does not have an assigned rarity.", FlaggedLoggingLevel.Debug);
            missing++;
        }

        foreach (var decorationKey in decorationKeys.Take(MaxDecorationKeys))
            Mod.Logger.Log($"Catalogue key that may be a decoration: {decorationKey}", FlaggedLoggingLevel.Debug);

        Mod.Logger.Log(
            $"Checked {gearNames.Count} items ({fromCatalogue} prefabs from {catalogueKeys} catalogue keys, the rest from the console, " +
            $"skipping {skippedAssets} GEAR_ assets that aren't prefabs): {missing} without a rarity. " +
            $"{decorationKeys.Count} keys contain '{DecorationHint}', the first {Math.Min(decorationKeys.Count, MaxDecorationKeys)} logged.",
            FlaggedLoggingLevel.Debug);
    }

    /// <summary>
    /// Gets every text key in the game's Addressables catalogues, like addresses, labels and asset IDs, and whether it
    /// leads to a prefab. A key whose locations can't be read counts as a prefab, so it's checked rather than missed.
    /// </summary>
    /// <returns>The keys, in no order, possibly repeated across catalogues.</returns>
    private static List<(string Key, bool IsPrefab)> CatalogueKeys()
    {
        List<(string Key, bool IsPrefab)> keys = [];

        foreach (var locatorInfo in Addressables.Instance.m_ResourceLocators)
        {
            var locator = locatorInfo?.Locator;
            if (locator == null)
                continue;

            var map = locator.TryCast<ResourceLocationMap>();
            if (map == null)
            {
                Mod.Logger.Log($"Skipped the catalogue '{locator.LocatorId}': it's a {locator.TryCast<Il2CppSystem.Object>()?.GetIl2CppType().FullName}.", FlaggedLoggingLevel.Debug);
                continue;
            }

            foreach (var entry in map.Locations)
            {
                var text = entry.Key?.ToString();
                if (!string.IsNullOrEmpty(text))
                    keys.Add((text, IsPrefab(entry.Value)));
            }
        }

        return keys;
    }

    /// <summary>Gets whether any of a key's locations loads a prefab, rather than a texture, material or other asset.</summary>
    /// <param name="locations">The key's locations.</param>
    /// <returns>Whether one is a prefab, or <see langword="true"/> if they can't be read.</returns>
    private static bool IsPrefab(Il2CppSystem.Collections.Generic.IList<IResourceLocation>? locations)
    {
        var list = locations?.TryCast<Il2CppSystem.Collections.Generic.List<IResourceLocation>>();
        if (list == null)
            return true;

        foreach (var location in list)
        {
            if (location?.ResourceType?.FullName == PrefabType)
                return true;
        }

        return false;
    }

    /// <summary>Gets the names of the items the console can add, if it has listed them yet.</summary>
    /// <returns>The gear item names, or none before the console has listed them.</returns>
    private static List<string> ConsoleGearNames()
    {
        List<string> names = [];

        var gearNames = ConsoleManager.m_SearchStringToGearNames;
        if (gearNames == null)
            return names;

        foreach (var gearName in gearNames.Values)
        {
            if (gearName != null && gearName.StartsWith(GearPrefix, StringComparison.OrdinalIgnoreCase))
                names.Add(gearName);
        }

        return names;
    }
}
