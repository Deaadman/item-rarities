<p align="center">
    <a href="#"><img src="https://raw.githubusercontent.com/Deaadman/ItemRarities/release/Images/MainHeading.png"></a>

---

<div align="center">

[![Latest Release](https://img.shields.io/github/v/release/Deaadman/ItemRarities?label=Latest%20Release&style=for-the-badge)](https://github.com/Deaadman/ItemRarities/releases/latest)

[![Total Downloads](https://img.shields.io/github/downloads/Deaadman/ItemRarities/total.svg?style=for-the-badge)](https://github.com/Deaadman/ItemRarities/releases)
[![Latest Downloads](https://img.shields.io/github/downloads/Deaadman/ItemRarities/latest/total.svg?style=for-the-badge)](https://github.com/Deaadman/ItemRarities/releases)

</div>

---

## GENERAL INFORMATION / FEATURES:

Item Rarities is a modification that gives each item within [**The Long Dark**](https://www.hinterlandgames.com/the-long-dark/) a sense of exclusivity.

#### General Features
- **Rarity Classifications:** Each item is given a 'rarity' based on several different factors.
- **Exclusivity:** Exclusive items will now 'feel' rarer once obtained.

#### HUD / UI Changes
- **Sort By Rarity:** You can now sort by the rarity of items in your inventory or containers!
- **Container Grid:** Whenever an item is selected or hovered over in the container grid, then the colour of that grid item changes.
- **Inventory Grid:** Whenever an item is selected or hovered over, it's grid colour changes, and it displays a label above the item.
- **Clothing Grid:** If a clothing item is selected or hovered over, the grid colour changes, and it displays a label above the item.
- **Radial Menu:** The radial menu changes colour based on what item is hovered over, while displaying a label of what rarity it is.
- **Inspect:** The inspect label is now integrated with the rest of the information that fades in whenever an item is inspected.
- **Hovering Label:** Whenever an item is hovered over before picking up, the label changes to the colour of that item's rarity.
- **Crafting Menu:** When a craftable item is selected in the crafting menu, a label displays what rarity it is.
- **Cooking Menu:** If a cookable item is selected within the cooking menu, a label displays what rarity it is.
- **Milling Menu:** Once a millable item is selected within it's menu, a label displays the current rarity of that item.

#### Customisable Options
- **Custom Colours:** Players can now choose any colours for each rarity, to suit them to your liking.

#### Modding Support
- **ModComponent SDK:** Now compatible with any custom items made with the **[ModComponent SDK](https://github.com/Deaadman/ModComponentSDK)**.
- **Programming Support:** Give your custom items custom rarities through programming. Visit the **[developers](https://github.com/Deaadman/ItemRarities?tab=readme-ov-file#developers)** section for more information.

---

## COMPATIBILITY:

### Required Dependencies:
For optimal functionality of this modification, ensure you have the following versions or newer for the latest version of this modification.

- [**The Long Dark**](https://store.steampowered.com/news/app/305620) - Version: **v2.39**
- [**MelonLoader**](https://github.com/LavaGang/MelonLoader/releases) - Version: **v0.6.6**
- [**Localization Utilities**](https://github.com/dommrogers/LocalizationUtilities/releases) - Version: **v2.0.1**
- [**Mod Settings**](https://github.com/DigitalzombieTLD/ModSettings) - Version: **v2.0.0**

### Incompatible Mods:

Currently, no mods are incompatible, but when there are some, avoid using the following mods with this modification as combining this mod with incompatible ones might result in game crashes, data loss, or unforeseen issues.

---

## INSTALLATION:

1. [**Download**](https://github.com/LavaGang/MelonLoader/releases/latest/download/MelonLoader.Installer.exe) the latest version of MelonLoader.
2. [**Download**](https://github.com/Deaadman/ItemRarities/releases/latest/download/ItemRarities.dll) the latest version of this modification.
3. Navigate to the game's mod directory: `[Path to The Long Dark Installation]/mods`.
4. Copy the `ItemRarities.dll` from your `Downloads` folder and paste it into the `mods` directory.
5. Launch the game. The mod should be enabled.

---

## **CONTRIBUTIONS**:

### Translations:
- [**deepsnowland**](https://github.com/deepsnowland) - For providing Japanese translations.
- [**Elderly-Emre**](https://github.com/Elderly-Emre) - For providing Turkish translations.
- **Laki** - For providing Polish translations.
- [**LettereUniche**](https://github.com/LettereUniche) - For providing Italian translations.
- **LordKai1102** - For providing German translations.
- [**Mezav23**](https://github.com/mezav23) - For providing Spanish translations.

---

## **DEVELOPERS**:

This section is for anybody who wants to add a rarity to any of their custom items through code, it's pretty simple - you just have to follow what's below. However, if you are currently using the **[ModComponent SDK](https://github.com/Deaadman/ModComponentSDK)** then you'll be better off using the integrated functionality as seen **[here]()**.

If you have a mod that already contains some code, you'll need to install the **[NuGet](https://www.nuget.org/packages/ItemRarities)** package into your project. After doing so, it's as simple as including this within your project.

```csharp
using ItemRarities.Enums;
using ItemRarities.Managers;

internal sealed class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
        RarityManager.AddGearItemAndRarity("GEAR_ExampleItem", Rarities.Mythic);
        RarityManager.AddGearItemAndRarity("GEAR_ExampleItem2", Rarities.Legendary);
    }
}
```

> [!NOTE]
> This modification is not officially a part of The Long Dark and is not affiliated with Hinterland Studio Inc or its affiliates.
