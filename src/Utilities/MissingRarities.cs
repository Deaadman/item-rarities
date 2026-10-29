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
        // Wintermute and Episode 5: notes, keys, maps, collectibles and quest deliveries.
        "GEAR_AstridBackPack_hangar",
        "GEAR_AuroraHatchCode",
        "GEAR_AuroraObservationNote",
        "GEAR_BankManagerHouseKey",
        "GEAR_BankVaultCode",
        "GEAR_BearSpearRelic",
        "GEAR_BlackrockConvictNote1",
        "GEAR_BlackrockConvictNote2",
        "GEAR_BlackrockConvictNote3",
        "GEAR_BlackrockMemo",
        "GEAR_Brainscan_A",
        "GEAR_Brainscan_B",
        "GEAR_Brainscan_C",
        "GEAR_Brainscan_D",
        "GEAR_Brand",
        "GEAR_BrokenRifle",
        "GEAR_BunkerClue1a",
        "GEAR_BunkerClue1b",
        "GEAR_BunkerClue1c",
        "GEAR_BunkerClue1d",
        "GEAR_CacheNote_ChurchMarshDir",
        "GEAR_CampOfficeCollectible",
        "GEAR_CargoShip_Captain_LogBook_A",
        "GEAR_CargoShip_Doctor_LogBook_A",
        "GEAR_CaveCacheNote",
        "GEAR_ChurchHymn",
        "GEAR_ChurchNoteEP1",
        "GEAR_ClipBoard_EP5DrLarsen",
        "GEAR_ClipBoard_MedicalReport_A",
        "GEAR_ClipBoard_MedicalReport_B",
        "GEAR_ClipBoard_MedicalReport_C",
        "GEAR_ClipBoard_MedicalReport_D",
        "GEAR_ConvictCorpseNote",
        "GEAR_DamCollectible1",
        "GEAR_DamControlRoomCodeNote",
        "GEAR_DamElevatorNotice",
        "GEAR_DamExitCodeNote",
        "GEAR_DamFenceKey",
        "GEAR_DamGarbageLetter",
        "GEAR_DamLockerKey",
        "GEAR_DamOfficeKey",
        "GEAR_DamTrailerBCrewNote",
        "GEAR_DespBay_CaptainNote",
        "GEAR_DespBay_CargoHoldNote",
        "GEAR_DespBay_EngineRoomNote",
        "GEAR_DespBay_Map",
        "GEAR_DespBay_SeaCharts",
        "GEAR_DespBay_ShipDoctorID",
        "GEAR_DespBay_SickBayNote",
        "GEAR_Doctor_Larsen_Diary_A",
        "GEAR_EmergencyKitNote",
        "GEAR_Ep3_ChurchArtifact",
        "GEAR_Ep3_ChurchFlyer",
        "GEAR_Ep3_ChurchMotelLetter",
        "GEAR_Ep3_ChurchNewsClipping",
        "GEAR_Ep3_ChurchThankyouLetter",
        "GEAR_Ep3_ChurchTheftReport",
        "GEAR_EP3_JoplinBunkerNote",
        "GEAR_EP3_JoplinBunkerNote2",
        "GEAR_EP3_JoplinCacheNote",
        "GEAR_EP3_PatientHistory01",
        "GEAR_EP3_PatientHistory02",
        "GEAR_EP3_PatientHistory03",
        "GEAR_EP3_PatientHistory04",
        "GEAR_EP3_PatientHistory05",
        "GEAR_EP3_PatientHistory06",
        "GEAR_Ep3ForestTalkers_FTCollectible1",
        "GEAR_Ep3ForestTalkers_FTCollectible2",
        "GEAR_Ep3ForestTalkers_FTCollectible3",
        "GEAR_Ep3ForestTalkers_PVCollectible1",
        "GEAR_Ep3ForestTalkers_PVCollectible2",
        "GEAR_Ep3ForestTalkers_PVCollectible3",
        "GEAR_Ep3HallFlyer",
        "GEAR_Ep3Rosary",
        "GEAR_Ep3TomsMap",
        "GEAR_Ep4_BlackrockEscapeNote1",
        "GEAR_Ep4_BlackrockNoisemakerNote",
        "GEAR_Ep4_BlackrockRationsNote",
        "GEAR_Ep4_ForestTalkerPlans",
        "GEAR_EP4_InfirmaryMeds",
        "GEAR_Ep4_InfirmaryMedsNote",
        "GEAR_Ep4_JaceCarCollectible",
        "GEAR_Ep4_LetterForestTalker1",
        "GEAR_Ep4_LetterForestTalker2",
        "GEAR_Ep4_LetterForestTalker3",
        "GEAR_Ep4_LetterForestTalkerFinal",
        "GEAR_Ep4_MineDangerNote",
        "GEAR_EP4_RooftopKey",
        "GEAR_Ep4_SubStationMemo",
        "GEAR_Ep4ConvictCacheNote1",
        "GEAR_Ep4GoldNugget",
        "GEAR_Ep4LockerKey1",
        "GEAR_Ep4LockerKey2",
        "GEAR_Ep4LockerKey3",
        "GEAR_Ep4LockerKey8",
        "GEAR_Ep4LostWorkerNote",
        "GEAR_Ep4MineGateKey",
        "GEAR_Ep4PowerWorkerID1",
        "GEAR_Ep4PowerWorkerID2",
        "GEAR_Ep4PrisonGateKey",
        "GEAR_Ep4ProsepectorNote",
        "GEAR_Ep4RumourLateEmployee",
        "GEAR_Ep4RumoursHatchLocation",
        "GEAR_Ep4RumoursHermitReport",
        "GEAR_Ep4RumoursLockBoxKey",
        "GEAR_Ep4RumoursPrisonRelease",
        "GEAR_Ep4RumoursRamblings1",
        "GEAR_Ep4RumoursWolfReport",
        "GEAR_Ep4RumoursWolfStudy",
        "GEAR_Ep4SawFarmNote",
        "GEAR_Ep4SteamTunnelCampNote",
        "GEAR_Ep5AtwoodCarNote",
        "GEAR_Ep5CargoShipLockerKey",
        "GEAR_Ep5FoothillsMap",
        "GEAR_Ep5GeoPlantWorkerLog",
        "GEAR_Ep5IceCaveBodyNote2",
        "GEAR_Ep5IceCaveBodyNote3",
        "GEAR_EP5JaceKey",
        "GEAR_Ep5MineSARNote",
        "GEAR_Ep5PMills_Police_Report_Memo_A",
        "GEAR_Ep5PMillsBarDiary01",
        "GEAR_Ep5PMillsClinicDrDiary01",
        "GEAR_Ep5PMillsClinicHypnosisReport01",
        "GEAR_Ep5PMillsClinicKey",
        "GEAR_Ep5PMillsClinicKeyNote01",
        "GEAR_Ep5PMillsClinicLarsenDiary01",
        "GEAR_Ep5PMillsClinicLetterToConstable01",
        "GEAR_Ep5PMillsClinicMorgueDoor01",
        "GEAR_Ep5PMillsClinicMorgueKey",
        "GEAR_Ep5PMillsClinicScansReport01",
        "GEAR_Ep5PMillsConstableArsonReport01",
        "GEAR_Ep5PMillsConstableAutopsyReport01",
        "GEAR_Ep5PMillsConstableLogBook",
        "GEAR_Ep5PMillsConstableMissingPersonLetter01",
        "GEAR_Ep5PMillsConstableReport01",
        "GEAR_Ep5PMillsConstablesGunLockerKey",
        "GEAR_Ep5PMillsCoopNoDeliveryNote01",
        "GEAR_Ep5PMillsCoopPrescription01",
        "GEAR_Ep5PMillsCoopScheduleNote01",
        "GEAR_Ep5PMillsMedicalReport01",
        "GEAR_Ep5PMillsMillSuspectSighting01",
        "GEAR_Ep5PMillsMissingPeopleFlyers01",
        "GEAR_Ep5PWildsBoatStorageKey",
        "GEAR_Ep5PWildsFishingCabinKey",
        "GEAR_Ep5SARChopperNote",
        "GEAR_EP5SupplyRoomKey",
        "GEAR_Ep5SuzukiExcavationNote",
        "GEAR_Ep5TelescopeMineCrankKey",
        "GEAR_Ep5TrainLoreNote",
        "GEAR_FarmerDepositBoxKey",
        "GEAR_FlareGunCase_hangar",
        "GEAR_FoodSupplies_hangar",
        "GEAR_ForestTalkerBloodyItem",
        "GEAR_ForestTalkerDamNote",
        "GEAR_ForestTalkerFlyer",
        "GEAR_ForestTalkerHiddenItem",
        "GEAR_ForestTalkerMap",
        "GEAR_ForestTalkerThankyou",
        "GEAR_ForestTalkerThankyou2",
        "GEAR_ForgeBlueprints",
        "GEAR_ForgeCacheNote",
        "GEAR_GMExtraSuppliesNote",
        "GEAR_GreyMotherPearls",
        "GEAR_GreyMotherTrunkKey",
        "GEAR_HankHatchCode",
        "GEAR_HankJournal1",
        "GEAR_HankJournal2",
        "GEAR_HankLockboxKey",
        "GEAR_HankNeiceLetter",
        "GEAR_HardCase",
        "GEAR_HardCase_hangar",
        "GEAR_HC_EP1_FM_TreeRoots_Dir",
        "GEAR_HC_EP1_ML_AlansCave_Dir",
        "GEAR_HC_EP1_ML_ClearCut_Dir",
        "GEAR_HC_EP1_ML_TracksEnt_Dir",
        "GEAR_HC_EP1_RW_HunterLodge_Dir",
        "GEAR_HC_EP1_RW_RavineEnd_Dir",
        "GEAR_Insulin",
        "GEAR_KnowledgeBRbook1",
        "GEAR_KnowledgeBRbook2",
        "GEAR_KnowledgeBRbook3",
        "GEAR_KnowledgeCarterDam",
        "GEAR_KnowledgeCollapse1",
        "GEAR_KnowledgeCollapse2",
        "GEAR_KnowledgeCollapse3",
        "GEAR_KnowledgeCollapse4",
        "GEAR_KnowledgeMilton",
        "GEAR_KnowledgeMysteryLake",
        "GEAR_KnowledgePVbook1",
        "GEAR_KnowledgePVbook2",
        "GEAR_KnowledgePVbook3",
        "GEAR_LakeCabinKey1",
        "GEAR_LakeCabinKey2",
        "GEAR_LakeCabinKey3",
        "GEAR_LakeCabinKey8",
        "GEAR_LakeDeerHuntNote",
        "GEAR_LakeIncidentNote",
        "GEAR_LakeLetter1",
        "GEAR_LakesFlareCacheNote",
        "GEAR_LakeTrailerKey1",
        "GEAR_LakeWolfCullNote",
        "GEAR_LetterBundle",
        "GEAR_LilyClimbMap",
        "GEAR_LoreNoteA",
        "GEAR_LoreNoteB",
        "GEAR_LoreNoteC",
        "GEAR_LoreNoteD",
        "GEAR_MedicalSupplies",
        "GEAR_MedicalSupplies_hangar",
        "GEAR_MiltonCaveLetter",
        "GEAR_MiltonDepositBoxKey1",
        "GEAR_MiltonDepositBoxKey2",
        "GEAR_MiltonDepositBoxKey3",
        "GEAR_MiltonFlareGunNote",
        "GEAR_MiltonGardenCache",
        "GEAR_MiltonLetter1",
        "GEAR_MiltonLetter2",
        "GEAR_MiltonParkNotice",
        "GEAR_MiltonPostOfficeCollectible1",
        "GEAR_MiltonStoreNotice",
        "GEAR_MountainTownFarmNote",
        "GEAR_MountainTownLockBoxKey",
        "GEAR_NoteMCU",
        "GEAR_OldLadyStolenItem",
        "GEAR_PassengerManifest",
        "GEAR_PlaneCrashID1",
        "GEAR_PlaneCrashID10",
        "GEAR_PlaneCrashID2",
        "GEAR_PlaneCrashID3",
        "GEAR_PlaneCrashID4",
        "GEAR_PlaneCrashID5",
        "GEAR_PlaneCrashID6",
        "GEAR_PlaneCrashID7",
        "GEAR_PlaneCrashID8",
        "GEAR_PlaneCrashID9",
        "GEAR_PlantSurvivalNote",
        "GEAR_PostCard_MP1_ForwardCamp",
        "GEAR_PostCard_MP2_Basement",
        "GEAR_PostCard_MP3_HighPeak",
        "GEAR_PostCard_MP4_Bunker",
        "GEAR_PrisonBusNote",
        "GEAR_PWilds_IceCaveBodyNote",
        "GEAR_PWilds_LakeCabinNote",
        "GEAR_PWilds_RiverBodyNote",
        "GEAR_PWilds_WaterfallBodyNote",
        "GEAR_RuralRegionFarmKey",
        "GEAR_SARStationKey",
        "GEAR_SARStationNote",
        "GEAR_ShedCodeNote",
        "GEAR_SurvivalSchoolClothing",
        "GEAR_SurvivalSchoolDeerHunt",
        "GEAR_SurvivalSchoolFishing",
        "GEAR_SurvivalSchoolPlants",
        "GEAR_SurvivalSchoolRabbits",
        "GEAR_SurvivalSchoolWolfHunt",
        "GEAR_TrailerSupplies",
        "GEAR_TraumaKit",
        "GEAR_UtilitiesBill",
        "GEAR_WaterTowerNote",
        // Test items, placeholders and carcasses that aren't in survival.
        "GEAR_TalesTestNote1",
        "Gear_Body_Dummy",
        "GEAR_CougarCarcass",
        "GEAR_WolfStarvingCarcass",
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
